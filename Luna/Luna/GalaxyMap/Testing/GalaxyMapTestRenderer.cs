using Luna.Extensions;
using SkiaScene;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class GalaxyMapTestRenderer : ISKSceneRenderer
    {
        Stopwatch stopwatch = new Stopwatch();
        SKRuntimeEffect starEffect;
        GalaxyMapViewModel galaxy;

        public GalaxyMapTestRenderer(GalaxyMapViewModel galaxyMapViewModel)
        {
            stopwatch.Start();
            this.galaxy = galaxyMapViewModel;
        }

        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            CreateShaders();
            canvas.Clear(Color.Black.ToSKColor());
            foreach (var sol in galaxy.SolarSystems)
                DrawSolarSystem(canvas, sol.Position);
        }

        void CreateShaders()
        {
            string starShaderCode = ShaderLibrary.SmoothStep + @"
uniform float2 iResolution; 
uniform float2 iPos; 

half4 main(float2 fragcoord) {
  float2 uv = (fragcoord - iPos.xy) / iResolution.y;
  float d = length(uv);
  float m = 0.03 / d;
  m *= _smoothstep(0.5, 0.2, d);
  return half4(m, m, m, m);
}";
            string errors;
            starEffect = SKRuntimeEffect.Create(starShaderCode, out errors);
            if (errors != null)
            {
                Debug.WriteLine(errors);
            }
        }

        void DrawSolarSystem(SKCanvas canvas, SKPoint pos)
        {
            Random random = new Random((int)pos.X);

            float sunSize = 200;
            var inputs = new SKRuntimeEffectUniforms(starEffect);
            inputs.Add("iResolution", new float[] { sunSize, sunSize });
            inputs.Add("iPos", new float[] { pos.X, pos.Y });
            //inputs.Add("uTime", (float)stopwatch.Elapsed.TotalSeconds);

            var starShader = starEffect.ToShader(false, inputs);
            SKPaint sun = new SKPaint()
            {
                Shader = starShader,
                BlendMode = SKBlendMode.Plus
            };

            SKPaint planet = new SKPaint()
            {
                Color = Color.Green.ToSKColor(),
                Style = SKPaintStyle.Fill
            };

            SKPaint ring = new SKPaint()
            {
                Color = Color.Gray.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };

            canvas.DrawRect(pos.X - sunSize * 0.5f, pos.Y - sunSize * 0.5f, sunSize, sunSize, sun);

            double time = stopwatch.ElapsedMilliseconds / 1000.0;
            int planetCount = random.Next(2, 5);
            for (int i = 0; i < planetCount; i++)
            {
                float rad = random.Next(40, 150);
                double localTime = time * (random.NextDouble() + 0.3) + random.NextDouble() * 10;
                SKPoint planetPos = pos + new SKPoint((float)Math.Sin(localTime), (float)Math.Cos(localTime)).Mult(rad);
                canvas.DrawCircle(pos, rad, ring);
                canvas.DrawCircle(planetPos, 5, planet);
            }
        }
    }
}
