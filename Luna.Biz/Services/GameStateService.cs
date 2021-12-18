using Luna.Biz.Scenes;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luna.Biz.DataTransferObjects;

namespace Luna.Biz.Services
{
    public class GameStateService
    {
        IDbContextFactory<LunaContext> contextFactory;
        SceneService sceneService;

        internal GameStateService(IDbContextFactory<LunaContext> contextFactory, SceneService sceneService)
        {
            this.contextFactory = contextFactory;
            this.sceneService = sceneService;
        }

        public async Task<GameStateDTO> GetGameState(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Players.Where(p => p.Id == playerId)
                    .Select(p => new GameStateDTO(p.GameState, p.StateTransitionTimeUTC)).FirstAsync();
            }
        }

        public Task<SceneDTO> GetPlayerScene(int playerId)
        {
            return sceneService.GetPlayerScene(playerId);
        }

        public async Task<SceneDTO> Arrive(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Include(p => p.VisitedScenes)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState != GameState.Traveling)
                    throw new InvalidOperationException("Player isnt traveling");
                if (player.StateTransitionTimeUTC > DateTime.UtcNow)
                    throw new InvalidOperationException("Travel time insufficient to arrive");

                var visitedIds = player.VisitedScenes.Select(v => v.LocationId).ToHashSet();
                var nextSceneId = await sceneService.GetRandomUndiscoveredScene(visitedIds);

                if (nextSceneId == null)
                    throw new InvalidOperationException("No unvisited location left to arrive at.");

                player.CurrentSceneId = nextSceneId;

                if (!player.VisitedScenes.Any(v => v.LocationId == nextSceneId))
                {
                    var vs = new VisitedScene(nextSceneId.Value);
                    player.VisitedScenes.Add(vs);
                }
                player.GameState = GameState.Observing;
                await context.SaveChangesAsync();

                return await sceneService.GetSceneById(nextSceneId.Value);
            }
        }

        public async Task<GameStateDTO> StartTravelling(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Include(p => p.VisitedScenes)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState == GameState.Traveling)
                    throw new InvalidOperationException("Player is already traveling");

                // check if there would be something to arrive at
                var visitedIds = player.VisitedScenes.Select(v => v.LocationId).ToHashSet();
                bool destinationAvailable = await sceneService.HasUndiscoveredSceneLeft(visitedIds);
                if (!destinationAvailable)
                    throw new InvalidOperationException("No unvisited location left to travel to.");

                player.StateTransitionTimeUTC = DateTime.UtcNow + new TimeSpan(6, 0, 0);
                player.CurrentSceneId = null;
                player.GameState = GameState.Traveling;
                await context.SaveChangesAsync();

                return new GameStateDTO(player.GameState, player.StateTransitionTimeUTC);
            }
        }

        public async Task RevivePlayer(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState != GameState.Dead)
                    throw new InvalidOperationException("Player isnt dead");
                if (player.StateTransitionTimeUTC > DateTime.UtcNow)
                    throw new InvalidOperationException("Not enough time passed to revive");

                player.GameState = GameState.Traveling;

                await context.SaveChangesAsync();
            }
        }

        public async Task KillPlayer(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Include(p => p.VisitedScenes)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState == GameState.Dead)
                    throw new InvalidOperationException("Player is already dead");

                player.GameState = GameState.Dead;
                player.StateTransitionTimeUTC = DateTime.UtcNow + new TimeSpan(0, 1, 0);

                player.VisitedScenes.RemoveAll(vs => vs.LocationId == player.CurrentSceneId);
                context.QuestLogs.RemoveRange(context.QuestLogs.Where(q => q.PlayerId == playerId && q.LocationId == player.CurrentSceneId.Value));

                await context.SaveChangesAsync();
            }
        }
    }
}
