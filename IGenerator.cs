using System;

namespace Mahjong
{
    interface IGenerator
    {
        void Generate(Field field, TileType[] types);
    }
}
