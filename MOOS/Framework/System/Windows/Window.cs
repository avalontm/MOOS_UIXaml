﻿using MOOS;
using MOOS.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Forms;

namespace System.Windows
{

    public enum WindowStartupLocation
    {
        Manual = 0,
        CenterScreen = 1,
        CenterOwner = 2
    }

    public class Window : Widget
    {
        public string Title { set; get; }
        public WindowStartupLocation WindowStartupLocation { get; set; }

        Widget _content;
        public Widget Content
        {
            set
            {
                _content = value;
                _content.Parent = this;
            }
            get
            {
                return _content;
            }
        }

        public bool Visible
        {
            set
            {
                _visible = value;
                OnSetVisible(value);
            }
            get
            {
                return _visible;
            }
        }

        public Window() : base()
        {
            this.Visible = false;
            X= 0;
            Y= 0;
            Width = 300;
            Height = 150;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            WindowManager.Childrens.Add(this);
        }

        public void ShowDialog()
        {
            onWindowStartupLocation();
            WindowManager.MoveToEnd(this);
            this.Visible = true;
        }

        public void Close()
        {
            this.Visible = false;
        }

        void onWindowStartupLocation()
        {
            switch (this.WindowStartupLocation)
            {
                case WindowStartupLocation.Manual:
                    break;
                case WindowStartupLocation.CenterOwner:

                    break;
                case WindowStartupLocation.CenterScreen:
                    X = (Framebuffer.Width / 2) - (this.Width / 2);
                    Y = (Framebuffer.Height / 2) - (this.Height / 2);
                    break;
            }

        }

        public bool IsUnderMouse()
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
        }

        public bool _visible;

        public virtual void OnSetVisible(bool value) { }

        public int BarHeight = 40;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index { get => WindowManager.Childrens.IndexOf(this); }

        public virtual void OnInput()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (
                    !WindowManager.HasWindowMoving &&
                    Control.MousePosition.X > CloseButtonX && Control.MousePosition.X < CloseButtonX + WindowManager.CloseButton.Width &&
                    Control.MousePosition.Y > CloseButtonY && Control.MousePosition.Y < CloseButtonY + WindowManager.CloseButton.Height
                )
                {
                    this.Visible = false;
                    return;
                }
                if (!WindowManager.HasWindowMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    WindowManager.MoveToEnd(this);
                    Move = true;
                    WindowManager.HasWindowMoving = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
                WindowManager.HasWindowMoving = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }
        }

        private int CloseButtonX => X + Width + 2 - (BarHeight / 2) - (WindowManager.CloseButton.Width / 2);
        private int CloseButtonY => Y - BarHeight + (BarHeight / 2) - (WindowManager.CloseButton.Height / 2);

        public override void Update()
        {
            base.Update();
            if (Content != null)
            {
                Content.Update();
            }
        }

        public override void Draw()
        {
            base.Draw();

            //WindowBar
            Framebuffer.Graphics.FillRectangle(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title, Foreground.Value);

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, Background.Value);

            if (Content != null)
            {
                Content.Draw();
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }

            Framebuffer.Graphics.DrawImage(CloseButtonX, CloseButtonY, WindowManager.CloseButton);
        }

        public void DrawBorder(bool HasBar = true)
        {
            Framebuffer.Graphics.DrawRectangle(X - (int)BorderThickness.Left, Y - (HasBar ? BarHeight : 0) - (int)BorderThickness.Top, Width + (int)(BorderThickness.Right*2), (HasBar ? BarHeight : 0) + Height + (int)(BorderThickness.Bottom*2), BorderBrush.Value);
        }
    }
}