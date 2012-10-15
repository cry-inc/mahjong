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

            string[] setups = ListSetups();
            foreach (string setup in setups)
            {
                ToolStripItem tsi = gameToolStripMenuItem.DropDownItems.Add(setup);
                tsi.Click += new EventHandler(tsi_Click);
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            ToolStripItem s = (ToolStripItem)sender;
            string setup = "Setups/" + s.Text + ".txt";
            panelView.Field = new Field(new ReverseGenerator(setup));
        }

        public string[] ListSetups()
        {
            string[] setups = Directory.GetFiles("Setups/");
            for (int i = 0; i < setups.Length; i++)
            {
                setups[i] = setups[i].Replace(".txt", "");
                setups[i] = setups[i].Replace("Setups/", "");
            }
            return setups;
        }

        private void saveTileFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";

            if (panelView.Field == null)
                return;

            if (panelView.Field.Tiles.Count % 2 != 0)
                MessageBox.Show("Number of tiles modulo two is not zero!");

            foreach (Tile tile in panelView.Field.Tiles)
                text += tile.X + " " + tile.Y + " " + tile.Z + "\n";

            File.WriteAllText("dump.txt", text);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panelView.Field != null)
            {
                panelView.Field.Tiles.Clear();
                panelView.Invalidate();
            }
        }
    }
}
