using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Mahjong
{
    enum PanelMode
    {
        Edit,
        Play,
    }

    class PanelView : Panel
    {
        public const int DRAWWIDTH = 75;
        public const int DRAWHEIGHT = 95;
        public const int CELLWIDTH = 30;
        public const int CELLHEIGHT = 20;

        private Field _field;
        private PanelMode _mode = PanelMode.Play;
        private Tile _selected;
        private Dictionary<int, Image> _images = new Dictionary<int, Image>();
        private Image _selImage = new Bitmap(Image.FromFile("tiles/selected.png"));
        private Image _tileImage = new Bitmap(Image.FromFile("tiles/tile.png"));
        private Image _disImage = new Bitmap(Image.FromFile("tiles/disabled.png"));
        private Image _hintImage = new Bitmap(Image.FromFile("tiles/hint.png"));
        private bool _drawGrid = false;
        private bool _showHint = false;
        private bool _showMoveable = false;
        private Tile _hint1, _hint2;

        public Field Field
        {
            get { return _field; }
            set { _field = value; Invalidate(); }
        }

        public PanelMode Mode
        {
            get { return _mode; }
            set { _mode = value; Invalidate(); }
        }

        public Tile Selected
        {
            get { return _selected; }
            set { _selected = value; Invalidate(); }
        }

        public bool DrawGrid
        {
            get { return _drawGrid; }
            set { _drawGrid = value; Invalidate(); }
        }

        public PanelView()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            
            // Add image for tiles without type
            _images.Add(-1, new Bitmap(_tileImage));
        }

        private RectangleF TileRectangle(Tile t)
        {
            return new RectangleF(t.X * CELLWIDTH, t.Y * CELLHEIGHT,
                Tile.WIDTH * CELLWIDTH, Tile.HEIGHT * CELLHEIGHT);
        }

        private void DrawTileGrid(Graphics g)
        {
            for (int x = 1; x <= Field.WIDTH-1; x++)
            {
                float px = x * CELLWIDTH;
                g.DrawLine(Pens.Black, px, 0, px, Field.HEIGHT * CELLHEIGHT);
            }
            for (int y = 1; y <= Field.HEIGHT-1; y++)
            {
                float py = y * CELLHEIGHT;
                g.DrawLine(Pens.Black, 0, py, Field.WIDTH * CELLWIDTH, py);
            }
        }

        private void DrawTile(Graphics g, Tile tile)
        {
            int id = tile.Type != null ? tile.Type.Id : -1;
            if (!_images.ContainsKey(id))
            {
                Bitmap copy;
                using (Image img = Image.FromFile("tiles/" + tile.Type.Name + ".png"))
                {
                    copy = new Bitmap(_tileImage);
                    Graphics.FromImage(copy).DrawImage(img, new Rectangle(0, 0, DRAWWIDTH, DRAWHEIGHT));
                    _images.Add(id, copy);
                }
            }

            Image texture = _images[id];
            RectangleF rect = TileRectangle(tile);
            rect.X -= 5;
            rect.Y -= 5 + 5 * tile.Z;
            rect.Width = DRAWWIDTH;
            rect.Height = DRAWHEIGHT;
            g.DrawImage(texture, rect);

            if (_showMoveable && !_field.CanMove(tile))
                g.DrawImage(_disImage, rect);

            if (_showHint && (tile == _hint1 || tile == _hint2))
                g.DrawImage(_hintImage, rect);

            if (tile == _selected)
                g.DrawImage(_selImage, rect);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_field == null)
                return;

            if (_drawGrid)
                DrawTileGrid(e.Graphics);

            if (_showHint)
                FindHints();

            // Draw sorted tile list from bottom left to top right
            Tile[] tiles = _field.GetSortedTiles();
            foreach (Tile t in tiles)
                DrawTile(e.Graphics, t);
        }

        private void ClickPlay(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _showMoveable = !_showMoveable;
                return;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _showHint = !_showHint;
                return;
            }

            int xp = (int)(e.X / CELLWIDTH);
            int yp = (int)(e.Y / CELLHEIGHT);

            Tile clicked = _field.GetTileFromCoord(xp, yp);
            if (clicked == null)
                return;

            if (!_field.CanMove(clicked))
                return;

            if (_selected == null || clicked == _selected)
                _selected = clicked;
            else
            {
                PlayResult result = _field.Play(_selected, clicked);
                if (result == PlayResult.Won)
                {
                    MessageBox.Show("You won the game in " + _field.GameTime.TotalSeconds + " seconds !");
                }
                else if ((result & PlayResult.NoFurtherMoves) != 0)
                {
                    DialogResult boxResult = MessageBox.Show("No further moves possible :( Scramble?", "Dead end",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (boxResult == DialogResult.Yes)
                        _field.Scramble();
                }
                _selected = null;
            }
        }

        private void ClickEdit(MouseEventArgs e)
        {
            int xp = (int)(e.X / CELLWIDTH);
            int yp = (int)(e.Y / CELLHEIGHT);

            if (e.Button == MouseButtons.Right)
            {
                Tile tile = _field.GetTileFromCoord(xp, yp);
                if (tile != null)
                    _field.Remove(tile);
            }
            else
            {
                int zp = _field.FindNewTileZ(xp, yp);
                Tile tile = new Tile(xp, yp, zp, null);
                _field.Add(tile);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_mode == PanelMode.Edit)
                ClickEdit(e);
            else
                ClickPlay(e);

            Invalidate();
        }

        private void FindHints()
        {
            TilePair hint = _field.GetHint();
            if (hint != null)
            {
                _hint1 = hint.Tile1;
                _hint2 = hint.Tile2;
            }
        }
    }
}
