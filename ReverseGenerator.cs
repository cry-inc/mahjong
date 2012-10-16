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
            //TODO: find bug with four tiles!

            field.Tiles = new Dictionary<int, Tile>();
            
            // Place the full set with a default tile type
            LoadStructure(field, types[0], _setupFile);

            List<TilePair> reversed = new List<TilePair>();
            List<Tile> removables = new List<Tile>();
            while (field.Tiles.Count > 0)
            {
                // Find two random outer tiles, remove them and store the coords
                removables.AddRange(ExtractRemovableTiles(field));

                // Continue until no more removable tile pairs are left
                while (removables.Count > 1)
                    reversed.Add(TilePair.FetchPair(removables));
            }

            // Read the list from behind and and get random a random tile type for each pair to build the game
            Random random = new Random();
            for (int i = reversed.Count - 1; i >= 0; i--)
            {
                int typeIndex = random.Next() % types.Length;
                reversed[i].Tile1.Type = types[typeIndex];
                reversed[i].Tile2.Type = types[typeIndex];
                field.Add(reversed[i].Tile1);
                field.Add(reversed[i].Tile2);
            }
        }

        private void LoadStructure(Field field, TileType type, string path)
        {
            string[] lines = File.ReadAllLines(path);

            if (lines.Length % 2 != 0)
                throw new Exception("Invalid odd tile count!");

            foreach (string line in lines)
            {
                string[] splitted = line.Split(' ');
                if (splitted.Length != 3) continue;
                Tile tile = new Tile(int.Parse(splitted[0]), int.Parse(splitted[1]), int.Parse(splitted[2]), type);
                field.Add(tile);
            }
        }

        private List<Tile> ExtractRemovableTiles(Field field)
        {
            List<Tile> removables = new List<Tile>();

            int[] keys = new int[field.Tiles.Count];
            field.Tiles.Keys.CopyTo(keys, 0);

            foreach (int key in keys)
            {
                if (field.CanMove(field.Tiles[key]))
                {
                    removables.Add(field.Tiles[key]);
                    field.Remove(field.Tiles[key]);
                }
            }

            return removables;
        }
    }
}
