using Internal.Runtime.CompilerServices;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;
using System;
using System.Diagnostics;
using System.Windows;
using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

namespace MOOS
{
    public static unsafe class API
    {
        public static unsafe void* HandleSystemCall(string name)
        {
            switch (name)
            {
                case "SayHello":
                    return (delegate*<void>)&SayHello;
                case "WriteLine":
                    return (delegate*<void>)&API_WriteLine;
                case "Allocate":
                    return (delegate*<ulong, nint>)&API_Allocate;
                case "Reallocate":
                    return (delegate*<nint, ulong, nint>)&API_Reallocate;
                case "Free":
                    return (delegate*<nint, ulong>)&API_Free;
                case "Sleep":
                    return (delegate*<ulong, void>)&API_Sleep;
                case "GetTick":
                    return (delegate*<ulong>)&API_GetTick;
                case "ReadAllBytes":
                    return (delegate*<string, ulong*, byte**, void>)&API_ReadAllBytes;
                case "Write":
                    return (delegate*<char, void>)&API_Write;
                case "SwitchToConsoleMode":
                    return (delegate*<void>)&API_SwitchToConsoleMode;
                case "DrawPoint":
                    return (delegate*<int, int, uint, void>)&API_DrawPoint;
                case "Debug.WriteLine":
                    return (delegate*<string, void>)&API_Debug_WriteLine;
                case "Graphics.DrawChar":
                    return (delegate*<int, int, char, uint, int>)&API_Graphics_DrawChar;
                case "Graphics.FillRectangle":
                    return (delegate*<int, int, int, int, int, uint, void>)&API_Graphics_FillRectangle;
                case "Graphics.DrawString":
                    return (delegate*<int, int, string, uint, int, int, void>)&API_Graphics_DrawString;
                case "UIFrameBuffer":
                    return (delegate*<int, int, int, int, int>)&API_UIFrameBuffer;
                case "UIFrameBuffer.Update":
                    return (delegate*<int, void>)&API_UIFrameBuffer_Update;
                    

            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        public static void API_DrawPoint(int x, int y, uint color)
        {
            if (!Framebuffer.TripleBuffered)
            {
                Framebuffer.Graphics.DrawPoint(x, y, color);
            }
        }

        public static void API_SwitchToConsoleMode()
        {
            Framebuffer.TripleBuffered = false;
        }

        public static void API_ReadAllBytes(string name, ulong* length, byte** data)
        {
            byte[] buffer = File.Instance.ReadAllBytes(name);

            *data = (byte*)Allocator.Allocate((ulong)buffer.Length);
            *length = (ulong)buffer.Length;
            fixed (byte* p = buffer) Native.Movsb(*data, p, *length);

            buffer.Dispose();
        }

        public static void API_Sleep(ulong ms)
        {
            Thread.Sleep(ms);
        }

        public static ulong API_GetTick()
        {
            return Timer.Ticks;
        }

        public static void API_Write(char c)
        {
            Console.Write(c);
        }

        public static void API_WriteLine()
        {
            Console.WriteLine();
        }

        public static nint API_Allocate(ulong size)
        {
            //Debug.WriteLine($"API_Allocate {size}");
            return Allocator.Allocate(size);
        }

        public static ulong API_Free(nint ptr)
        {
            //Debug.WriteLine($"API_Free 0x{((ulong)ptr).ToString("x2")}");
            return Allocator.Free(ptr);
        }

        public static nint API_Reallocate(nint intPtr, ulong size)
        {
            return Allocator.Reallocate(intPtr, size);
        }

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }

        public static void API_Debug_WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        public static void API_Graphics_FillRectangle(int uid, int x, int y, int width, int height, uint color)
        {
            Framebuffer.Graphics.FillRectangle(x, y, width, height, color);
        }

        public static int API_Graphics_DrawChar(int x, int y, char chr, uint color)
        {
            return WindowManager.font.DrawChar(Framebuffer.Graphics, x, y, chr, color);
        }

        public static void API_Graphics_DrawString(int x, int y, string str, uint color, int lineLimit, int heightLimit)
        {
            WindowManager.font.DrawString(x, y, str, color, lineLimit, heightLimit);
        }

        public static int API_UIFrameBuffer(int x, int y, int width, int height)
        {
            return new UIFramebuffer(x, y, width, height).UID;
        }

        public static void API_UIFrameBuffer_Update(int uid)
        {
            if (Framebuffer.TripleBuffered)
            {
                for (int i = 0; i < Framebuffer.Width * Framebuffer.Height; i++)
                {
                    if (i >= (UIFramebuffer.Frames[uid].X * UIFramebuffer.Frames[uid].Y) && i <= ((UIFramebuffer.Frames[uid].X + UIFramebuffer.Frames[uid].Width) * (UIFramebuffer.Frames[uid].Y + UIFramebuffer.Frames[uid].Height)))
                    {
                        if (Framebuffer.FirstBuffer[i] != UIFramebuffer.Frames[uid].FirstBuffer[i])
                        {
                            UIFramebuffer.Frames[uid].FirstBuffer[i] = Framebuffer.FirstBuffer[i];
                        }
                    }
                }
            }
        }
    }
}