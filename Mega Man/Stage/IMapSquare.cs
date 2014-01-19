using MegaMan.Common;
using MegaMan.Common.Geometry;
using MegaMan.Common.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaMan.Engine
{
    public interface IMapSquare
    {
        Tile Tile { get; }
        int X { get; }
        int Y { get; }
        float ScreenX { get; }
        float ScreenY { get; }
        RectangleF BoundBox { get; }
        RectangleF BlockBox { get; }
        TileProperties Properties { get; }
        void Draw(IRenderingContext context, int layer, float posX, float posY);
    }
}
