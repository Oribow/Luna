using Luna.Extensions;
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
            TextSize = 24,
            TextAlign = SKTextAlign.Center,
            Typeface = SKTypefaceExtensions.FromEmbeddedResource("Luna.Assets.Fonts.Xolonium.Xolonium-Regular.ttf")
        };
        SKPaint starPaint = new SKPaint()
        {
            BlendMode = SKBlendMode.Plus
        };
        SKRuntimeEffectUniforms starShaderInputs;
        SKRuntimeEffect starEffect;

        const float BaseStarSize = 250;

        public StarRenderer(float minEdge0, float minEdge1, float maxEdge0, float maxEdge1, IGalaxyMapDataProvider dataProvider) : base(minEdge0, minEdge1, maxEdge0, maxEdge1, dataProvider)
        {
            LoadShaders();
        }

        protected override void ZoomedDraw(SKCanvas canvas, float zoomLevel, byte alpha)
        {
            var stars = dataProvider.SolarSystems;
            if (stars == null)
                return;

            //labelPaint.Color = labelPaint.Color.WithAlpha(alpha);

            float[] iPos = new float[2];
            for (int i = 0; i < stars.Count; i++)
            {
                float starSize = BaseStarSize * stars[i].Scale;

                iPos[0] = stars[i].Position.X;
                iPos[1] = stars[i].Position.Y;
                starShaderInputs["iPos"] = iPos;
                starShaderInputs["iResolution"] = new float[2] { starSize, starSize };
                starShaderInputs["tint"] = new float[] { stars[i].Tint.Red / 255f, stars[i].Tint.Green / 255f, stars[i].Tint.Blue / 255f, 1 };

                var starShader = starEffect.ToShader(false, starShaderInputs);
                starPaint.Shader = starShader;
                canvas.DrawRect(stars[i].Position.X - starSize * 0.5f, stars[i].Position.Y - starSize * 0.5f, starSize, starSize, starPaint);
            }
        }

        public override void Draw2(SKCanvas canvas, float zoomLevel, float time)
        {
            var stars = dataProvider.SolarSystems;
            if (stars == null)
                return;

            //labelPaint.Color = labelPaint.Color.WithAlpha(alpha);

            for (int i = 0; i < stars.Count; i++)
            {
                SKPoint textPos = stars[i].Position + new SKPoint(0, -30);
                canvas.DrawText(stars[i].Name, textPos, labelPaint);
            }
        }

        private void LoadShaders()
        {
            starEffect = ShaderLibrary.Compile(ShaderLibrary.Star);
            starShaderInputs = new SKRuntimeEffectUniforms(starEffect);
        }
    }
}
