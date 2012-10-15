using System;
using System.Collections.Generic;

namespace Mahjong
{
    class TilePair
    {
        private static Random _random = new Random();

        public Tile Tile1;
        public Tile Tile2;

        public TilePair(Tile tile1, Tile tile2)
        {
            Tile1 = tile1;
            Tile2 = tile2;
        }

        public static TilePair FetchPair(List<Tile> tiles)
        {
            if (tiles.Count < 2)
                throw new Exception("Less than two tiles in the list!");

            int index = _random.Next() % tiles.Count;
            Tile tile1 = tiles[index];
            tiles.RemoveAt(index);

            index = _random.Next() % tiles.Count;
            Tile tile2 = tiles[index];
            tiles.RemoveAt(index);

            return new TilePair(tile1, tile2);
        }
    }
}
