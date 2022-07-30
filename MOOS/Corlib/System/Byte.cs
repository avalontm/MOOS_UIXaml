#if Kernel
using MOOS;
using MOOS.NET.IPv4;
#endif

namespace System
{
    public struct Byte
    {
        public static byte Parse(string s)
        {
            const string digits = "0123456789";
            var result = 0;

            int z = 0;
            bool neg = false;

            if (s.Length >= 1)
            {
                if (s[0] == '+')
                {
                    z = 1;
                }

                if (s[0] == '-')
                {
                    z = 1;
                    neg = true;
                }
            }

            for (int i = z; i < s.Length; i++)
            {
                var ind = digits.IndexOf(s[i]);
                if (ind == -1)
                {
                    Console.WriteLine("Malformated");
                    return 0;
                }
                result = (result * 10) + ind;
            }

            if (neg)
            {
                result *= -1;
            }

            return (byte)result;
        }

        public unsafe override string ToString()
        {
            return ((ulong)this).ToString();
        }

        public string ToString(string format)
        {
            return ((ulong)this).ToString(format);
        }

        public int CompareTo(byte b)
        {
            if (this != b)
            {
                return -1;
            }

            return 0;
        }
    }
}