using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Luna.Extensions
{
    static class SKBitmapExtensions
    {
        public static SKBitmap LoadBitmapResource(string resourceID)
        {
            Assembly assembly = typeof(SKBitmapExtensions).GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                return SKBitmap.Decode(stream);
            }
        }
    }
}
