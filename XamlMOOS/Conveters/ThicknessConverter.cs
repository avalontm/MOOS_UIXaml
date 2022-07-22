using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XamlMOOS.Conveters
{
    public class ThicknessConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Thickness thickness = new Thickness();

            if (source == null)
            {
                return thickness;
            }

            thickness = new Thickness(int.Parse(source.ToString()));

            return thickness;
        }
    }
}
