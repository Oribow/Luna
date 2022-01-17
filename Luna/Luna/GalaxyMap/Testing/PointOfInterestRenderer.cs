using Luna.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class PointOfInterestRenderer : IMapDrawer
    {
        public PointOfInterest[] POIs { get; set; }
        float minEdge0;
        float minEdge1;
        float maxEdge0;
        float maxEdge1;
        float size;

        SKPaint markerPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = Color.Green.ToSKColor()
        };

        SKPaint labelPaint = new SKPaint()
        {
            Color = Color.White.ToSKColor()
        };

        public PointOfInterestRenderer(float minEdge0, float minEdge1, float maxEdge0, float maxEdge1, float size)
        {
            this.minEdge0 = minEdge0;
            this.minEdge1 = minEdge1;
            this.maxEdge0 = maxEdge0;
            this.maxEdge1 = maxEdge1;
            this.size = size;

            labelPaint.TextSize = size;
        }

        public void Draw(SKCanvas canvas, float zoomLevel)
        {
            if (POIs == null)
                return;

            if (zoomLevel < minEdge0 || zoomLevel > maxEdge1)
                return;

            float alpha = MathFExtensions.SmoothStep(minEdge0, minEdge1, zoomLevel) * MathFExtensions.SmoothStep(maxEdge1, maxEdge0, zoomLevel);
            byte byteAlpha = (byte)(alpha * 255);

            markerPaint.Color = markerPaint.Color.WithAlpha(byteAlpha);
            labelPaint.Color = labelPaint.Color.WithAlpha(byteAlpha);
            float inverseZoom = 1f / zoomLevel;
            labelPaint.TextSize = 30 * inverseZoom;
            for (int i = 0; i < POIs.Length; i++)
            {
                canvas.DrawCircle(POIs[i].position, size * inverseZoom, markerPaint);
                SKPoint textPos = POIs[i].position + new SKPoint(10, -10);
                canvas.DrawText(POIs[i].name, textPos, labelPaint);
            }
        }
    }
}
