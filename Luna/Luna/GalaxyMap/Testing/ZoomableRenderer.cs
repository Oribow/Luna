using Luna.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    abstract class ZoomableRenderer : Renderer
    {
        float minEdge0;
        float minEdge1;
        float maxEdge0;
        float maxEdge1;

        protected ZoomableRenderer(float minEdge0, float minEdge1, float maxEdge0, float maxEdge1, IGalaxyMapDataProvider dataProvider) : base(dataProvider)
        {
            this.minEdge0 = minEdge0;
            this.minEdge1 = minEdge1;
            this.maxEdge0 = maxEdge0;
            this.maxEdge1 = maxEdge1;
        }

        public override void Draw1(SKCanvas canvas, float zoomLevel, float time)
        {
            if (zoomLevel < minEdge0 || zoomLevel > maxEdge1)
                return;

            float alpha = MathFExtensions.SmoothStep(minEdge0, minEdge1, zoomLevel) * MathFExtensions.SmoothStep(maxEdge1, maxEdge0, zoomLevel);

            byte byteAlpha = (byte)(alpha * 255);
            ZoomedDraw(canvas, zoomLevel, byteAlpha);
        }

        abstract protected void ZoomedDraw(SKCanvas canvas, float zoomLevel, byte alpha);
    }
}
