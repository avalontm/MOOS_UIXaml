﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Windows.Controls
{
    public class GridCollection
    { 
        public Position Position { set; get; }
        public int Row { set; get; }
        public int Column { set; get; }

        public GridCollection(int row, int column)
        {
            Position = new Position();
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"X: {Position.X}, Y: {Position.Y}, Width: {Position.Width}, Height: {Position.Height}";
        }
    }

    public class Grid : Widget
    {
        public RowDefinitionCollection RowDefinitions { set; get; }
        public ColumnDefinitionCollection ColumnDefinitions { set; get; }
        public UIElementCollection Children { set; get; }
        List<GridCollection> Grids { set; get; }
        int _rows = 1;
        int _columns = 1;

        public Grid() : base()
        {
            Children = new UIElementCollection();
            RowDefinitions = new RowDefinitionCollection();
            ColumnDefinitions = new ColumnDefinitionCollection();
            Grids = new List<GridCollection>();
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

            if (RowDefinitions.Count == 0)
            {
                RowDefinitions.Add(new RowDefinition());
            }

            _rows = RowDefinitions.Count;

            if (ColumnDefinitions.Count == 0)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            _columns = ColumnDefinitions.Count;


            if (Grids.Count == 0)
            {
                for (int c = 0; c < _columns; c++)
                {
                    for (int r = 0; r < _rows; r++)
                    {
                        Grids.Add(new GridCollection(r, c));
                    }
                }
            }

            onUpdateGrids();
            onDrawGrids();
        }

        int rowPixels = 0;
        int colPixels = 0;
        int rowTotalStar = 0;
        int colTotalStar = 0;

        void onUpdateGrids()
        {
            rowPixels = 0;
            colPixels = 0;
            rowTotalStar = 0;
            colTotalStar = 0;

            for (int c = 0; c < _columns; c++)
            {
                if (!ColumnDefinitions[c].Width.IsStar)
                {
                    colPixels += ColumnDefinitions[c].Width.Value;
                }
                else
                {
                    colTotalStar++;
                }
            }

            for (int r = 0; r < _rows; r++)
            {
                if (!RowDefinitions[r].Height.IsStar)
                {
                    rowPixels += RowDefinitions[r].Height.Value;
                }
                else
                {
                    rowTotalStar++;
                }
            }

            for (int c = 0; c < _columns; c++)
            {
                for (int r = 0; r < _rows; r++)
                {
                    RowDefinitions[r].Position.X = 0;
                    RowDefinitions[r].Position.Y = 0;
                    ColumnDefinitions[c].Position.X = 0;
                    ColumnDefinitions[c].Position.Y = 0;

                    GridHight(r, c);
                    GridWidth(r, c);
                    GridPosX(r, c);
                    GridPosY(r, c);
                }
            }
        }

        void onDrawGrids()
        {
            for (int g = 0; g < Grids.Count; g++)
            {
                Grids[g].Position.X = ColumnDefinitions[Grids[g].Column].Position.X;
                Grids[g].Position.Y = RowDefinitions[Grids[g].Row].Position.Y;
                Grids[g].Position.Width = ColumnDefinitions[Grids[g].Column].Position.Width;
                Grids[g].Position.Height = RowDefinitions[Grids[g].Row].Position.Height;

                for (int c = 0; c < Children.Count; c++)
                {
                    if (Grids[g].Row == Children[c].GridRow && Grids[g].Column == Children[c].GridColumn)
                    {
                        Children[c].Parent = this;
                        Children[c].Pos = Grids[g];
                        Children[c].Draw();
                    }
                }
            }
        }

        void GridPosX(int r, int c)
        {
            if (c == 0)
            {
                ColumnDefinitions[c].Position.X = this.Parent.X + ColumnDefinitions[c].Position.X;
            }
            else
            {
                ColumnDefinitions[c].Position.X = ColumnDefinitions[c-1].Position.X + ColumnDefinitions[c-1].Width.Value;
            }
        }

        void GridPosY(int r, int c)
        {
            if (r == 0)
            {
                RowDefinitions[r].Position.Y = this.Parent.Y + RowDefinitions[r].Position.Y;
            }
            else
            {
                RowDefinitions[r].Position.Y  = RowDefinitions[r-1].Position.Y + RowDefinitions[r-1].Height.Value;
            }
        }

        void GridHight(int r, int c)
        {
            if (RowDefinitions[r].Height.IsAuto)
            {
                RowDefinitions[r].Position.Height = RowDefinitions[r].Height.Value;
            }
            else if (RowDefinitions[r].Height.IsStar)
            {
                RowDefinitions[r].Height.Value = ((this.Parent.Height - rowPixels) / rowTotalStar);
                RowDefinitions[r].Position.Height = RowDefinitions[r].Height.Value;
            }
            else if (RowDefinitions[r].Height.IsAbsolute)
            {
                RowDefinitions[r].Position.Height = RowDefinitions[r].Height.Value;
            }
        }

        void GridWidth(int r, int c)
        {
            if (ColumnDefinitions[c].Width.IsAuto)
            {
                ColumnDefinitions[c].Position.Width = ColumnDefinitions[c].Width.Value;
            }
            else if (ColumnDefinitions[c].Width.IsStar)
            {
                ColumnDefinitions[c].Width.Value = ((this.Parent.Width - colPixels) / colTotalStar);
                ColumnDefinitions[c].Position.Width = ColumnDefinitions[c].Width.Value;
            }
            else if (ColumnDefinitions[c].Width.IsAbsolute)
            {
                ColumnDefinitions[c].Position.Width = ColumnDefinitions[c].Width.Value;
            }
        }

        public static void SetRow(Widget control, int value)
        {
            control.GridRow = value;
        }

        public static void SetColumn(Widget control, int value)
        {
            control.GridColumn = value;
        }
    }
}
