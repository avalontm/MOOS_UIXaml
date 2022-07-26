using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        [DllImport("SayHello")]
        public static extern void SayHello();

        //Check out
        //Kernel.API
        //Internal.Runtime.CompilerHelpers.InteropHelpers
        [DllImport("WriteLine")]
        public static extern void WriteLine();

        [DllImport("Allocate")]
        public static extern nint Allocate(ulong size);

        [DllImport("Free")]
        public static extern ulong Free(nint ptr);

        [DllImport("Reallocate")]
        public static extern nint Reallocate(nint intPtr, ulong size);

        [DllImport("GetTick")]
        public static extern uint GetTick();

        [DllImport("Sleep")]
        public static extern void Sleep(ulong ms);

        [DllImport("ReadAllBytes")]
        public static extern void ReadAllBytes(string name, out ulong size, out byte* data);

        [DllImport("Write")]
        public static extern void Write(char c);

        [DllImport("SwitchToConsoleMode")]
        public static extern void SwitchToConsoleMode();

        [DllImport("DrawPoint")]
        public static extern void DrawPoint(int x, int y, uint color);
        [DllImport("Debug.WriteLine")]
        public static extern void Debug_WriteLine(string value);


        [DllImport("Graphics.FillRectangle")]
        public static extern void Graphics_FillRectangle(int uid, int x, int y, int width, int height, uint color);
        [DllImport("UIFrameBuffer")]
        public static extern int UIFrameBuffer(int x, int y, int width, int height);
        [DllImport("UIFrameBuffer.Update")]
        public static extern void UIFrameBuffer_Update(int uid);

        [RuntimeExport("Main")]
        public static void Main()
        {
            onDraw();
        }

        static void onDraw()
        {
            int uid = UIFrameBuffer(100,100,300,300);

            for (; ; )
            {
                Graphics_FillRectangle(uid, 100, 100, 300, 300, 0xFF32A852);
                UIFrameBuffer_Update(uid);
            }
        }
    }
}