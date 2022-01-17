using SkiaScene;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap
{
    class SKBetterScene : SKScene
    {
        public SKBetterScene(ISKSceneRenderer sceneRenderer) : base(sceneRenderer)
        {
        }

        public SKPoint GetViewPointFromCanvasPoint(SKPoint viewPoint)
        {
            return Matrix.MapPoint(viewPoint);
        }
    }
}
