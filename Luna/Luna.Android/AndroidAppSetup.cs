using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Luna;
using Luna.Biz.DataAccessors;
using Luna.Biz.DataAccessors.Scenes;
using Luna.Database;
using Luna.Droid;
using Luna.Droid.Data;
using Luna.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Droid
{
    class AndroidAppSetup : AppSetup
    {
        readonly AssetManager assetManager;

        public AndroidAppSetup(AssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        protected override async Task SetupWithContext(IContainer container)
        {
            // extract and install scenes if not done so already
            using (var stream = assetManager.Open("testdata.zip"))
            {
                SceneDataInstaller.UpdateDataFrom(stream);
            }

            var sceneRepo = container.Resolve<SceneDataRepository>();
            sceneRepo.RefreshData();

            await base.SetupWithContext(container);
        }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);

            builder.RegisterType<SceneDataRepository>()
                .As<ISceneDataRepository>()
                .As<SceneDataRepository>()
                .SingleInstance();
            builder.RegisterType<AndroidNotificationManager>().As<INotificationManager>().SingleInstance();
        }
    }
}