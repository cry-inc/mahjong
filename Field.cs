using System;
using System.Drawing;
using System.Collections.Generic;

namespace Mahjong
{
    class Field
    {
        public const int WIDTH = 40;
        public const int HEIGHT = 36;

        private List<Tile> _tiles;
        TileType[] _types;

        public List<Tile> Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public TileType[] TileTypes
        { get { return _types; } }

        public Field(IGenerator generator)
        {
            _types = TileType.LoadTileTypes("Tiles/tiles.txt");
            generator.Generate(this, _types);
        }

        public Tile FindTile(float x, float y, float z)
        {
            Tile tile = null;

            foreach (Tile t in _tiles)
                if (t.IsInside(x, y, z))
                    tile = t;

            return tile;
        }

        public Tile GetTileFromCoord(float x, float y)
        {
            Tile topmostTile = null;

            for (int i = 0; i < _tiles.Count; i++)
            {
                Tile tile = _tiles[i];
                if (tile.IsInside(x, y))
                    if (topmostTile == null || topmostTile.Z < tile.Z)
                        topmostTile = tile;
            }

            return topmostTile;
        }

        public int GetZFromCoord(float x, float y)
        {
            int z = -1;

            Tile topmostTile = GetTileFromCoord(x, y);
            if (topmostTile != null)
                z = topmostTile.Z;

            return z;
        }

        public Point[] GetTestPoints(int x, int y)
        {
            Point[] points = new Point[WIDTH * HEIGHT];
            int p = 0;

            for (int xp = 0; xp < Tile.WIDTH; xp++)
                for (int yp = 0; yp < Tile.HEIGHT; yp++)
                    points[p++] = new Point(x + xp, y + yp);

            return points;
        }

        public int FindNewTileZ(int x, int y)
        {
            int maxZ = -1;
            Point[] points = GetTestPoints(x, y);
            foreach (Point p in points)
            {
                int z = GetZFromCoord(p.X, p.Y);
                if (z > maxZ) maxZ = z;
            }

            return maxZ + 1;
        }

        private bool CanMove(Tile tile, int xd, int yd, int zd)
        {
            Point[] points = GetTestPoints(tile.X, tile.Y);
            
            int z = tile.Z + zd;
            for (int i = 0; i < points.Length; i++)
            {
                int x = points[i].X + xd;
                int y = points[i].Y + yd;

                Tile found = FindTile(x, y, z);
                if (found != null && tile != found)
                    return false;
            }

            return true;
        }

        private bool CanMoveUp(Tile tile)
        {
            return CanMove(tile, 0, 0, 1);
        }

        private bool CanMoveRight(Tile tile)
        {
            return CanMove(tile, 1, 0, 0);
        }

        private bool CanMoveLeft(Tile tile)
        {
            return CanMove(tile, -1, 0, 0);
        }

        public bool CanMove(Tile tile)
        {
            bool up = CanMoveUp(tile);
            bool ul = up && CanMoveLeft(tile);
            bool ur = up && CanMoveRight(tile);
            return ul || ur;
        }
    }
}
