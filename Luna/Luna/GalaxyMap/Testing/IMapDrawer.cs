using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    interface IMapDrawer
    {
        public void Draw(SKCanvas canvas, float zoomLevel);
    }
}
