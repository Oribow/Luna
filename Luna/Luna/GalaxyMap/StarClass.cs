using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap
{
    class StarClass
    {
        private static readonly StarClass[] classes = new StarClass[]
        {
            new StarClass(0.003, new SKColor(207, 255, 255), 3),
            new StarClass(0.007, new SKColor(225, 255,255), 2.6),
            new StarClass(0.01, new SKColor(255,255,255), 2.3),
            new StarClass(0.04, new SKColor(255, 252, 207), 2),
            new StarClass(0.08, new SKColor(255, 255, 107), 1.6),
            new StarClass(0.18, new SKColor(253, 120, 0), 1.4),
            new StarClass(0.69, new SKColor(255, 46, 0), 1),
        };

        public static StarInstance GenerateStarInstance(Random random)
        {
            double val = random.NextDouble();
            StarClass pickedClass = classes[0];
            for (int i = 0; i < classes.Length; i++)
            {
                if (val <= classes[i].Abundance)
                {
                    pickedClass = classes[i];
                    break;
                }
                else
                {
                    val -= classes[i].Abundance;
                }
            }


            // +/- 10%
            float scale = (float)(pickedClass.BaseSize + pickedClass.BaseSize * (random.NextDouble() - 0.5) * 2 * 0.1);
            pickedClass.BaseTint.ToHsl(out float h, out float s, out float l);
            h += h * (float)((random.NextDouble() - 0.5) * 2 * 0.1);
            SKColor tint = SKColor.FromHsl(h, s, l);
            return new StarInstance(scale, tint);
        }

        public double Abundance { get; }
        public SKColor BaseTint { get; }
        public double BaseSize { get; }

        public StarClass(double abundance, SKColor baseTint, double baseSize)
        {
            Abundance = abundance;
            BaseTint = baseTint;
            BaseSize = baseSize;
        }
    }

    class StarInstance
    {
        public float Scale { get; }
        public SKColor Tint { get; }

        public StarInstance(float scale, SKColor tint)
        {
            Scale = scale;
            Tint = tint;
        }
    }
}
