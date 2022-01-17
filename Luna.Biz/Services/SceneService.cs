using Luna.Biz.DataAccessors;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Luna.Biz.DataTransferObjects;
using Luna.Biz.DataAccessors.Scenes;
using Luna.Biz.Locations;
using System.Numerics;
using Luna.Biz.Extensions;

namespace Luna.Biz.Services
{
    public class SceneService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneDataRepository sceneRepo;

        Random random = new Random();

        internal SceneService(ISceneDataRepository sceneRepo, IDbContextFactory<LunaContext> contextFactory)
        {
            this.sceneRepo = sceneRepo;
            this.contextFactory = contextFactory;
        }

        public async Task<SceneDataInfoDTO> GetSceneDataInfo(int sceneId)
        {
            using( var context = contextFactory.CreateDbContext())
            {
                var ass = await context.AssignedScenes.FindAsync(sceneId);
                var loc = await sceneRepo.GetSceneData(ass.SceneDataId);
                return new SceneDataInfoDTO(loc.Id, loc.Name, loc.ResolveAssetPath(loc.BackgroundImage));
            }   
        }

        public async Task<LocationDTO[]> GetLocations(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var scenes = await context.AssignedScenes
                    .Where(sc => sc.PlayerId == playerId)
                    .ToArrayAsync();

                return scenes.Select(sc => new LocationDTO(sc.Position, sc.LocationName, sc.HasBeenVisited, sc.Id)).ToArray();
            }
        }

        public async Task ArriveAtScene(int playerId, int sceneId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var scene = await context.AssignedScenes.FindAsync(sceneId);

                await ArriveAtScene(playerId, scene, context, random.Next(0, 4));
            }
        }

        internal async Task<AssignedScene> CreateStarterScene(int playerId, LunaContext context)
        {
            var starterSceneData = await sceneRepo.GetSceneData(sceneRepo.ListScenes().First());
            Vector2 pos = LocationHelper.GetRandomPointInRect(Vector2.Zero, new Random());
            var starterScene = new AssignedScene(starterSceneData.Id, pos.X, pos.Y, playerId, starterSceneData.Name);
            context.AssignedScenes.Add(starterScene);
            await context.SaveChangesAsync();

            await ArriveAtScene(playerId, starterScene, context, 2);

            await context.SaveChangesAsync();

            return starterScene;
        }

        internal Guid[] SelectUnassignedSceneDatas(ISet<Guid> assignedSceneDataIds, int amount)
        {
            var candidates = sceneRepo.ListScenes()
                .Where(g => !assignedSceneDataIds.Contains(g))
                .ToArray();

            candidates.Shuffle(random);
            amount = Math.Min(amount, candidates.Length);
            Guid[] result = new Guid[amount];

            for (int i = 0; i < amount; i++)
            {
                result[i] = candidates[i];
            }
            return result;
        }

        internal async Task ArriveAtScene(int playerId, AssignedScene scene, LunaContext context, int locationCountToGenerate)
        {
            if (scene.HasBeenVisited)
                return;
            scene.HasBeenVisited = true;
            var assignedScenes = await context.AssignedScenes
                .Where(sc => sc.PlayerId == playerId)
                .ToArrayAsync();
            var assignedScenesPositions = assignedScenes
                .Select(sc => sc.Position)
                .ToArray();

            // generate new locations around this scene
            LocationHelper locationHelper = new LocationHelper(scene.Position, assignedScenesPositions);

            int limit = sceneRepo.SceneCount - assignedScenes.Length;

            // if there aren't any more scenes to assign
            // this is dangerous, what happens when new scenes get added?
            if (limit > 0)
            {
                limit = Math.Min(locationCountToGenerate, limit);
                var newPositions = locationHelper.GeneratePossibleNewLocationsAroundOwnPos(limit);

                int unvisitedSceneCount = assignedScenes.Where(sc => !sc.HasBeenVisited).Count();

                // if we failed to find a free space around the current location, use some force
                if (unvisitedSceneCount == 0 && newPositions.Length == 0)
                {
                    newPositions = locationHelper.ForceGenerateNewLocationsAnywhere(limit);
                }

                var assignedSceneIds = assignedScenes
                    .Select(sc => sc.SceneDataId)
                    .ToHashSet();
                var sceneDataToAssign = SelectUnassignedSceneDatas(assignedSceneIds, newPositions.Length);
                for (int i = 0; i < sceneDataToAssign.Length; i++)
                {
                    var sceneData = await sceneRepo.GetSceneData(sceneDataToAssign[i]);
                    context.AssignedScenes.Add(new AssignedScene(sceneDataToAssign[i], newPositions[i].X, newPositions[i].Y, playerId, sceneData.Name));
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
