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
        public float Scale { get; }
        public SKColor Tint { get; }

        public SolarSystem(SKPoint position, string name, Guid sceneId, float scale, SKColor tint)
        {
            Position = position;
            Name = name;
            SceneId = sceneId;
            Scale = scale;
            Tint = tint;
        }
    }
}
