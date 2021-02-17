using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mahjong
{
    public partial class MainForm : Form
    {
        private Field _field;
        private HttpView _httpView;

        public MainForm()
        {
            InitializeComponent();

            if (Environment.CommandLine.IndexOf("-editor") != -1)
            {
                //Edit Mode
                panelView.Mode = PanelMode.Edit;
                panelView.DrawGrid = true;
                startMenuItem.Visible = false;
                browserMenuItem.Visible = false;
                ClearGame();
            }
            else {
                // Play Mode
                saveMenuItem.Visible = false;
                clearMenuItem.Visible = false;
                RestartGame();
            }
        }

        void ClearGame()
        {
            _field = new Field(new EmptyGenerator());
            panelView.Field = _field;
        }

        void RestartGame()
        {
            if (_httpView != null)
            {
                _httpView.Close();
            }
            _field = new Field(new ReverseGenerator("setup.txt", "tiles/tiles.txt"));
            _httpView = new HttpView(_field);
            panelView.Field = _field;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void playInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://127.0.0.1:8080");
        }

        private void clearMenuItem_Click(object sender, EventArgs e)
        {
            ClearGame();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";

            if (panelView.Field.Tiles.Count % 2 != 0)
            {
                MessageBox.Show("Number of tiles modulo two is not zero!");
                return;
            }

            foreach (KeyValuePair<int, Tile> pair in panelView.Field.Tiles)
                text += pair.Value.X + " " + pair.Value.Y + " " + pair.Value.Z + "\n";

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "newsetup.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
                File.WriteAllText(dialog.FileName, text);
        }
    }
}
