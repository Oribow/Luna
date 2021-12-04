using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Data
{
    public class DataModule : Module
    {
        string dbConnectionString;

        public DataModule(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new LunaContextFactory(dbConnectionString)).As<IDbContextFactory<LunaContext>>().SingleInstance();
            
        }
    }
}
