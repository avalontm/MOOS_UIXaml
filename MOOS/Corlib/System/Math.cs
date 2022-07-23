namespace System
{
    public static class Math
    {
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

        public static double Pow(double number, double raiseToPower)
        {
            double result = 0;
            if (raiseToPower < 0)
            {
                raiseToPower *= -1;
                result = 1 / number;
                for (int i = 1; i < raiseToPower; i++)
                {
                    result /= number;
                }
            }
            else
            {
                result = number;
                for (int i = 0; i <= raiseToPower; i++)
                {
                    result *= number;
                }
            }
            return result;
        }

    }
}