using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    interface IMapDrawer
    {
        public void Draw1(SKCanvas canvas, float zoomLevel, float time);
        public void Draw2(SKCanvas canvas, float zoomLevel, float time);
    }
}
