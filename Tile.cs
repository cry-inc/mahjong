using System;
using System.Drawing;

namespace Mahjong
{
    class Tile
    {
        public const int WIDTH = 2;
        public const int HEIGHT = 4;
        public const int DEPTH = 1;

        private int _x;
        private int _y;
        private int _z;
        private TileType _type;

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

        public bool IsInside(float x, float y)
        {
            if (x >= _x && y >= _y && x < _x + WIDTH && y < _y + HEIGHT)
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

        public Tile(int x, int y, int z, TileType type)
        {
            _x = x;
            _y = y;
            _z = z;
            _type = type;
        }
    }
}
