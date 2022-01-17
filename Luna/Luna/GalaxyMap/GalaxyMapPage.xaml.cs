using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Linq;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Numerics;
using Luna.Extensions;
using SkiaScene.TouchManipulation;
using SkiaScene;
using Autofac;
using System.Threading.Tasks;
using System.Diagnostics;
using Luna.GalaxyMap.Testing;
using System.Threading;
using Luna.Biz.Services;

namespace Luna.GalaxyMap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalaxyMapPage : ContentPage
    {
        Stopwatch stopwatch = new Stopwatch();

        readonly PlayerService playerService;

        public GalaxyMapPage()
        {
            InitializeComponent();
            
            playerService = App.Container.Resolve<PlayerService>();

            BindingContext = new GalaxyMapViewModel(App.Container.Resolve<SceneService>(), playerService);
        }

        private SKPoint CanvasCoordToXamarin(SKPoint coor)
        {
            coor.X = coor.X * ((float)GalaxyMapView.Width / GalaxyMapView.CanvasSize.Width);
            coor.Y = coor.Y * ((float)GalaxyMapView.Height / GalaxyMapView.CanvasSize.Height);

            return new SKPoint(coor.X, coor.Y);
        }

        private void OpenContextWindow(SolarSystem solarSystem)
        {
            var vm = (GalaxyMapViewModel)BindingContext;
            if (vm.PlayerPosition == null || vm.PlayerPosition.IsTraveling)
                return;

            SKPoint targetPos = CanvasCoordToXamarin(GalaxyMapView.CanvasPointToViewPoint(solarSystem.Position));
            ContextWindow.IsVisible = true;

            SKPoint offset = SKPoint.Empty;
            if (targetPos.X > GalaxyMapView.Width * 0.5f)
                offset.X = -(float)ContextWindow.Width;
            if (targetPos.Y > GalaxyMapView.Height * 0.5f)
                offset.Y = -(float)ContextWindow.Height;
            targetPos += offset;

            AbsoluteLayout.SetLayoutBounds(ContextWindow, new Rectangle(targetPos.X, targetPos.Y, 0.4, AbsoluteLayout.AutoSize));
            if (vm.PlayerPosition.Position == solarSystem)
                ContextWindow.BindingContext = new VisitContextWindowViewModel(solarSystem.SceneId);
            else
                ContextWindow.BindingContext = new JumpContextWindow(solarSystem.SceneId, playerService);
        }

        private void CloseContextWindow()
        {
            ContextWindow.IsVisible = false;
        }

        private void GalaxyMapView_OnSolarSystemClicked(SolarSystem obj)
        {
            OpenContextWindow(obj);
        }

        private void GalaxyMapView_OnViewMoved()
        {
            CloseContextWindow();
        }
    }
}