using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class StarRenderer : ZoomableRenderer
    { 
        SKPaint labelPaint = new SKPaint()
        {
            Color = Color.White.ToSKColor(),
            TextSize = 9
        };
        SKPaint starPaint = new SKPaint()
        {
            BlendMode = SKBlendMode.Plus
        };
        SKRuntimeEffectUniforms starShaderInputs;
        SKRuntimeEffect starEffect;

        public StarRenderer(float minEdge0, float minEdge1, float maxEdge0, float maxEdge1, IGalaxyMapDataProvider dataProvider) : base(minEdge0, minEdge1, maxEdge0, maxEdge1, dataProvider)
        {
            LoadShaders();
        }

        protected override void ZoomedDraw(SKCanvas canvas, float zoomLevel, byte alpha)
        {
            var stars = dataProvider.SolarSystems;
            if (stars == null)
                return;

            labelPaint.Color = labelPaint.Color.WithAlpha(alpha);

            float starSize = 50;
            float starRad = 50 * 0.5f;
            float[] iPos = new float[2];
            for (int i = 0; i < stars.Count; i++)
            {
                SKPoint textPos = stars[i].Position + new SKPoint(10, -10);
                canvas.DrawText(stars[i].Name, textPos, labelPaint);


                iPos[0] = stars[i].Position.X;
                iPos[1] = stars[i].Position.Y;
                starShaderInputs["iPos"] = iPos;
                if (!stars[i].HasBeenVisited)
                {
                    starShaderInputs["tint"] = new float[4] { 0, 0, 1, 1 };
                }
                else
                {
                    starShaderInputs["tint"] = new float[4] { 1, 1, 1, 1 };
                }

                var starShader = starEffect.ToShader(false, starShaderInputs);
                starPaint.Shader = starShader;
                canvas.DrawRect(stars[i].Position.X - starRad, stars[i].Position.Y - starRad, starSize, starSize, starPaint);
            }
        }

        private void LoadShaders()
        {
            starEffect = ShaderLibrary.Compile(ShaderLibrary.Star);
            starShaderInputs = new SKRuntimeEffectUniforms(starEffect);
            starShaderInputs["iResolution"] = new float[2] { 50, 50 };
            starShaderInputs["iPos"] = new float[2];
        }
    }
}
