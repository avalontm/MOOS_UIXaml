using MOOS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class TextBox : Widget
    {
        public string Text { set;get; }
        public int FontSize { set; get; }
        public FontWeight FontWeight { get; set; }
        public HorizontalAlignment HorizontalContentAlignment { get; set; }
        public VerticalAlignment VerticalContentAlignment { get; set; }

        public TextBox()
        {
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
            FontWeight = new FontWeight();
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            Background = Brushes.White;
            Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;
        }

        void PS2Keyboard_OnKeyChanged(ConsoleKeyInfo key)
        {
            if (IsFocus)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (!string.IsNullOrEmpty(Text))
                        {
                            Text = Text.Remove(Text.Length);
                        }   
                        break;
                    case ConsoleKey.Enter:

                        break;
                    default:
                        Text += key.KeyChar;
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

        }

        int start = 0;

        public override void Draw()
        {
            base.Draw();

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, Background.Value);

            if (!string.IsNullOrEmpty(Text))
            {
                int w = 0, h = (Y + (Height / 2)) - (WindowManager.font.FontSize / 2);
              
                for (int i = start; i < Text.Length; i++)
                {
                    w += WindowManager.font.DrawChar(Framebuffer.Graphics, X + w, h, Text[i], Foreground.Value);

                    if (w + WindowManager.font.FontSize > Width)
                    {
                        start = 0;
                    }
                    else
                    {
                        start = 0;
                    }
                }

                if (BorderBrush != null)
                {
                    DrawBorder();
                }
            }
        }
    }
}
