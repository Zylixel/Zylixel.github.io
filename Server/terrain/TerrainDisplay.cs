#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace terrain
{
    internal class TerrainDisplay : Form
    {
        private readonly PictureBox _pic;
        private readonly PictureBox _pic2;
        private readonly TerrainTile[,] _tilesBak;
        private Bitmap _bmp;
        private Mode _mode;
        private Panel _panel;
        private TerrainTile[,] _tiles;

        public TerrainDisplay(TerrainTile[,] tiles)
        {
            _mode = Mode.Erase;
            _tilesBak = (TerrainTile[,]) tiles.Clone();
            this._tiles = tiles;
            ClientSize = new Size(800, 800);
            BackColor = Color.Black;
            WindowState = FormWindowState.Maximized;
            _panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Controls =
                {
                    (_pic = new PictureBox
                    {
                        Image = _bmp = RenderColorBmp(tiles),
                        SizeMode = PictureBoxSizeMode.AutoSize
                    })
                }
            };
            _panel.HorizontalScroll.Enabled = true;
            _panel.VerticalScroll.Enabled = true;
            _panel.HorizontalScroll.Visible = true;
            _panel.VerticalScroll.Visible = true;
            Controls.Add(_panel);
            _pic2 = new PictureBox
            {
                Image = _bmp,
                Width = 250,
                Height = 250,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            Controls.Add(_pic2);
            _pic2.BringToFront();

            Text = _mode.ToString();

            _pic.MouseMove += pic_MouseMove;
            _pic.MouseDoubleClick += pic_MouseDoubleClick;
        }

        private void pic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _tiles[e.X, e.Y].Region = TileRegion.Spawn;
            _bmp.SetPixel(e.X, e.Y, Color.FromArgb((int) GetColor(_tiles[e.X, e.Y])));
            _pic.Invalidate();
            _pic2.Invalidate();
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mode == Mode.Erase && (MouseButtons & MouseButtons.Left) != 0)
            {
                Point center = e.Location;
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                    {
                        if (x*x + y*y <= 10*10)
                        {
                            _tiles[center.X + x, center.Y + y].Terrain = TerrainType.None;
                            _tiles[center.X + x, center.Y + y].Elevation = 0;
                            _tiles[center.X + x, center.Y + y].Biome = "ocean";
                            _tiles[center.X + x, center.Y + y].TileObj = "";
                            _tiles[center.X + x, center.Y + y].TileId = TileTypes.DeepWater;
                            _tiles[center.X + x, center.Y + y].Region = TileRegion.None;
                            _tiles[center.X + x, center.Y + y].Name = "";
                            _bmp.SetPixel(center.X + x, center.Y + y, Color.FromArgb(
                                (int) GetColor(_tiles[center.X + x, center.Y + y])));
                        }
                    }
                _pic.Invalidate();
                _pic2.Invalidate();
            }
            else if (_mode == Mode.Average && (MouseButtons & MouseButtons.Left) != 0)
            {
                Point center = e.Location;
                Dictionary<TerrainTile, int> dict = new Dictionary<TerrainTile, int>();
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                        if (x*x + y*y <= 10*10)
                        {
                            TerrainTile t = _tiles[center.X + x, center.Y + y];
                            if (dict.ContainsKey(t))
                                dict[t]++;
                            else
                                dict[t] = 0;
                        }
                int maxOccurance = dict.Values.Max();
                TerrainTile targetTile = dict.First(t => t.Value == maxOccurance).Key;
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                        if (x*x + y*y <= 10*10)
                        {
                            _tiles[center.X + x, center.Y + y] = targetTile;
                            _bmp.SetPixel(center.X + x, center.Y + y, Color.FromArgb(
                                (int) GetColor(_tiles[center.X + x, center.Y + y])));
                        }
                _pic.Invalidate();
                _pic2.Invalidate();
            }
        }

        private static uint GetColor(TerrainTile tile)
        {
            if (tile.Region == TileRegion.Spawn)
                return 0xffff0000;
            return TileTypes.Color[tile.TileId];
        }

        private static Bitmap RenderColorBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    buff[x, y] = GetColor(tiles[x, y]);
                }
            buff.Unlock();
            return bmp;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) _mode = _mode + 1;
            if (_mode == Mode.Max) _mode = 0;
            Text = _mode.ToString();

            if (e.KeyCode == Keys.S)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "WMap files (*.wmap)|*.wmap|All Files (*.*)|*.*";
                if (sfd.ShowDialog() != DialogResult.Cancel)
                    WorldMapExporter.Export(_tiles, sfd.FileName);
            }
            else if (e.KeyCode == Keys.R)
            {
                _tiles = (TerrainTile[,]) _tilesBak.Clone();
                _bmp = RenderColorBmp(_tiles);
                _pic.Image = _pic2.Image = _bmp;
            }
            base.OnKeyUp(e);
        }

        private enum Mode
        {
            Erase,
            Average,
            Max
        }
    }
}