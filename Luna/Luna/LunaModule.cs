using Autofac;
using Luna.Biz;
using Luna.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Luna
{
    class LunaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new LunaContextFactory(DatabaseCreator.ConnectionString)).As<IDbContextFactory<LunaContext>>().SingleInstance();
            builder.RegisterModule(new BizModule());
        }
    }
}
