using Autofac;
using Autofac.Core;
using Luna.Biz.Services;
using Luna.FarCaster;
using Luna.Observation;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna
{
    public partial class App : Application
    {
        public const int PlayerId = 1;

        public static IContainer Container { get; private set; }

        public App(AppSetup appSetup)
        {
            Container = appSetup.CreateContainer();
            InitializeComponent();
        }

        public async Task Initialize()
        {
            var gss = Container.Resolve<GameStateService>();
            var gameState = await gss.GetGameState(PlayerId);

            switch (gameState)
            {
                case GameState.Traveling:
                    MainPage = new NavigationPage(new FarCasterPage());
                    break;
                case GameState.Observing:
                    MainPage = new NavigationPage(new ObservationPage());
                    break;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
