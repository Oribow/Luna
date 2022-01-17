using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Extensions
{
    static class MathFExtensions
    {
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static float SmoothStep(float edge0, float edge1, float value)
        {
            float t = Clamp((value - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            return t * t * (3.0f - 2.0f * t);
        }
    }
}
