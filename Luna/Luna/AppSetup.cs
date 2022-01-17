using Autofac;
using Luna.Biz;
using Luna.Biz.DataAccessors.Scenes;
using Luna.Biz.Services;
using Luna.Database;
using Luna.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
    public class AppSetup
    {
        protected IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule(new LunaModule());
            builder.RegisterType<NotifyingGameStateService>();
        }

        public async Task Setup()
        {
            var container = CreateContainer();
            await SetupWithContext(container);
            App.Container = container;
        }

        protected virtual async Task SetupWithContext(IContainer container)
        {
            var contextFactory = container.Resolve<IDbContextFactory<LunaContext>>();
            var playerService = container.Resolve<PlayerService>();

            await GameCreator.StartNewGame(contextFactory, playerService);
            //await GameCreator.EnsureGameExist(contextFactory, playerService);
        }
    }
}
