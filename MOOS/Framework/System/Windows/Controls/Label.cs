using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Label : Widget
    {
        public string Content { set; get; }
        public int FontSize { set; get; }
        public FontWeight FontWeight { get; set; }

        public Label()
        {
            FontWeight = new FontWeight();
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();

            if (!string.IsNullOrEmpty(Content))
            {
                WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Content)) / 2), (Y + (Height / 2)) - (WindowManager.font.FontSize / 2), Content, Foreground.Value);
            }
        }
    }
}
