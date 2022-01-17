using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Luna.Extensions
{
    public static class Vector2Extensions
    {
        public static SKPoint ToSKPoint(this Vector2 vec)
        {
            return new SKPoint(vec.X, vec.Y);
        }
    }
}
