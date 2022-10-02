using SkiaScene;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class GalaxMapRenderer2 : ISKSceneRenderer
    {
        SectorRenderer sectorRenderer;
        StarRenderer starRenderer;
        BackgroundRenderer backgroundRenderer;
        PlayerRenderer playerRenderer;
        UIRenderer uIRenderer;
        Stopwatch stopwatch;

        public GalaxMapRenderer2(IGalaxyMapDataProvider renderData)
        {
            backgroundRenderer = new BackgroundRenderer(renderData);
            sectorRenderer = new SectorRenderer(0f, 0f, 1.85f, 1.85f, renderData);
            starRenderer = new StarRenderer(0, 0, 10, 10, renderData);
            playerRenderer = new PlayerRenderer(renderData);
            uIRenderer = new UIRenderer(renderData);
            stopwatch = new Stopwatch();
            stopwatch.Start();

            new DebugDrawer();
        }

        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            float time = stopwatch.ElapsedMilliseconds / 1000f;
            //DebugDrawer.Instance.Text(301, "Zoom " + scale);
            backgroundRenderer.Draw1(canvas, scale, time);
            //canvas.Clear(Color.Black.ToSKColor());
            
            //sectorRenderer.Draw(canvas, scale);

            starRenderer.Draw1(canvas, scale, time);

            playerRenderer.Draw1(canvas, scale, time);

            starRenderer.Draw2(canvas, scale, time);

            uIRenderer.Render(canvas);

            //DebugDrawer.Instance.Circle(24, SKPoint.Empty, Color.Magenta);

            //DebugDrawer.Instance.Draw(canvas, canvas.TotalMatrix);
        }
    }
}
