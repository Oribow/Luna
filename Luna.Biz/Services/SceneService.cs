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
    internal class SceneService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneRepository sceneRepo;
        Random random = new Random();

        internal SceneService(IDbContextFactory<LunaContext> contextFactory, ISceneRepository sceneRepo)
        {
            this.contextFactory = contextFactory;
            this.sceneRepo = sceneRepo;
        }

        public async Task<SceneDTO> GetSceneById(Guid sceneId)
        {
            var loc = await sceneRepo.Get(sceneId);
            return new SceneDTO(loc.Id, loc.Name, loc.ResolveAssetPath(loc.BackgroundImage));
        }

        public async Task<SceneDTO> GetPlayerScene(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var locId = await context.Players
                    .Where(p => p.Id == playerId)
                    .Select(p => p.CurrentSceneId)
                    .FirstAsync();

                return await GetSceneById(locId.Value);
            }
        }

        public Task<Guid?> GetRandomUndiscoveredScene(ISet<Guid> visitedLocations)
        {
            var candidates = sceneRepo.ListScenes()
                .Where(g => !visitedLocations.Contains(g))
                .ToArray();

            if (candidates.Length == 0)
            {
                // clear and retry Only for debug purposes
                candidates = sceneRepo.ListScenes().ToArray();
                return Task.FromResult<Guid?>(candidates[random.Next(candidates.Length)]);

                return Task.FromResult<Guid?>(null);
            }

            return Task.FromResult<Guid?>(candidates[random.Next(candidates.Length)]);
        }

        public async Task<bool> HasUndiscoveredSceneLeft(ISet<Guid> visitedLocations)
        {
            return true;
            var loc = await GetRandomUndiscoveredScene(visitedLocations);
            return loc.HasValue;
        }
    }
}
