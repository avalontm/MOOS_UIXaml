
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

        RowDefinition _row;
        public RowDefinition Row
        {
            set
            {
                _row = value;
            }
            get
            {
                return _row;
            }
        }

        public Widget() 
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
        }

        public virtual void Draw()
        {

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
