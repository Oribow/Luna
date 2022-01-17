using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class LocationDTO
    {
        public Vector2 Position { get; }
        public string Name { get; }
        public bool HasBeenVisited { get; }
        public int SceneId { get; }

        public LocationDTO(Vector2 position, string name, bool hasBeenVisited, int sceneId)
        {
            Position = position;
            Name = name;
            HasBeenVisited = hasBeenVisited;
            SceneId = sceneId;
        }
    }
}
