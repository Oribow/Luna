using Luna.Biz.Scenes;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Luna.Biz.DataTransferObjects;

namespace Luna.Biz.Services
{
    public class SceneService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneRepository sceneRepo;
        Random random = new Random();

        internal SceneService(IDbContextFactory<LunaContext> contextFactory, ISceneRepository sceneRepo)
        {
            this.contextFactory = contextFactory;
            this.sceneRepo = sceneRepo;
        }

        public async Task<SceneDTO> Arrive(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Include(p => p.VisitedScenes)
                    .ThenInclude(v => v.AvailableQuests)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (!player.IsTraveling)
                    throw new InvalidOperationException("Player isnt traveling");
                if (!player.CanArrive)
                    throw new InvalidOperationException("Travel time insufficient to arrive");

                var visitedIds = player.VisitedScenes.Select(v => v.LocationId).ToHashSet();
                var nextSceneId = await GetRandomUndiscoveredScene(visitedIds);

                if (nextSceneId == null)
                    throw new InvalidOperationException("No unvisited location left to arrive at.");

                player.CurrentSceneId = nextSceneId;

                if (!player.VisitedScenes.Any(v => v.LocationId == nextSceneId))
                {
                    var scene = await sceneRepo.Get(nextSceneId.Value);
                    var vs = new VisitedScene(nextSceneId.Value);
                    player.VisitedScenes.Add(vs);
                    vs.AvailableQuests.Add(new AvailableQuest(scene.IntroQuest));
                }
                await context.SaveChangesAsync();

                var loc = await sceneRepo.Get(nextSceneId.Value);

                return new SceneDTO(loc.Id, loc.IntroQuest, loc.Name, loc.GetImagePath(loc.BackgroundImage));
            }
        }

        public async Task<TravelStateDTO> Travel(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Include(p => p.VisitedScenes)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.IsTraveling)
                    throw new InvalidOperationException("Player is already traveling");

                // check if there would be something to arrive at
                var visitedIds = player.VisitedScenes.Select(v => v.LocationId).ToHashSet();
                bool destinationAvailable = await HasUndiscoveredSceneLeft(visitedIds);
                if (!destinationAvailable)
                    throw new InvalidOperationException("No unvisited location left to travel to.");

                player.ArrivalPossibleAfterUTC = DateTime.UtcNow + new TimeSpan(6, 0, 0);
                player.CurrentSceneId = null;
                await context.SaveChangesAsync();

                return new TravelStateDTO(player.ArrivalPossibleAfterUTC);
            }
        }

        public async Task<TravelStateDTO> GetTravelState(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var eta = await context.Players.Where(p => p.Id == playerId).Select(p => p.ArrivalPossibleAfterUTC).FirstAsync();
                return new TravelStateDTO(eta);
            }
        }

        public async Task<SceneDTO> GetCurrentScene(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var locId = await context.Players
                    .Where(p => p.Id == playerId)
                    .Select(p => p.CurrentSceneId)
                    .FirstAsync();

                var loc = await sceneRepo.Get(locId.Value);

                return new SceneDTO(loc.Id, loc.IntroQuest, loc.Name, loc.GetImagePath(loc.BackgroundImage));
            }
        }

        private Task<Guid?> GetRandomUndiscoveredScene(ISet<Guid> visitedLocations)
        {
            var candidates = sceneRepo.ListScenes()
                .Where(g => !visitedLocations.Contains(g))
                .ToArray();

            if (candidates.Length == 0)
                return Task.FromResult<Guid?>(null);

            return Task.FromResult<Guid?>(candidates[random.Next(candidates.Length)]);
        }

        private async Task<bool> HasUndiscoveredSceneLeft(ISet<Guid> visitedLocations)
        {
            var loc = await GetRandomUndiscoveredScene(visitedLocations);
            return loc.HasValue;
        }
    }
}
