using System;

namespace Mahjong
{
    interface IGenerator
    {
        void Generate(Field field);
        void Scramble(Field field);
    }
}
