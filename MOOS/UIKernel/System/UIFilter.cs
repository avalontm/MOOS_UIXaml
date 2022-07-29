using MOOS;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace System
{
    public static class UIFilter
    {
        public static unsafe void Bilinear()
        {
            int newWidth = Framebuffer.Width;
            int newHeight = Framebuffer.Height;
            int pixelSize = 4; // 4 pixels
            int srcStride = pixelSize * Framebuffer.Width;
            int dstStride = pixelSize * newWidth;

            int dstOffset = dstStride - 3 * newWidth; // 3 bits
            int xFactor = (Framebuffer.Width / newWidth);
            int yFactor = (Framebuffer.Height / newHeight);
            uint* src;
            uint* dst;

            Image screen = Framebuffer.Graphics.Save();
            fixed (uint* dataPtr = screen.RawData)
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

            int ymax = Framebuffer.Height - 1;
            int xmax = Framebuffer.Width - 1;

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

            Image filter = new Image()
            {
                Width = newWidth,
                Height = newHeight,
                Bpp = 4,
                RawData = tmp
            };
            screen.Dispose();

            Framebuffer.Graphics.DrawImage(0, 0, filter, false);
            filter.Dispose();
        }
    }
}
