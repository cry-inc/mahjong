using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mahjong
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.Field = new Field(new TurtleGenerator());
        }

        private void saveTileFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";

            if (panelView.Field == null)
                return;

            if (panelView.Field.Tiles.Count % 2 != 0)
                MessageBox.Show("Number of tiles modulo two is not zero!");

            foreach (Tile tile in panelView.Field.Tiles)
                text += tile.X + " " + tile.Y + " " + tile.Z + " " + tile.Orientation + "\n";

            File.WriteAllText("dump.txt", text);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.Field.Tiles.Clear();
            panelView.Invalidate();
        }
    }
}
