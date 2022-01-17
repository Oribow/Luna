using Luna.Biz.DataAccessors;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luna.Biz.DataTransferObjects;
using Luna.Biz.Locations;

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

                player.CurrentScene = starterScene;
                player.PrevScene = starterScene;
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

        public async Task<PlayerStateDTO> LetPlayerTravelTo(int playerId, int sceneId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players
                    .Where(p => p.Id == playerId).FirstAsync();

                player.PrevSceneId = player.CurrentSceneId;
                player.CurrentSceneId = sceneId;
                player.LockoutStartUTC = DateTime.UtcNow;
                player.LockoutEndUTC = player.LockoutStartUTC + new TimeSpan(0, 0, 5);
                await context.SaveChangesAsync();

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
                    .Include(p => p.CurrentScene)
                    .Where(p => p.Id == playerId).FirstAsync();

                if (player.GameState == GameState.Dead)
                    throw new InvalidOperationException("Player is already dead");

                player.GameState = GameState.Dead;
                player.LockoutEndUTC = DateTime.UtcNow + new TimeSpan(0, 1, 0);
                player.LockoutStartUTC = DateTime.UtcNow;

                context.QuestLogs.RemoveRange(context.QuestLogs.Where(q => q.PlayerId == playerId && q.SceneDataId == player.CurrentScene.SceneDataId));

                await context.SaveChangesAsync();

                return new PlayerStateDTO(player.GameState, player.CurrentSceneId.Value, player.PrevSceneId.Value, player.LockoutEndUTC, player.LockoutStartUTC);
            }
        }
    }
}
