using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace YarnCompilerTool
{
    class Location
    {
        public Vector2 Position { get; }
        public bool WasTaken { get; set; }

        public Location(Vector2 position, bool wasTaken)
        {
            Position = position;
            WasTaken = wasTaken;
        }
    }
}
