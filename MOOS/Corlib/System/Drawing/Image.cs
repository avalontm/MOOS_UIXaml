using Internal.Runtime.CompilerServices;
using MOOS.Driver;
using System.Diagnostics;
using System.Windows.Media;

namespace System.Drawing
{
    public class Image
    {
        public uint[] RawData;
        public int Bpp;
        public int Width;
        public int Height;

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            Bpp = 4;
            RawData = new uint[width * height];
        }

        public Image()
        {

        }

        public uint GetPixel(int X, int Y)
        {
            return RawData[Y * Width + X];
        }

        public Image ResizeImage(int NewWidth, int NewHeight)
        {
            if(NewWidth == 0 || NewHeight == 0) 
            {
                return new Image();
            }

            int w1 = Width, h1 = Height;
            uint[] temp = new uint[NewWidth * NewHeight];

            int x_ratio = ((w1 << 16) / NewWidth) + 1, y_ratio = ((h1 << 16) / NewHeight) + 1;
            int x2, y2;

            for (int i = 0; i < NewHeight; i++)
            {
                for (int j = 0; j < NewWidth; j++)
                {
                    x2 = ((j * x_ratio) >> 16);
                    y2 = ((i * y_ratio) >> 16);
                    temp[(uint)((i * NewWidth) + j)] = RawData[(uint)((y2 * w1) + x2)];
                }
            }

            Image image = new Image()
            {
                Width = NewWidth,
                Height = NewHeight,
                Bpp = Bpp,
                RawData = temp
            };

            return image;
        }

        public unsafe Image Bilinear(int newWidth, int newHeight)
        {
            int pixelSize = Bpp; // 4 pixels
            int srcStride = pixelSize * Width;
            int dstStride = pixelSize * newWidth;

            int dstOffset = dstStride - 3 * newWidth; // 3 bits
            int xFactor = (Width / newWidth);
            int yFactor = (Height / newHeight);
            uint* src;
            uint* dst;

            fixed (uint* dataPtr = RawData)
            {
                src = dataPtr;
            }

            uint[] tmp = new uint[newHeight * dstStride];

            fixed (uint* dataPtr = tmp)
            {
                dst = dataPtr;
            }
           
            double ox, oy, dx1, dy1, dx2, dy2;
            int ox1, oy1, ox2, oy2;

            int ymax = Height - 1;
            int xmax = Width - 1;

            uint* tp1, tp2;
            uint* p1, p2, p3, p4;

            for (int y = 0; y < newHeight; y++)
            {
                oy = y * yFactor;
                oy1 = (int)oy;
                oy2 = (oy1 == ymax) ? oy1 : oy1 + 1;
                dy1 = oy - oy1;
                dy2 = 1.0 - dy1;

                tp1 = src + oy1 * srcStride;
                tp2 = src + oy2 * srcStride;

                for (int x = 0; x < newHeight; x++)
                {
                    ox = x * xFactor;
                    ox1 = (int)ox;
                    ox2 = (ox1 == xmax) ? ox1 : ox1 + 1;
                    dx1 = ox - ox1;
                    dx2 = 1.0 - dx1;

                    p1 = tp1 + ox1 * pixelSize;
                    p2 = tp1 + ox2 * pixelSize;
                    p3 = tp2 + ox1 * pixelSize;
                    p4 = tp2 + ox2 * pixelSize;

                    for (int i = 0; i < pixelSize; i++, dst++, p1++, p2++, p3++, p4++)
                        *dst = (uint)(dy2 * (dx2 * (*p1) + dx1 * (*p2)) + dy1 * (dx2 * (*p3) + dx1 * (*p4)));
                }

                dst += dstOffset;
            }

            int index = 0;
            for (uint* counter = dst; *counter != 0; counter++)
            {
                tmp[index++] = *counter;
            }

            Image image = new Image()
            {
                Width = newWidth,
                Height = newHeight,
                Bpp = Bpp,
                RawData = tmp
            };

            return image;
        }

        public override void Dispose()
        {
            RawData.Dispose();
            base.Dispose();
        }
    }
}