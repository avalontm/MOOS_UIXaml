using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Windows.Media
{
    public static class ColorConverter
    {
        public static uint ConvertFromString(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return 0;
            }

            int i = hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X') ? 2 : 0;
            uint value = 0;

            if (hex[0] == '#')
            {
                hex = hex.Remove(0);
            }

            while (i < hex.Length)
            {
                uint x = hex[i++];

                if (x >= '0' && x <= '9') x = x - '0';
                else if (x >= 'A' && x <= 'F') x = (x - 'A') + 10;
                else if (x >= 'a' && x <= 'f') x = (x - 'a') + 10;
                else return 0;

                value = 16 * value + x;
            }

            return value;
        }
    }
}
