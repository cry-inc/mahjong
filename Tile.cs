using System;
using System.Drawing;

namespace Mahjong
{
    class Tile
    {
        public const int WIDTH = 4;
        public const int HEIGHT = 2;
        public const int DEPTH = 1;

        private int _x;
        private int _y;
        private int _z;
        private TileType _type;
        private TileOrientation _orientation;

        public int X
        { get { return _x; } }

        public int Y
        { get { return _y; } }

        public int Z
        { get { return _z; } }

        public TileType Type
        { 
            get { return _type; } 
            set { _type = value; }
        }

        public TileOrientation Orientation
        { get { return _orientation; } }

        public Rectangle Rect
        { get { return new Rectangle(_x, _y, WIDTH, HEIGHT); } }

        public bool IsInside(float x, float y)
        {
            int w = WIDTH, h = HEIGHT;
            if (_orientation == TileOrientation.Vertical)
            { w = HEIGHT; h = WIDTH; }

            if (x >= _x && y >= _y && x < _x + w && y < _y + h)
                return true;
            else
                return false;
        }

        public bool IsInside(float x, float y, float z)
        {
            if (IsInside(x, y) && z >= _z && z < _z + DEPTH)
                return true;
            else
                return false;
        }

        public Tile(int x, int y, int z, TileType type, TileOrientation orientation)
        {
            _x = x;
            _y = y;
            _z = z;
            _type = type;
            _orientation = orientation;
        }
    }
}
