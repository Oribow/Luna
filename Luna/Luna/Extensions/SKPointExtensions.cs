using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Extensions
{
    static class SKPointExtensions
    {
        public static SKPoint Min(SKPoint a, SKPoint b)
        {
            return new SKPoint(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }

        public static SKPoint Max(SKPoint a, SKPoint b)
        {
            return new SKPoint(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        public static SKPoint Mult(this SKPoint a, float mult)
        {
            return new SKPoint(a.X * mult, a.Y * mult);
        }

        public static SKPoint Interpolate(SKPoint a, SKPoint b, float t)
        {
            return a + (b - a).Mult(t);
        }

        public static float RotationBetween(SKPoint a, SKPoint b)
        {
            return (float)(Math.Acos(Dot(a, b) / (a.Length * b.Length)) * MathExtensions.Rad2Deg);
        }

        public static float Dot(SKPoint a, SKPoint b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static SKPoint Up => new SKPoint(0, 1);
    }
}
