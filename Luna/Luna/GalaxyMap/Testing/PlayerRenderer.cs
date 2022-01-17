using Luna.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap.Testing
{
    class PlayerRenderer : Renderer
    {
        SKBitmap playerShipTexture;

        public PlayerRenderer(IGalaxyMapDataProvider provider) : base(provider)
        {
            Load();
        }

        public override void Draw(SKCanvas canvas, float zoomLevel)
        {
            if (dataProvider.PlayerPosition == null)
                return;

            float halfPlayerSize = 50 / zoomLevel;
            SKPoint playerPos = GetPlayerPos();
            var rect = new SKRect(playerPos.X - halfPlayerSize, playerPos.Y - halfPlayerSize, playerPos.X + halfPlayerSize, playerPos.Y + halfPlayerSize);
            canvas.DrawBitmap(playerShipTexture, rect);
        }

        private SKPoint GetPlayerPos()
        {
            if (dataProvider.PlayerPosition.IsTraveling)
            {
                float t = (float)Math.Min(1, dataProvider.PlayerPosition.PercentTraveled);
                return SKPointExtensions.Interpolate(dataProvider.PlayerPosition.PrevPosition.Position, dataProvider.PlayerPosition.Position.Position, t);
            }
            else
            {
                return dataProvider.PlayerPosition.Position.Position;
            }
        }

        private void Load()
        {
            playerShipTexture = SKBitmapExtensions.LoadBitmapResource(GetType(), "Luna.GalaxyMap.Assets.spaceship.png");
        }
    }
}
