using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Luna.Extensions
{
    static class SKTypefaceExtensions
    {
        public static SKTypeface FromEmbeddedResource(string resourceID)
        {
            Assembly assembly = typeof(SKTypefaceExtensions).GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                return SKTypeface.FromStream(stream);
            }
        }
    }
}
