using Autofac;
using Luna.Biz.Services;
using Luna.GalaxyMap.Testing;
using Luna.GalaxyMap;
using SkiaScene.TouchManipulation;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using TouchTracking.Forms;
using System.Windows.Input;

namespace Luna.Views
{
    class GalaxyMapView : SKGLView, IGalaxyMapDataProvider
    {
        public static readonly BindableProperty SolarSystemsProperty = BindableProperty.Create(nameof(SolarSystems), typeof(IReadOnlyList<SolarSystem>), typeof(GalaxyMapView), new SolarSystem[0]);

        public static readonly BindableProperty PlayerPositionProperty = BindableProperty.Create(nameof(PlayerPosition), typeof(PlayerPosition), typeof(GalaxyMapView), null);

        public static readonly BindableProperty SectorsProperty = BindableProperty.Create(nameof(Sectors), typeof(IReadOnlyList<PointOfInterest>), typeof(GalaxyMapView), new PointOfInterest[0]);

        public static readonly BindableProperty SolarSystemClickedCommandProperty = BindableProperty.Create(nameof(SolarSystemClickedCommand), typeof(ICommand), typeof(GalaxyMapView));

        public static readonly BindableProperty ViewMovedCommandProperty = BindableProperty.Create(nameof(ViewMovedCommand), typeof(ICommand), typeof(GalaxyMapView));

        public IReadOnlyList<SolarSystem> SolarSystems
        {
            get => (IReadOnlyList<SolarSystem>)GetValue(SolarSystemsProperty);
            set => SetValue(SolarSystemsProperty, value);
        }

        public PlayerPosition PlayerPosition
        {
            get => (PlayerPosition)GetValue(PlayerPositionProperty);
            set => SetValue(PlayerPositionProperty, value);
        }

        public IReadOnlyList<PointOfInterest> Sectors
        {
            get => (IReadOnlyList<PointOfInterest>)GetValue(SectorsProperty);
            set => SetValue(SectorsProperty, value);
        }

        public ICommand SolarSystemClickedCommand
        {
            get => (ICommand)GetValue(SolarSystemClickedCommandProperty);
            set => SetValue(SolarSystemClickedCommandProperty, value);
        }

        public ICommand ViewMovedCommand
        {
            get => (ICommand)GetValue(ViewMovedCommandProperty);
            set => SetValue(ViewMovedCommandProperty, value);
        }

        public event Action<SolarSystem> OnSolarSystemClicked;
        public event Action OnViewMoved;

        ITouchGestureRecognizer touchGestureRecognizer;
        ISceneGestureResponder sceneGestureResponder;
        GalaxMapRenderer2 mapRenderer;
        SKBetterScene scene;

        public GalaxyMapView()
        {
            HasRenderLoop = true;

            mapRenderer = new GalaxMapRenderer2(this);
            scene = new SKBetterScene(mapRenderer)
            {
                MaxScale = 5,
                MinScale = 0.3f,
            };

            var touchEffect = new TouchEffect();
            touchEffect.Capture = true;
            touchEffect.TouchAction += TouchEffect_TouchAction;
            Effects.Add(touchEffect);

            touchGestureRecognizer = new TouchGestureRecognizer();
            touchGestureRecognizer.OnTap += TouchGestureRecognizer_OnTap;
            touchGestureRecognizer.OnPan += TouchGestureRecognizer_OnPan;
            sceneGestureResponder = new SceneGestureRenderingResponder(() => InvalidateSurface(), scene, touchGestureRecognizer)
            {
                TouchManipulationMode = TouchManipulationMode.IsotropicScale,
                MaxFramesPerSecond = 30,
            };
            sceneGestureResponder.StartResponding();

            SizeChanged += OnSizeChanged;
            OnSizeChanged(null, null);
        }

        public SKPoint CanvasPointToViewPoint(SKPoint canvasPoint)
        {
            return scene.GetViewPointFromCanvasPoint(canvasPoint);
        }

        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            SKPoint location = XamarinCoordToCanvas(new SKPoint(args.Location.X, args.Location.Y));
            touchGestureRecognizer.ProcessTouchEvent(args.Id, args.Type, location);
        }

        private SKPoint XamarinCoordToCanvas(SKPoint coor)
        {
            coor.X = coor.X * (CanvasSize.Width / (float)Width);
            coor.Y = coor.Y * (CanvasSize.Height / (float)Height);

            return new SKPoint(coor.X, coor.Y);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            var centerPoint = new SKPoint(this.CanvasSize.Width / 2, this.CanvasSize.Height / 2);
            scene.ScreenCenter = centerPoint;
        }

        private void TouchGestureRecognizer_OnPan(object sender, PanEventArgs args)
        {
            if (args.TouchActionType == TouchTracking.TouchActionType.Moved)
            {
                OnViewMoved?.Invoke();
                ViewMovedCommand?.Execute(null);
            }
        }

        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            //DebugDrawer.Instance.LogFrame();
            scene.Render(e.Surface.Canvas);
            //uiRenderer.Render(args.Surface.Canvas);
            //DebugDrawer.Instance.Draw(args.Surface.Canvas, args.Surface.Canvas.TotalMatrix);
        }

        private void TouchGestureRecognizer_OnTap(object sender, TapEventArgs args)
        {
            const float maxDist = 25 * 25;
            SKPoint tapPos = scene.GetCanvasPointFromViewPoint(args.ViewPoint);
            foreach (var system in ((GalaxyMapViewModel)BindingContext).SolarSystems)
            {
                if (SKPoint.DistanceSquared(tapPos, system.Position) <= maxDist)
                {
                    OnSolarSystemClicked?.Invoke(system);
                    SolarSystemClickedCommand?.Execute(system);
                    return;
                }
            }
        }
    }
}
