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
        public int MaxLength { set; get; }
        public FontWeight FontWeight  {set; get; }
        public TextWrapping TextWrapping  {set; get; }
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
            TextWrapping = TextWrapping.NoWrap;
            Background = Brushes.White;
            Keyboard.OnKeyChanged += Keyboard_OnKeyChanged;
        }

        void Keyboard_OnKeyChanged(ConsoleKeyInfo key)
        {
            if (IsFocus)
            {
                if (key.KeyState == System.ConsoleKeyState.Pressed)
                {
                    if (MaxLength > 0)
                    {
                        if (Text.Length > MaxLength)
                        {
                            return;
                        }
                    }

                    switch (key.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (Text.Length > 0)
                            {
                                Text.Length -= 1;
                            }
                            break;
                        case ConsoleKey.Enter:

                            break;
                        default:
                            if (key.KeyChar != '\0')
                            {
                                Text += key.KeyChar.ToString();
                            }
                            break;
                    }
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }

        int start = 0;
        DateTime Flicker = DateTime.Now;

        public override void Draw()
        {
            base.Draw();

            int pos = 1;
            int w = 0, h = (Y + (Height / 2)) - (WindowManager.font.FontSize / 2);
            int fnt = (WindowManager.font.FontSize / 2);
            int _w = (pos * fnt);

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, Background.Value);

            if (!string.IsNullOrEmpty(Text))
            {
                w = WindowManager.font.MeasureString(Text);
              
                if (w > Width - fnt)
                {
                    start = (Text.Length) - ((Width / fnt) - 2);
                }
                else
                {
                    start = 0;
                }

                for (int i = start; i < Text.Length; i++)
                {
                    _w = (pos * fnt);

                    if (_w < (Width - fnt))
                    {
                        WindowManager.font.DrawChar(Framebuffer.Graphics, X + _w, h, Text[i], Foreground.Value);
                    }
                  pos++;
                }
            }

            _w += (2 + fnt);

            if (IsFocus)
            {
                if (DateTime.Now.Ticks > Flicker.Ticks )
                {
                    Flicker = DateTime.Now.AddTicks(TimeSpan.FromMilliseconds(50).Ticks);
                    Framebuffer.Graphics.DrawLine((X + _w), Y + 5, (X + _w), (Y + Height) - 5, 0xFF000000);
                }
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }
            ;
        }

       
    }
}
