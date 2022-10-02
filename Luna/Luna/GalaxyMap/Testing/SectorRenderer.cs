using Luna.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class SectorRenderer : ZoomableRenderer
    {
        SKPaint labelPaint = new SKPaint()
        {
            Color = Color.White.ToSKColor(),
            TextAlign = SKTextAlign.Center
        };
        SKBitmap locationMarker;
        SKRuntimeEffectUniforms starShaderInputs;
        SKRuntimeEffect starEffect;

        public SectorRenderer(float minEdge0, float minEdge1, float maxEdge0, float maxEdge1, IGalaxyMapDataProvider dataProvider) : base(minEdge0, minEdge1, maxEdge0, maxEdge1, dataProvider)
        {
            LoadShaders();
        }

        protected override void ZoomedDraw(SKCanvas canvas, float zoomLevel, byte alpha)
        {
            if (dataProvider.Sectors == null)
                return;

            labelPaint.Color = labelPaint.Color.WithAlpha(alpha);
            float inverseZoom = 1f / zoomLevel;
            labelPaint.TextSize = 50 * inverseZoom;
            
            for (int i = 0; i < dataProvider.Sectors.Count; i++)
            {
                canvas.DrawText(dataProvider.Sectors[i].name, dataProvider.Sectors[i].position, labelPaint);
            }
            var stars = dataProvider.SolarSystems;

            // draw stars
            float markerSize = 35f / zoomLevel;
            float halfMarkerSize = markerSize * 0.5f;
            for (int i = 0; i < stars.Count; i++)
            {
                var rect = new SKRect(stars[i].Position.X - halfMarkerSize, stars[i].Position.Y - halfMarkerSize, stars[i].Position.X + halfMarkerSize, stars[i].Position.Y + halfMarkerSize);
                canvas.DrawBitmap(locationMarker, rect);
            }
        }

        private void LoadShaders() {
            locationMarker = SKBitmapExtensions.LoadBitmapResource("Luna.GalaxyMap.Assets.location_marker.png");
        }
    }
}
