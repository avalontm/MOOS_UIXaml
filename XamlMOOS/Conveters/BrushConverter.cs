using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Resources.ResXFileRef;

namespace XamlMOOS.Conveters
{
    public class BrushConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Brush brush = null;

            if (source == null)
            {
                return brush;
            }

            var converter = new System.Windows.Media.BrushConverter();
            brush = (Brush)converter.ConvertFromString(source.ToString());

            return brush;
        }
    }
}
