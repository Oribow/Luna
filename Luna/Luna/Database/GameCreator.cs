using Autofac;
using Luna.Biz;
using Luna.Biz.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Database
{
    static class GameCreator
    {
        public static async Task StartNewGame(IDbContextFactory<LunaContext> contextFactory, PlayerService playerService)
        {
            await DatabaseCreator.RecreateDatabase(contextFactory);
            await playerService.CreatePlayer(App.PlayerId);
        }

        public static async Task EnsureGameExist(IDbContextFactory<LunaContext> contextFactory, PlayerService playerService)
        {
            await DatabaseCreator.EnsureDatabaseExists(contextFactory);
            bool doesPlayerExist = await playerService.DoesPlayerExist(App.PlayerId);

            if (!doesPlayerExist)
            {
                // create new game
                await playerService.CreatePlayer(App.PlayerId);
            }
        }
    }
}
