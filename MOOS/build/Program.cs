using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Drawing;
using System.Runtime;
using System.Windows.Forms;
using System.Windows;

static unsafe class Program
{
    static void Main() { }
    static Image Cursor;
    static Image CursorMoving;
    public static Image Wallpaper;

    static bool USBMouseTest()
    {
        HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out var Buttons);
        return Buttons != MouseButtons.None;
    }

    static bool USBKeyboardTest()
    {
        HID.GetKeyboardThings(HID.Keyboard, out var ScanCode, out var Key);
        return ScanCode != 0;
    }

    [RuntimeExport("KMain")]
    static void KMain()
    {
        Hub.Initialize();
        HID.Initialize();
        EHCI.Initialize();

        if (HID.Mouse != null)
        {
            Console.Write("[Warning] Press please press Mouse any key to validate USB Mouse ");
            bool res = Console.Wait(&USBMouseTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Mouse = null;
                }
            }
        }

        if (HID.Keyboard != null)
        {
            Console.Write("[Warning] Press please press any key to validate USB keyboard ");
            bool res = Console.Wait(&USBKeyboardTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Keyboard = null;
                }
            }
        }

        USB.StartPolling();

        //Use qemu for USB debug
        //VMware won't connect virtual USB HIDs
        if (HID.Mouse == null)
        {
            Console.WriteLine("USB Mouse not present");
        }
        if (HID.Keyboard == null)
        {
            Console.WriteLine("USB Keyboard not present");
        }

        //Sized width to 512
        //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
        Cursor = new PNG(File.Instance.ReadAllBytes("Images/Cursor.png"));
        CursorMoving = new PNG(File.Instance.ReadAllBytes("Images/Grab.png"));
        //Image from unsplash
        Wallpaper = new PNG(File.Instance.ReadAllBytes("Images/Wallpaper1.png"));

        BitFont.Initialize();

        string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.Instance.ReadAllBytes("Song.btf"), 16));

        WindowManager.Initialize();
        MessageBox.Initialize();
      
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        Audio.Initialize();
        AC97.Initialize();

        SMain();
        }

    public static void SMain()
    {
        Console.WriteLine("Press any key to enter desktop...");

        Framebuffer.TripleBuffered = true;

        Image wall = Wallpaper;
        Wallpaper = wall.ResizeImage(Framebuffer.Width, Framebuffer.Height);
        wall.Dispose();
        
        UIMoos xamlWindow = new UIMoos();
        xamlWindow.ShowDialog();

        for (; ; )
        {
            WindowManager.InputAll();

            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);

            WindowManager.DrawAll();
            WindowManager.UpdateAll();

            Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, WindowManager.HasWindowMoving ? CursorMoving : Cursor);
            Framebuffer.Update();
           
            FPSMeter.Update();
        }

    }

}
