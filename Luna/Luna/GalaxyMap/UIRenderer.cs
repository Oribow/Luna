using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    class UIRenderer
    {
        GalaxyMapViewModel mapData;

        public UIRenderer(GalaxyMapViewModel mapData)
        {
            this.mapData = mapData;
        }

        public void Render(SKCanvas canvas)
        {
            if (mapData == null)
                return;

            SKMatrix matrix = canvas.TotalMatrix;
            canvas.ResetMatrix();

            DrawTravelETA(canvas);

            canvas.SetMatrix(matrix);
        }

        void DrawTravelETA(SKCanvas canvas)
        {
            if (mapData.PlayerPosition.IsTraveling)
            {
                SKPaint font = new SKPaint()
                {
                    Color = Color.White.ToSKColor(),
                    TextSize = 50
                };

                canvas.DrawText("ETA: " + mapData.PlayerPosition.ETA.ToString(@"hh\:mm\:ss"), new SKPoint(0, 60), font);
            }
        }
    }
}
