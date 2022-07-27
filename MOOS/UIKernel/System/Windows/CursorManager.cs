using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;

namespace System.Windows
{
    public enum CursorState
    {
        Normal = 0,
        Grab =1,
        Moving=2,
        TextSelect=3,
    }

    public class CursorManager
    {
        static Image CursorNormal { set; get; }
        static Image CursorMoving { set; get; }
        static Image CursorTextSelect { set; get; }
        public static CursorState State { set; get; }

        public static void Initialize()
        {
            //Sized width to 512
            //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
            CursorNormal = new PNG(File.Instance.ReadAllBytes("Images/Cursor.png"));
            CursorMoving = new PNG(File.Instance.ReadAllBytes("Images/Grab.png"));
            CursorTextSelect = new PNG(File.Instance.ReadAllBytes("Images/CursorTextSelect.png"));

            State = CursorState.Normal; 
        }

        public static Image GetCursor
        {
            get
            {
                switch (State)
                {
                    case CursorState.Normal:
                        return CursorNormal;
                    case CursorState.Grab:
                        return CursorMoving;
                    case CursorState.TextSelect:
                        return CursorTextSelect;
                    default:
                        return CursorNormal;
                }
            }
        }

        public static void Update()
        {
            if (WindowManager.HasWindowMoving)
            {
                State = CursorState.Grab;
                return;
            }

            if (WindowManager.FocusControl != null)
            {
                if (WindowManager.FocusControl.MouseEnter)
                {
                    State = WindowManager.FocusControl.Cursor;
                }
                else
                {
                    State = CursorState.Normal;
                }
                return;
            }

            State = CursorState.Normal;
        }
    }
}
