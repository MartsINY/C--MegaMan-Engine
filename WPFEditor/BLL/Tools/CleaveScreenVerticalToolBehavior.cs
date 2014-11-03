﻿using MegaMan.Common.Geometry;
using MegaMan.Editor.Controls;

namespace MegaMan.Editor.Bll.Tools
{
    public class CleaveScreenVerticalToolBehavior : IToolBehavior
    {
        public void Click(ScreenCanvas canvas, Point location) { }

        public void Move(ScreenCanvas canvas, Point location) { }

        public void Release(ScreenCanvas canvas, Point location)
        {
            int tilePosX = location.X / canvas.Screen.Tileset.TileSize;
            canvas.Screen.CleaveVertically(tilePosX);
        }

        public void RightClick(ScreenCanvas canvas, Point location) { }
    }
}
