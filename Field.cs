using System;
using System.Drawing;
using System.Collections.Generic;

namespace Mahjong
{
    enum PlayResult
    {
        DifferentTypes = 1,
        CanNotMoveTile = 2,
        ValidMove = 4,
        InvalidMove = 8,
        NoFurtherMoves = 16,
        Won = 32,
    }

    class Field
    {
        public const int WIDTH = 40;
        public const int HEIGHT = 36;

        private Dictionary<int, Tile> _tiles;
        private bool _started = false;
        private DateTime _startTime;
        private TimeSpan _gameTime;
        private TileType[] _types;
        private IGenerator _generator;

        public Dictionary<int, Tile> Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public TileType[] Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public TimeSpan GameTime
        {
            get { return _gameTime; }
        }

        public Field(IGenerator generator)
        {
            _generator = generator;
            _generator.Generate(this);
            if (_tiles == null || _types == null)
                throw new Exception("Generator failed. Tiles or TileTypes missing!");
        }

        public void Scramble()
        {
            _generator.Scramble(this);
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

        private static int TileDrawingOrder(Tile tile1, Tile tile2)
        {
            if (tile1.Z == tile2.Z)
            {
                if (tile1.Y == tile2.Y)
                {
                    return tile1.X - tile2.X;
                }
                else return tile1.Y - tile2.Y;
            }
            else return tile1.Z - tile2.Z;
        }

        public Tile[] GetSortedTiles()
        {
            List<Tile> tiles = new List<Tile>();
            foreach (KeyValuePair<int, Tile> pair in _tiles)
                tiles.Add(pair.Value);
            tiles.Sort(TileDrawingOrder);
            return tiles.ToArray();
        }

        public PlayResult Play(Tile tile1, Tile tile2)
        {
            if (!_started)
            {
                _startTime = DateTime.Now;
                _started = true;
            }

            if (tile1 == tile2)
                return PlayResult.InvalidMove;

            if (tile1.Type != tile2.Type)
                return PlayResult.DifferentTypes;

            if (!CanMove(tile1) || !CanMove(tile2))
                return PlayResult.CanNotMoveTile;

            Remove(tile1);
            Remove(tile2);

            if (_tiles.Count == 0)
            {
                _gameTime = DateTime.Now - _startTime;
                return PlayResult.Won;
            }

            PlayResult result = PlayResult.ValidMove;
            if (!NextMovePossible())
                result |= PlayResult.NoFurtherMoves;
            return result;
        }

        public bool NextMovePossible()
        {
            // Get all removable tiles
            List<Tile> removables = new List<Tile>();
            foreach (KeyValuePair<int, Tile> pair in _tiles)
                if (CanMove(pair.Value))
                    removables.Add(pair.Value);

            // Check if there are two with the same id in this list
            for (int i=0; i<removables.Count; i++)
                for (int j=0; j<removables.Count; j++)
                    if (j != i && removables[i].Type == removables[j].Type)
                        return true;

            return false;
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

        public TilePair GetHint()
        {
            List<Tile> tiles = new List<Tile>();
            foreach (KeyValuePair<int, Tile> pair in _tiles)
                if (CanMove(pair.Value))
                    tiles.Add(pair.Value);

            for (int i = 0; i < tiles.Count; i++)
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (i == j) continue;
                    if (tiles[i].Type == tiles[j].Type)
                        return new TilePair(tiles[i], tiles[j]);
                }

            return null;
        }
    }
}
