using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    class DebugDrawer
    {
        public static DebugDrawer Instance { get; private set; }

        Dictionary<int, Action<SKCanvas>> textQueue = new Dictionary<int, Action<SKCanvas>>();
        Dictionary<int, Action<SKCanvas>> drawQueue = new Dictionary<int, Action<SKCanvas>>();
        long lastFrameTime;
        Stopwatch stopwatch = new Stopwatch();

        public DebugDrawer()
        {
            Instance = this;
            stopwatch.Start();
        }

        float debugTextOffset;
        public void Draw(SKCanvas canvas, SKMatrix drawMatrix)
        {
            //DrawGrid(canvas);

            SKMatrix prevMatrix = canvas.TotalMatrix;
            canvas.SetMatrix(SKMatrix.Identity);
            debugTextOffset = 60;
            foreach (var cmd in textQueue.Values)
            {
                cmd.Invoke(canvas);
            }

            canvas.SetMatrix(drawMatrix);
            foreach (var cmd2 in drawQueue.Values)
            {
                cmd2.Invoke(canvas);
            }
            canvas.SetMatrix(prevMatrix);
        }

        public void Text(int controlId, string text)
        {
            Action<SKCanvas> func = (canvas) =>
            {
                SKPaint whiteText = new SKPaint()
                {
                    Color = Color.White.ToSKColor(),
                    TextSize = 50
                };
                canvas.DrawText(text, new SKPoint(0, debugTextOffset), whiteText);
                debugTextOffset += 60;
            };
            textQueue[controlId] = func;
        }

        public void Circle(int controlId, SKPoint pos, Color color, float radius = 5)
        {
            Action<SKCanvas> func = (canvas) =>
            {
                SKPaint paint = new SKPaint()
                {
                    Color = color.ToSKColor(),
                };
                canvas.DrawCircle(pos, radius, paint);
            };
            drawQueue[controlId] = func;
        }

        public void LogFrame()
        {
            float fps = 1000.0f / (stopwatch.ElapsedMilliseconds - lastFrameTime);
            lastFrameTime = stopwatch.ElapsedMilliseconds;
            Text(300, "FPS " + fps.ToString("0.00"));
        }

        private void DrawGrid(SKCanvas canvas)
        {
            SKPaint rectPaint = new SKPaint()
            {
                Color = Color.Black.ToSKColor(),
                IsStroke = true,
                StrokeWidth = 1
            };

            const int RectCount = 5;
            const int RectSize = 100;
            for (int y = -RectCount; y < RectCount; y++)
            {
                for (int x = -RectCount; x < RectCount; x++)
                {
                    canvas.DrawRect(new SKRect(x * RectSize, y * RectSize, (x + 1) * RectSize, (y + 1) * RectSize), rectPaint);
                }
            }
        }
    }
}
