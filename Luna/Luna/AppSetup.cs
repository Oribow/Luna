using Autofac;
using Luna.Biz.Services;
using Luna.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule(new LunaModule());
            builder.RegisterType<NotifyingGameStateService>().As<IGameStateService>();
        }
    }
}
