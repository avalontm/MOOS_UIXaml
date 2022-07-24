#if HasGUI
using MOOS;
using System;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Button : Widget
    {
        public string Content { set; get; }
        public Binding Command { set; get; }
        public static object CommandProperty { get;  set; }

        bool clicked;

        public Button()
        {
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
            Background = new Brush(0xFF111111);
        }

        public override void Update()
        {
            base.Update();

            if (!WindowManager.HasWindowMoving && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height)
            {
                this.onMouseFocus();
                
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (Command != null && Command.Source != null)
                    {
                        if (!clicked)
                        {
                            clicked = true;
                            Command.Source.Execute.Invoke();
                        }

                    }
                }

            }
            else
            {
                this.onMouseLostFocus();
            }

            if (Control.MouseButtons == MouseButtons.None)
            {
                clicked = false;
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (this.Parent == null)
            {
                return;
            }

            //Position & margin
            if (Pos == null)
            {
                X = this.Parent.X + this.Margin.Left;
                Y = this.Parent.Y + this.Margin.Top;
                Width = this.Parent.Width - (this.Margin.Right * 2);
                Height = this.Parent.Height - (this.Margin.Bottom * 2);
            }
            else
            {
                X = this.Pos.Position.X + this.Margin.Left;
                Y = this.Pos.Position.Y + this.Margin.Top;
                Width = this.Pos.Position.Width - (this.Margin.Right * 2);
                Height = this.Pos.Position.Height - (this.Margin.Bottom * 2);
            }

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, Background.Value);

            if (Content != null)
            {
                WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Content)) / 2),(Y + (Height / 2) ) - (WindowManager.font.FontSize/2) , Content, Foreground.Value);
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }
        }

        public void DrawBorder()
        {
            Framebuffer.Graphics.DrawRectangle(X - (int)BorderThickness.Left, Y - (int)BorderThickness.Top, Width + (int)(BorderThickness.Right*2), Height + (int)(BorderThickness.Bottom*2), BorderBrush.Value);
        }

        public void SetBinding(object commandProperty, Binding binding)
        {
            Command = binding;
        }
    }
}
#endif