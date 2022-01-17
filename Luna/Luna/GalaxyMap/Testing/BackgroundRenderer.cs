using Luna.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class BackgroundRenderer : Renderer
    {
        private const int TileSize = 512;
        private const int TileCount = 64;
        private const int TileRows = 8;
        private const int TileColumns = 8;

        SKBitmap[,] tiles;
        SKPaint starFieldPaint = new SKPaint()
        {
            Color = Color.Red.ToSKColor(),
        };
        SKRuntimeEffectUniforms starFieldShaderInputs;
        SKRuntimeEffect starFieldEffect;
        SKShader backgroundShader;

        public BackgroundRenderer(IGalaxyMapDataProvider dataProvider) : base(dataProvider)
        {
            LoadTiles();
            LoadShaders();
        }

        public override void Draw(SKCanvas canvas, float zoomLevel)
        {
            //canvas.Clear(Color.Black.ToSKColor());
            /*SKPoint offset = new SKPoint(TileColumns * TileSize * 0.5f, TileRows * TileSize * 0.5f);
            for (int x = 0; x < TileRows; x++)
            {
                for (int y = 0; y < TileColumns; y++)
                {
                    canvas.DrawBitmap(tiles[x, y], new SKPoint(TileSize * x, TileSize * y) - offset);
                }
            }*/

            var localBounds = canvas.LocalClipBounds;

            // draw star field on top
            starFieldShaderInputs["iResolution"] = new float[] { localBounds.Width, localBounds.Height };
            starFieldShaderInputs["iZoom"] = new float[] { zoomLevel };
            starFieldShaderInputs["iPos"] = new float[] { localBounds.Left, localBounds.Top };

            var starFieldShader = starFieldEffect.ToShader(false, starFieldShaderInputs);
            var combinedShader = SKShader.CreateCompose(starFieldShader, backgroundShader, SKBlendMode.Plus);

            starFieldPaint.Shader = backgroundShader;
            canvas.DrawRect(localBounds, starFieldPaint);
        }

        private void LoadTiles()
        {
            tiles = new SKBitmap[TileColumns, TileRows];
            for (int x = 0; x < TileRows; x++)
            {
                for (int y = 0; y < TileColumns; y++)
                {
                    int i = x + y * TileRows;
                    tiles[x, y] = SKBitmapExtensions.LoadBitmapResource(GetType(), $"Luna.GalaxyMap.Assets.Background.tile{i:000}.png");
                }
            }
        }

        private void LoadShaders()
        {
            // background
            var background = SKBitmapExtensions.LoadBitmapResource(GetType(), "Luna.GalaxyMap.Assets.Nebula Blue.png");
            backgroundShader = SKShader.CreateBitmap(background, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

            // starfield
            starFieldEffect = ShaderLibrary.Compile(ShaderLibrary.StarFieldBackground);
            starFieldShaderInputs = new SKRuntimeEffectUniforms(starFieldEffect);
        }
    }
}
