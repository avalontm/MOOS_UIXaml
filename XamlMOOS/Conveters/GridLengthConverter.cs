using System;
using System.Collections.Generic;
using System.Globalization;
//------------------------------------------------------------------------------
// Basic Code By AvalonTM
//------------------------------------------------------------------------------

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XamlMOOS.Conveters
{
    public class GridLengthConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            GridLength gridLength;

            if (source == null)
            {
                return gridLength;
            }

            switch (source.ToString().ToLower())
            {
                case "auto":
                    gridLength = new GridLength(1, GridUnitType.Auto);
                    break;
                case "*":
                    gridLength = new GridLength(1, GridUnitType.Star);
                    break;
                default:
                    gridLength = new GridLength(int.Parse(source.ToString()));
                    break;
            }

            return gridLength;
        }
    }
}
