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
        SKPoint prevOffset;

        public PlayerRenderer(IGalaxyMapDataProvider provider) : base(provider)
        {
            Load();
        }

        public override void Draw1(SKCanvas canvas, float zoomLevel, float time)
        {
            if (dataProvider.PlayerPosition == null)
                return;

            SKPoint offset;
            if (!dataProvider.PlayerPosition.IsTraveling)
            {
                const float circleRad = 60;
                const float speed = 0.4f;
                offset = new SKPoint(MathF.Sin(time * speed), MathF.Cos(time * speed)).Mult(circleRad);
            }
            else {
                offset = new SKPoint(MathF.Max(0, prevOffset.X - 1), MathF.Max(0, prevOffset.Y - 1));
            }

            const float halfPlayerSize = 25;
            SKPoint playerPos = GetPlayerPos() + offset;
            var rect = new SKRect(playerPos.X - halfPlayerSize, playerPos.Y - halfPlayerSize, playerPos.X + halfPlayerSize, playerPos.Y + halfPlayerSize);
            canvas.DrawBitmap(playerShipTexture, rect);
            prevOffset = offset;
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
            playerShipTexture = SKBitmapExtensions.LoadBitmapResource("Luna.GalaxyMap.Assets.spaceship.png");
        }
    }
}
