using System;
using System.IO;
using System.Collections.Generic;

namespace Mahjong
{
    class ReverseGenerator : IGenerator
    {
        private string _setupFile;

        public ReverseGenerator(string file)
        {
            _setupFile = file;
        }

        public void Generate(Field field, TileType[] types)
        {
            field.Tiles = new List<Tile>();
            
            // Place the full set with bogus type ids
            LoadStructure(field, types[0], _setupFile);

            List<TilePair> reversed = new List<TilePair>();
            List<Tile> removables = new List<Tile>();
            while (field.Tiles.Count > 0)
            {
                // find two random outer tiles, remove them and store the coords in a list
                removables.AddRange(ExtractRemovableTiles(field));

                while (removables.Count > 1)
                    reversed.Add(TilePair.FetchPair(removables));
            }

            // 4. reverse the list or read the coords from behind and fill up the field with randonly generated id pairs!
            Random random = new Random();
            for (int i = reversed.Count - 1; i >= 0; i--)
            {
                int typeIndex = random.Next() % types.Length;
                reversed[i].Tile1.Type = types[typeIndex];
                reversed[i].Tile2.Type = types[typeIndex];
                field.Tiles.Add(reversed[i].Tile1);
                field.Tiles.Add(reversed[i].Tile2);
            }
        }

        private void LoadStructure(Field field, TileType type, string path)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] splitted = line.Split(' ');
                if (splitted.Length != 3) continue;
                Tile tile = new Tile(int.Parse(splitted[0]), int.Parse(splitted[1]), int.Parse(splitted[2]), type);
                field.Tiles.Add(tile);
            }

            if (field.Tiles.Count % 2 != 0)
                throw new Exception("Invalid odd tile count!");
        }

        private List<Tile> ExtractRemovableTiles(Field field)
        {
            List<Tile> removables = new List<Tile>();

            for (int i=0; i<field.Tiles.Count; i++)
                if (field.CanMove(field.Tiles[i]))
                {
                    removables.Add(field.Tiles[i]);
                    field.Tiles.RemoveAt(i);
                }

            return removables;
        }
    }
}
