using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Extensions
{
    static class MathExtensions
    {
        public const double Deg2Rad = Math.PI / 180;
        public const double Rad2Deg = 180 / Math.PI;

        public static double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
