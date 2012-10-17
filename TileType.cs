using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace Mahjong
{
    class TileType
    {
        public const int TYPECOUNT = 4;

        private int _id;
        private string _name;

        public int Id
        { get { return _id; } }

        public string Name
        { get { return _name; } }

        private TileType(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public static TileType[] LoadTileTypes(string path)
        {
            List<TileType> tiles = new List<TileType>();
            string[] lines = File.ReadAllLines(path);
            int id = 0;
            foreach (string line in lines)
            {
                string[] splitted = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (splitted.Length < 1)
                    continue;
                string name = splitted[0];
                tiles.Add(new TileType(id++, name));
            }
            
            return tiles.ToArray();
        }
    }
}
