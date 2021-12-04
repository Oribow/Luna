using Autofac;
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
        }
    }
}
