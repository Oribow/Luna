using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    class MapRenderData
    {
        public PointOfInterest[] Sectors { get; set; } = new PointOfInterest[0];
        public PointOfInterest[] Stars { get; set; } = new PointOfInterest[0];
        public PlayerPosition PlayerPosition { get; set; } = null;
    }

    struct PointOfInterest
    {
        public string name;
        public SKPoint position;
        public bool hasBeenVisited;

        public PointOfInterest(string name, SKPoint position, bool hasBeenVisited)
        {
            this.name = name;
            this.position = position;
            this.hasBeenVisited = hasBeenVisited;
        }
    }
}
