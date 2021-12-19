using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Luna;
using Luna.Biz.Scenes;
using Luna.Droid.Data;
using Luna.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celestia.App.Droid
{
    class AndroidAppSetup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);

            builder.RegisterType<SceneRepository>().As<ISceneRepository>();
            builder.RegisterType<AndroidNotificationManager>().As<INotificationManager>();
        }
    }
}