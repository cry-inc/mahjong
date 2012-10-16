using System;
using System.Drawing;
using System.Collections.Generic;

namespace Mahjong
{
    class Field
    {
        public const int WIDTH = 40;
        public const int HEIGHT = 36;

        private Dictionary<int, Tile> _tiles;
        private TileType[] _types;

        public Dictionary<int, Tile> Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public TileType[] TileTypes
        { get { return _types; } }

        public Field(IGenerator generator)
        {
            // TODO: Move type loading into generator!
            _types = TileType.LoadTileTypes("Tiles/tiles.txt");
            generator.Generate(this, _types);
        }

        private int CalcTileIndex(int x, int y, int z)
        {
            return z * WIDTH * HEIGHT + y * WIDTH + x;
        }

        public void Add(Tile tile)
        {
            int index = CalcTileIndex(tile.X, tile.Y, tile.Z);
            _tiles.Add(index, tile);
        }

        public void Remove(Tile tile)
        {
            int index = CalcTileIndex(tile.X, tile.Y, tile.Z);
            if (tile != _tiles[index])
                throw new Exception("Uh-oh, tile reference mismatch!");
            _tiles.Remove(index);
        }

        public int[] GetPossibleTileIndices(float x, float y, float z)
        {
            int ix = (int)Math.Floor(x);
            int iy = (int)Math.Floor(y);
            int iz = (int)Math.Floor(z);

            int[] indicies = new int[8];
            int c = 0;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                    indicies[c++] = CalcTileIndex(ix - i, iy - j, iz);

            return indicies;
        }

        public Tile FindTile(float x, float y, float z)
        {
            Tile tile = null;

            int[] indices = GetPossibleTileIndices(x, y, z);
            foreach (int i in indices)
                if (_tiles.ContainsKey(i) && _tiles[i].IsInside(x, y, z))
                    tile = _tiles[i];

            return tile;
        }

        public Tile GetTileFromCoord(float x, float y)
        {
            Tile topmostTile = null;

            // TODO: implement without iterating over all tiles!
            foreach (KeyValuePair<int, Tile> pair in _tiles)
            {
                Tile tile = pair.Value;
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
