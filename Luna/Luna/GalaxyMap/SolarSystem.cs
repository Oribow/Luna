using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap
{
    class SolarSystem
    {
        public SKPoint Position { get; }
        public string Name { get; }
        public bool HasBeenVisited { get; }
        public int SceneId { get; }

        public SolarSystem(SKPoint position, string name, bool hasBeenVisited, int sceneId)
        {
            Position = position;
            Name = name;
            HasBeenVisited = hasBeenVisited;
            SceneId = sceneId;
        }
    }
}
