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

        public Image ConvertToGray()
        {
            int nOffset = 4 - Width * 3;
            uint[] temp = new uint[Width * Height];
            byte red, green, blue;

            unsafe
            {
                byte* p = (byte*)RawData.GetRawData();
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

                        p += 3;
                        temp[y * Width + x] = (uint)p;
                    }
                    p += nOffset;
                }
 
            }

            Image image = new Image()
            {
                Width = Width,
                Height = Height,
                Bpp = Bpp,
                RawData = temp
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