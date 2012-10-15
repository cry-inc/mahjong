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
        private Color _color;

        public int Id
        { get { return _id; } }

        public string Name
        { get { return _name; } }

        public Color Color
        { get { return _color; } }

        private TileType(int id, string name, Color color)
        {
            _id = id;
            _name = name;
            _color = color;
        }

        public static TileType[] LoadTileTypes(string path)
        {
            List<TileType> tiles = new List<TileType>();
            string[] lines = File.ReadAllLines(path);
            int id = 0;
            foreach (string line in lines)
            {
                string[] splitted = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                string name = splitted[0];
                byte r = byte.Parse(splitted[1]);
                byte g = byte.Parse(splitted[2]);
                byte b = byte.Parse(splitted[3]);
                tiles.Add(new TileType(id++, name, Color.FromArgb(r, g, b)));
            }
            
            return tiles.ToArray();
        }
    }
}
