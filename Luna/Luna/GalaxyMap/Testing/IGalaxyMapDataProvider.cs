using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    interface IGalaxyMapDataProvider
    {
        IReadOnlyList<SolarSystem> SolarSystems { get; }
        IReadOnlyList<PointOfInterest> Sectors { get; }
        PlayerPosition PlayerPosition { get; }
    }
}
