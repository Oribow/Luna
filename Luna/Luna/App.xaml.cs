using Autofac;
using Autofac.Core;
using Luna.Biz.Models;
using Luna.Biz.Services;
using Luna.Death;
using Luna.GalaxyMap;
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

        public static IContainer Container { get; set; }

        public App()
        {
            InitializeComponent();
        }

        public async Task OpenLandingPage()
        {
            var gss = Container.Resolve<PlayerService>();
            var gameState = await gss.GetPlayersState(PlayerId);

            switch (gameState.State)
            {
                case GameState.Alive:
                    MainPage = new NavigationPage(new GalaxyMapPage());
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
