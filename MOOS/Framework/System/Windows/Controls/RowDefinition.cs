using System;
using System.Collections.Generic;


namespace System.Windows.Controls
{
    public class RowDefinition 
    {
        public Position Position { set; get; }
        public int ActualHeight { get; }
        public int MaxHeight { get; set; }
        public int MinHeight { get; set; }
        public int Offset { get; }
        public GridLength Height { set; get; }

        public RowDefinition()
        {
            Position = new Position();
        }
    }
}
