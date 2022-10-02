using Luna.Biz.DataAccessors;
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
    public class PlayerService : IPlayerService
    {
        IDbContextFactory<LunaContext> contextFactory;
        SceneService sceneService;

        internal PlayerService(IDbContextFactory<LunaContext> contextFactory, SceneService sceneService)
        {
            this.contextFactory = contextFactory;
            this.sceneService = sceneService;
        }

        public async Task CreatePlayer(int id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                Player player = new Player(id);
                context.Players.Add(player);
                await context.SaveChangesAsync();

                var starterScene = await sceneService.CreateStarterScene(id, context);

                player.CurrentSceneId = starterScene.SceneDataId;
                player.PrevSceneId = starterScene.SceneDataId;
                player.GameState = GameState.Intro;
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DoesPlayerExist(int id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return (await context.Players.FindAsync(id)) != null;
            }
        }

        public async Task<PlayerStateDTO> GetPlayersState(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Players.Where(p => p.Id == playerId)
                    .Select(p =>
                    new PlayerStateDTO(p.GameState, p.CurrentSceneId.Value,
                    p.PrevSceneId.Value,
                    p.LockoutEndUTC,
                    p.LockoutStartUTC)
                    ).FirstAsync();
            }
        }

        public async Task<PlayerStateDTO> LetPlayerTravelTo(int playerId, Guid sceneId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Where(p => p.Id == playerId).FirstAsync();

                player.PrevSceneId = player.CurrentSceneId;
                player.CurrentSceneId = sceneId;
                player.LockoutStartUTC = DateTime.UtcNow;
                player.LockoutEndUTC = player.LockoutStartUTC + (player.TravelCounter == 0 ? new TimeSpan(0, 0, 30) : new TimeSpan(6, 0, 0));
                player.TravelCounter++;
                await context.SaveChangesAsync();

                await sceneService.RevealScene(playerId, sceneId, context);

                return new PlayerStateDTO(player.GameState, player.CurrentSceneId.Value, player.PrevSceneId.Value, player.LockoutStartUTC, player.LockoutEndUTC);
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
                if (player.LockoutEndUTC > DateTime.UtcNow)
                    throw new InvalidOperationException("Not enough time passed to revive");

                player.GameState = GameState.Alive;
                await context.SaveChangesAsync();
            }
        }

        public async Task<PlayerStateDTO> KillPlayer(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState == GameState.Dead)
                    throw new InvalidOperationException("Player is already dead");

                player.GameState = GameState.Dead;
                player.LockoutEndUTC = DateTime.UtcNow + new TimeSpan(0, 0, 30);
                player.LockoutStartUTC = DateTime.UtcNow;

                context.QuestLogs.RemoveRange(context.QuestLogs.Where(q => q.PlayerId == playerId && q.SceneDataId == player.CurrentSceneId));

                await context.SaveChangesAsync();

                return new PlayerStateDTO(player.GameState, player.CurrentSceneId.Value, player.PrevSceneId.Value, player.LockoutEndUTC, player.LockoutStartUTC);
            }
        }

        public async Task<SceneDataInfoDTO[]> GetNextLocationOptions(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var currSceneId = await context.Players.Where(pl => pl.Id == playerId)
                    .Select(pl => pl.CurrentSceneId).FirstAsync();

                var takenIds = await context.RevealedScenes.Where(sc => sc.PlayerId == playerId).Select(sc => sc.SceneDataId).ToArrayAsync();

                int seed = currSceneId.Value.GetHashCode() + playerId;
                Guid[] candidates = sceneService.GetRandomUnrevealedSceneDataIds(takenIds.ToHashSet(), 3, seed);

                SceneDataInfoDTO[] locations = new SceneDataInfoDTO[candidates.Length];
                for (int i = 0; i < locations.Length; i++)
                {
                    locations[i] = await sceneService.GetSceneDataInfo(candidates[i]);
                }

                return locations;
            }
        }

        public async Task PlayerCompletedIntro(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Where(p => p.Id == playerId).FirstAsync();

                player.GameState = GameState.Alive;
                await context.SaveChangesAsync();
            }
        }
    }
}
