using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap
{
    class PlayerPosition
    {
        public SolarSystem Position { get; }
        public SolarSystem PrevPosition { get; }
        public DateTime ArrivalUTC { get; }
        public DateTime DepatureUTC { get; }

        public double PercentTraveled => IsTraveling ? (DateTime.UtcNow - DepatureUTC).TotalSeconds / (ArrivalUTC - DepatureUTC).TotalSeconds : 1;
        public bool IsTraveling => ArrivalUTC > DateTime.UtcNow;
        public TimeSpan ETA => ArrivalUTC - DateTime.UtcNow;

        public PlayerPosition(SolarSystem position, SolarSystem prevPosition, DateTime arrivalUTC, DateTime depatureUTC)
        {
            Position = position;
            PrevPosition = prevPosition;
            ArrivalUTC = arrivalUTC;
            DepatureUTC = depatureUTC;
        }
    }
}
