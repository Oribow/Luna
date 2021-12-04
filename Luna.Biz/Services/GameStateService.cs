using Luna.Biz.Scenes;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.Services
{
    public class GameStateService
    {
        IDbContextFactory<LunaContext> contextFactory;

        internal GameStateService(IDbContextFactory<LunaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<GameState> GetGameState(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                bool isTraveling = await context.Players.Where(p => p.Id == playerId).Select(p => p.CurrentSceneId == null).FirstAsync();

                if (isTraveling)
                    return GameState.Traveling;
                else
                    return GameState.Observing;
            }
        }
    }

    public enum GameState
    {
        Traveling,
        Observing
    }
}
