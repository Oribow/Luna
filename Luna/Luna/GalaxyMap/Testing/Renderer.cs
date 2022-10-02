using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    abstract class Renderer : IMapDrawer
    {
        protected readonly IGalaxyMapDataProvider dataProvider;

        public Renderer(IGalaxyMapDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public abstract void Draw1(SKCanvas canvas, float zoomLevel, float time);
        public virtual void Draw2(SKCanvas canvas, float zoomLevel, float time) { }
    }
}
