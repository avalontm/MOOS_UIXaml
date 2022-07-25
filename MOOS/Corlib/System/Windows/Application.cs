using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Windows
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UIApplication
    {
        public UIntPtr hwnd;
        public int x;
        public int y;
        public int width;
        public int height;

    }
}
