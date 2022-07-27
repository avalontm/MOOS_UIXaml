namespace System
{
    public static class Math
    {
        public static double E = 2.7182818284590451;
        public static double PI = 3.1415926535897931;
        public static double Tau = 6.2831853071795862;

        public static int Abs(int value) 
        {
            return value < 0 ? value * -1 : value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double Pow(double a, double b)
        {
            double c = 1;
            for (int i = 0; i < b; i++)
                c *= a;
            return c;
        }

        public static double Fact(double x)
        {
            double ret = 1;
            for (int i = 1; i <= x; i++)
                ret *= i;
            return ret;
        }

        public static double Sin(double x)
        {
            double y = x;
            double s = -1;
            for (int i = 3; i <= 100; i += 2)
            {
                y += s * (Pow(x, i) / Fact(i));
                s *= -1;
            }
            return y;
        }

        public static double Cos(double x)
        {
            double y = 1;
            double s = -1;
            for (int i = 2; i <= 100; i += 2)
            {
                y += s * (Pow(x, i) / Fact(i));
                s *= -1;
            }
            return y;
        }
        public static double Tan(double x)
        {
            return (Sin(x) / Cos(x));
        }

        public static double Sqrt(double _d)
        {
            double w = _d, h = 1, t = 0;
            if (w < 1)
            {
                h = _d;
                w = 1;
            }
            do
            {
                w *= 0.5;
                h += h;
            } while (w > h);
            for (int i = 0; i < 4; i++)
            {
                t = ((w + h) * 0.5);
                h = ((h / t) * w);
                w = t;
            }
            return (((w + h) * 0.5));
        }

        public static double Floor(double num)
        {
            return (int)num;
        }

        public static double Round(double d, int decimals)
        {
            double multiplier = Pow(10, decimals);

            if (d < 0)
                multiplier *= -1;

            return Floor((d * multiplier) + 0.5) / multiplier;

        }
    }
}