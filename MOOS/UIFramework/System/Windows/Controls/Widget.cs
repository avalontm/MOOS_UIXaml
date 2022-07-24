
using System.Diagnostics;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Position
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
    }

    public class Widget
    {
        Brush _background;
        public string Name { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public Thickness Margin { set; get; }
        public Thickness Padding { set; get; }
        public Brush Background 
        {
            set { _background = value;
            }
            get { return _background; }
        }
        public Brush Foreground { set; get; }
        public Brush BorderBrush { set; get; }
        public Thickness BorderThickness { set; get; }
        public Brush ColorFocus { set; get; }
        public Brush ColorNormal { set; get; }
        public int GridRow { get;  set; }
        public int GridColumn { get; set; }
        public FontFamily FontFamily { get; set; }

        Widget _parent;
        public Widget Parent
        {
            set
            {
                _parent = value;
                onSetParent(_parent);
            }
            get
            {
                return _parent;
            }
        }

        GridCollection _pos;
        public GridCollection Pos
        {
            set
            {
                _pos = value;
            }
            get
            {
                return _pos;
            }
        }

        public int GridColumnSpan { get; set; }
        public int GridRowSpan { get; set; }

        public Widget() : base()
        {
            Parent = this;
            Background = new Brush(0xFF222222);
            Foreground = new Brush(0xFFFFFF);
            BorderBrush = new Brush(0xFF333333);
            ColorNormal = new Brush(0xFF111111);
            ColorFocus = new Brush(0xFF141414);
            BorderThickness = new Thickness(1);
            Margin = new Thickness();
            Padding = new Thickness();
            FontFamily = new FontFamily();
        }

        public virtual void Draw()
        {
            if (this.Parent == null)
            {
                return;
            }

            // Position & margin
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
        }

        public virtual void Update() 
        {
        }

        public void onSetParent(Widget parent)
        {
            X = parent.X;
            Y = parent.Y;
            Width = parent.Width;
            Height = parent.Height;
        }

        public void onMouseFocus()
        {
           // Background.Value = ColorFocus.Value;
        }

        public void onMouseLostFocus()
        {
           // Background.Value = ColorNormal.Value;
        }
    }
}
