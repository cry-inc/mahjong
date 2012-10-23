using System;
using System.Collections.Generic;

namespace Mahjong
{
    class EmptyGenerator : IGenerator
    {
        public void Generate(Field field)
        {
            field.Tiles = new Dictionary<int, Tile>();
            field.Types = new TileType[0];
        }

        public void Scramble(Field field) { }
    }
}
