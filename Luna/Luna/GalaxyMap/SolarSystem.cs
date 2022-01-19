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
        public Guid SceneId { get; }

        public SolarSystem(SKPoint position, string name, Guid sceneId)
        {
            Position = position;
            Name = name;
            SceneId = sceneId;
        }
    }
}
