using Autofac;
using Autofac.Core;
using Luna.Biz.Models;
using Luna.Biz.Services;
using Luna.Death;
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

        public async Task OpenLandingPage()
        {
            var gss = Container.Resolve<IGameStateService>();
            var gameState = await gss.GetGameState(PlayerId);

            switch (gameState.State)
            {
                case GameState.Traveling:
                    MainPage = new NavigationPage(new FarCasterPage(false));
                    break;
                case GameState.Observing:
                    MainPage = new NavigationPage(new ObservationPage(false));
                    break;
                case GameState.Dead:
                    MainPage = new NavigationPage(new DeathPage());
                    break;
                default:
                    throw new Exception("No page specified for game state");
            }
        }

        protected override void OnStart()
        {
            Container.Resolve<INotificationManager>().CancelAll();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
