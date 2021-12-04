using Autofac;
using Luna.Biz.Scenes;
using Luna.Biz.QuestPlayer;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz
{
    public class BizModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var constrFinder = new AllConstructorFinder();

            builder.RegisterType<GameStateService>().FindConstructorsWith(constrFinder).SingleInstance();
            builder.RegisterType<QuestService>().FindConstructorsWith(constrFinder).SingleInstance();
            builder.RegisterType<SceneService>().FindConstructorsWith(constrFinder).SingleInstance();
        }
    }
}
