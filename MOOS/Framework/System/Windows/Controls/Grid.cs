using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Windows.Controls
{
    public class Grid : Widget
    {
        public RowDefinitionCollection RowDefinitions { set; get; }
        public UIElementCollection Children { set; get; }

        public Grid() : base()
        {
            Children = new UIElementCollection();
            RowDefinitions = new RowDefinitionCollection();
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update();
            }
        }

        public override void Draw()
        {
            base.Draw();

            for (int r = 0; r < RowDefinitions.Count; r++)
            {
                RowDefinition row = RowDefinitions[r];

                if (r == 0)
                {
                    row.Position.X = this.Parent.X;
                    row.Position.Y = this.Parent.Y;
                    row.Position.Width = this.Parent.Width;

                    if (row.Height.IsAuto)
                    {
                        row.Position.Height = row.Height.Value;
                    }
                    else if (row.Height.IsStar)
                    {
                        row.Position.Height = this.Parent.Height;
                    }
                    else if (row.Height.IsAbsolute)
                    {
                        row.Position.Height = row.Height.Value;
                    }
                }
                else
                {
                    RowDefinition _prev = RowDefinitions[r-1];

                    row.Position.X = _prev.Position.X;

                    row.Position.Y = _prev.Position.Y + _prev.Position.Height;

                    row.Position.Width = _prev.Position.Width;

                    if (row.Height.IsAuto)
                    {
                        row.Position = GetPixel(r);
                    }
                    else if (row.Height.IsStar)
                    {
                        row.Position = GetStar(r);
                    }   
                    else if (row.Height.IsAbsolute)
                    {
                        row.Position = GetPixel(r);
                    }

                }
            }

            for (int c = 0; c < Children.Count; c++)
            {
                Children[c].Parent = this;
                Children[c].Row = RowDefinitions[Children[c].GridRow];
                Children[c].Draw();
            }
        }

        Position GetPixel(int index)
        {
            Position position = new Position();

            position.X = this.Parent.X;

            if (index == 0)
            {
                position.Y = this.Parent.Y;
            }
            else
            {
                position.Y = RowDefinitions[index - 1].Position.Y + RowDefinitions[index - 1].Position.Height;
            }

            position.Width = this.Parent.Width;

            position.Height = RowDefinitions[index].Height.Value;

            return position;
        }

        Position GetStar(int index)
        {
            Position position = new Position();
            int height = 0;

            position.X = this.Parent.X;

            if (index == 0)
            {
                position.Y = this.Parent.Y;
            }
            else
            {
                position.Y = RowDefinitions[index - 1].Position.Y + RowDefinitions[index-1].Position.Height;
            }
          
            position.Width = this.Parent.Width;

            for (int r = 0; r < RowDefinitions.Count; r++)
            {
                if (index != r)
                {
                    if (!RowDefinitions[r].Height.IsStar)
                    {
                        height += RowDefinitions[r].Height.Value;
                    }
                }
            }

            int stars = 0;

            for (int r = 0; r < RowDefinitions.Count; r++)
            {
                if (RowDefinitions[r].Height.IsStar)
                {
                    stars++;
                }
            }

            if (stars == 0)
            {
                stars = 1;
            }

            int _height = (this.Parent.Height - height);

            if (_height > 0)
            {
                position.Height = (_height / stars);
            }

            return position;
        }

        public static void SetRow(Widget control, int value)
        {
            control.GridRow = value;
        }
    }
}
