using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;

namespace System.Windows
{
    public enum Cursor
    {
        Normal,
        Grab,
        Moving,
        TextSelect,
        Hand,
        Cross,
        Wait,
    }

    public class CursorManager
    {
        static Image CursorNormal { set; get; }
        static Image CursorMoving { set; get; }
        static Image CursorTextSelect { set; get; }
        static Image CursorHand{ set; get; }
        public static Cursor State { set; get; }

        public static void Initialize()
        {
            //Sized width to 512
            //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
            CursorNormal = new PNG(File.Instance.ReadAllBytes("Images/Cursor.png"));
            CursorMoving = new PNG(File.Instance.ReadAllBytes("Images/Grab.png"));
            CursorTextSelect = new PNG(File.Instance.ReadAllBytes("Images/CursorTextSelect.png"));
            CursorHand = new PNG(File.Instance.ReadAllBytes("Images/CursorHand.png"));
            State = Cursor.Normal; 
        }

        public static Image GetCursor
        {
            get
            {
                switch (State)
                {
                    case Cursor.Normal:
                        return CursorNormal;
                    case Cursor.Grab:
                        return CursorMoving;
                    case Cursor.TextSelect:
                        return CursorTextSelect;
                    case Cursor.Hand:
                        return CursorHand;
                    default:
                        return CursorNormal;
                }
            }
        }

        public static void Update()
        {
            if (WindowManager.HasWindowMoving)
            {
                State = Cursor.Grab;
                return;
            }
            
            if (WindowManager.FocusControl != null)
            {
                if (WindowManager.FocusControl.MouseEnter && WindowManager.FocusControl.MouseFocus)
                {
                    State = WindowManager.FocusControl.Cursor;
                }
                else
                {
                    State = Cursor.Normal;
                }
                return;
            }
            State = Cursor.Normal;
        }
    }
}
