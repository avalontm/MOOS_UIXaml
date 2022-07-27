using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Windows
{
    public class CursorConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Cursor cursor = Cursor.Normal;

            switch (source.ToString().ToLower())
            {
                case "hand":
                    cursor = Cursor.Hand;
                    break;
                default:
                    cursor = Cursor.Normal;
                    break;
            }

            return cursor;
        }
    }
}
