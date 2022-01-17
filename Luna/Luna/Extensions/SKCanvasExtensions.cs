using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Extensions
{
    static class SKCanvasExtensions
    {
        public static void DrawBitmapCentered(this SKCanvas canvas, SKBitmap bitmap, SKPoint center)
        {
            canvas.DrawBitmap(bitmap, center - new SKPoint(bitmap.Width, bitmap.Height).Mult(0.5f));
        }

        public static SKMatrix GetResetMatrix(this SKCanvas canvas)
        {
            var mat = canvas.TotalMatrix;
            canvas.ResetMatrix();
            return mat;
        }
    }
}
