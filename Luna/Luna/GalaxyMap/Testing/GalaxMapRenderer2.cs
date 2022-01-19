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

        public GalaxMapRenderer2(IGalaxyMapDataProvider renderData)
        {
            backgroundRenderer = new BackgroundRenderer(renderData);
            sectorRenderer = new SectorRenderer(0f, 0f, 1, 2.0f, renderData);
            starRenderer = new StarRenderer(2f, 2.1f, 10, 10.1f, renderData);
            playerRenderer = new PlayerRenderer(renderData);

            new DebugDrawer();
        }

        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            DebugDrawer.Instance.Text(301, "Zoom " + scale);
            backgroundRenderer.Draw(canvas, scale);
            //canvas.Clear(Color.Black.ToSKColor());
            sectorRenderer.Draw(canvas, scale);

            starRenderer.Draw(canvas, scale);

            playerRenderer.Draw(canvas, scale);

            DebugDrawer.Instance.Draw(canvas, canvas.TotalMatrix);
        }
    }
}
