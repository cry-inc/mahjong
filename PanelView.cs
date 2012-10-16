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

        private Field _field;
        private PanelMode _mode = PanelMode.Play;
        private Tile _selected;
        private Dictionary<int, Image> _images = new Dictionary<int, Image>();
        private Image _selImage = Image.FromFile("Tiles/selected.png");

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

        public PanelView()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        public float GetCellWidth()
        { return 30; /* (float)Width / Field.WIDTH; */ }

        public float GetCellHeight()
        { return 20; /* (float)Height / Field.HEIGHT; */ }

        private RectangleF TileRectangle(float cellWidth, float cellHeight, Tile t)
        {
            return new RectangleF(t.X * cellWidth, t.Y * cellHeight,
                Tile.WIDTH * cellWidth, Tile.HEIGHT * cellHeight);
        }

        private void DrawGrid(Graphics g, float cellWidth, float cellHeight)
        {
            for (int x = 1; x <= Field.WIDTH-1; x++)
            {
                float px = x * cellWidth;
                g.DrawLine(Pens.Black, px, 0, px, Height);
            }
            for (int y = 1; y <= Field.HEIGHT-1; y++)
            {
                float py = y * cellHeight;
                g.DrawLine(Pens.Black, 0, py, Width, py);
            }
        }

        private void DrawTile(Graphics g, float cellWidth, float cellHeight, Tile tile)
        {
            if (!_images.ContainsKey(tile.Type.Id))
            {
                Bitmap copy;
                using (Image img = Image.FromFile("Tiles/" + tile.Type.Name + ".png"))
                {
                    copy = new Bitmap(DRAWWIDTH, DRAWHEIGHT);
                    Graphics.FromImage(copy).DrawImage(img, new Rectangle(0, 0, DRAWWIDTH, DRAWHEIGHT));
                    _images.Add(tile.Type.Id, copy);
                }
            }
            Image texture = _images[tile.Type.Id];
            
            RectangleF rect = TileRectangle(cellWidth, cellHeight, tile);
            RectangleF drawRect = rect;
            drawRect.X -= 5;
            drawRect.Y -= 5 + 5 * tile.Z;
            drawRect.Width = DRAWWIDTH;
            drawRect.Height = DRAWHEIGHT;
            
            g.DrawImage(texture, drawRect);

            if (tile == _selected)
            {
                g.DrawImage(_selImage, drawRect);
                //Color highlighted = Color.FromArgb(125, 255, 0, 255);
                //g.FillRectangle(new SolidBrush(highlighted), drawRect);
            }
        }

        private static int CompareTilesByZ(Tile tile1, Tile tile2)
        {
            if (tile1.Z == tile2.Z)
            {
                if (tile1.X == tile2.X)
                {
                    return tile1.Y - tile2.Y;
                }
                else return tile1.X - tile2.X;
            }
            else return tile1.Z - tile2.Z;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_field == null)
                return;

            float cellWidth = GetCellWidth();
            float cellHeight = GetCellHeight();

            DrawGrid(e.Graphics, cellWidth, cellHeight);

            // Copy tiles into list and sort
            List<Tile> tiles = new List<Tile>();
            foreach (KeyValuePair<int, Tile> pair in _field.Tiles)
                tiles.Add(pair.Value);
            tiles.Sort(CompareTilesByZ);

            // Draw sorted tile list from bottom left to top right
            foreach (Tile t in tiles)
                DrawTile(e.Graphics, cellWidth, cellHeight, t);
        }

        private void ClickPlay(MouseEventArgs e)
        {
            float cellWidth = GetCellWidth();
            float cellHeight = GetCellHeight();

            int xp = (int)(e.X / cellWidth);
            int yp = (int)(e.Y / cellHeight);

            Tile clicked = _field.GetTileFromCoord(xp, yp);
            if (clicked == null)
                return;

            if (_selected == null || clicked == _selected)
                _selected = clicked;
            else
            {
                if (_selected.Type.Id == clicked.Type.Id && _field.CanMove(_selected) && _field.CanMove(clicked))
                {
                    _field.Remove(clicked);
                    _field.Remove(_selected);

                    // TODO: Check for no solution
                }
                _selected = null;
            }
        }

        private void ClickEdit(MouseEventArgs e)
        {
            float cellWidth = GetCellWidth();
            float cellHeight = GetCellHeight();

            int xp = (int)(e.X / cellWidth);
            int yp = (int)(e.Y / cellHeight);

            if (e.Button == MouseButtons.Middle)
            {
                Tile tile = _field.GetTileFromCoord(xp, yp);
                if (tile != null)
                    _field.Remove(tile);
            }
            else
            {
                int zp = _field.FindNewTileZ(xp, yp);
                Tile tile = new Tile(xp, yp, zp, _field.TileTypes[0]);
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
    }
}
