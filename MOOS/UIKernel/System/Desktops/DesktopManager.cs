using MOOS;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Desktops.Controls;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Desktops
{
    public static class DesktopManager
    {

        static DesktopBar bar { set; get; }
        static DesktopDocker docker { set; get; }
        static List<DesktopControl> items { set; get; }
        static ICommand itemDesktop { set; get; }
        public static Image Wallpaper { set; get; }

        public static void Initialize()
        {
            DesktopExtentions.Initialize();
            bar = new DesktopBar();
            docker = new DesktopDocker();
            items = new List<DesktopControl>();

            //Image from unsplash
            Wallpaper = new PNG(File.Instance.ReadAllBytes("Images/Wallpaper1.png"));

            Image wall = Wallpaper;
            Wallpaper = wall.ResizeImage(Framebuffer.Width, Framebuffer.Height);
            wall.Dispose();

            //Bar Elements
            DesktopBarItem item = new DesktopBarItem();
            item.Content = "Desktop";
            item.X = 0;
            item.Y = 0;
            item.Command = new Binding() { Source = itemDesktop = new ICommand(onItemDesktop) };
            items.Add(item);

            DesktopBarClock clock = new DesktopBarClock();
            clock.HorizontalAlignment = Windows.HorizontalAlignment.Right;
            clock.X = 5;
            clock.Y = 0;
            clock.Command = new Binding() { Source = itemDesktop = new ICommand(onItemClock) };
            items.Add(clock);
        }

        static void onItemDesktop(object obj)
        {
            Debug.WriteLine($"[Item] Desktop");
        }

        static void onItemClock(object obj)
        {
            Debug.WriteLine($"[Item] Clock");
        }

        public static void Update()
        {
            docker.Update();
            bar.Update();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
            }
        }

        public static void Draw()
        {
            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);

            docker.Draw();
            bar.Draw();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw();
            }
        }
    }
}
