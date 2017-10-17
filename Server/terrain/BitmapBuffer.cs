#region

using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace terrain
{
    internal unsafe class BitmapBuffer
    {
        private readonly Bitmap _bmp;
        private readonly int _h;
        private readonly int _w;
        private BitmapData _dat;
        private byte* _ptr;
        private int _s;

        public BitmapBuffer(Bitmap bmp)
        {
            this._bmp = bmp;
            _w = bmp.Width;
            _h = bmp.Height;
        }

        public uint this[int x, int y]
        {
            get { return *(uint*) (_ptr + x*4 + y*_s); }
            set { *(uint*) (_ptr + x*4 + y*_s) = value; }
        }

        public void Lock()
        {
            _dat = _bmp.LockBits(new Rectangle(0, 0, _w, _h), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            _s = _dat.Stride;
            _ptr = (byte*) _dat.Scan0;
        }

        public void Unlock()
        {
            _bmp.UnlockBits(_dat);
        }
    }
}