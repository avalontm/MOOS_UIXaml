using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text
{
    public class Encoding
    {
        public static Encoding ASCII
        {
            get { return new Encoding(); }
        }

        public byte[] GetBytes(string s)
        {
            byte[] buffer = new byte[s.Length];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)s[i];
            return buffer;
        }

    }
}
