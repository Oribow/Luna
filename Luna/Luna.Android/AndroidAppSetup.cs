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
using Luna.Data;
using Luna.Droid;
using Luna.Droid.Data;
using Luna.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luna.Droid
{
    class AndroidAppSetup : AppSetup
    {
        readonly AssetManager assetManager;

        public AndroidAppSetup(AssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);

            builder.RegisterType<SceneRepository>().As<ISceneRepository>().SingleInstance();
            builder.RegisterType<AndroidNotificationManager>().As<INotificationManager>().SingleInstance();
            builder.RegisterInstance(new PlatformBootstrapHelper(assetManager)).As<IPlatformBootstrapHelper>().SingleInstance();
        }
    }
}