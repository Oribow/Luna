using Luna.Biz.DataAccessors;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Luna.Biz.DataTransferObjects;
using System.Numerics;
using Luna.Biz.Extensions;

namespace Luna.Biz.Services
{
    public class SceneService
    {
        const string StarterSceneId = "31d8a726-0000-4e11-acad-1474434b2d21";

        IDbContextFactory<LunaContext> contextFactory;
        ISceneDataRepository sceneRepo;

        internal SceneService(ISceneDataRepository sceneRepo, IDbContextFactory<LunaContext> contextFactory)
        {
            this.sceneRepo = sceneRepo;
            this.contextFactory = contextFactory;
        }

        public async Task<SceneDataInfoDTO> GetSceneDataInfo(Guid sceneId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var loc = await sceneRepo.GetSceneData(sceneId);
                return new SceneDataInfoDTO(loc.Id, loc.Name, loc.BackgroundImage, loc.Position);
            }
        }

        public async Task<LocationDTO[]> GetLocations(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var scenes = await context.RevealedScenes
                    .Where(sc => sc.PlayerId == playerId)
                    .ToArrayAsync();

                return scenes.Select(sc => new LocationDTO(sc.Position, sc.MapName, sc.SceneDataId)).ToArray();
            }
        }

        internal Task<RevealedScene> CreateStarterScene(int playerId, LunaContext context)
        {
            return RevealScene(playerId, Guid.Parse(StarterSceneId), context);
        }

        internal async Task<RevealedScene> RevealScene(int playerId, Guid sceneId, LunaContext context)
        {
            var sceneData = await sceneRepo.GetSceneData(sceneId);

            var scene = new RevealedScene(playerId, sceneData.Id, sceneData.Position, sceneData.Name);
            context.RevealedScenes.Add(scene);
            await context.SaveChangesAsync();

            return scene;
        }

        internal Guid[] GetRandomUnrevealedSceneDataIds(ISet<Guid> revealedSceneDataIds, int amount, int seed)
        {
            var candidates = sceneRepo.ListScenes()
                .Where(g => !revealedSceneDataIds.Contains(g))
                .ToArray();

            Random r = new Random(seed);
            candidates.Shuffle(r);
            amount = Math.Min(amount, candidates.Length);
            Guid[] result = new Guid[amount];

            for (int i = 0; i < amount; i++)
            {
                result[i] = candidates[i];
            }
            return result;
        }
    }
}
