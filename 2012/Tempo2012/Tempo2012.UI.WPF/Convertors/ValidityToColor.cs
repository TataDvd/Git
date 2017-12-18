using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Tempo2012.UI.WPF.Convertors
{
    public class ValidityToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var staff = (string)value;
            if (!string.IsNullOrWhiteSpace(staff))
            {
                return "White";
            }
            else
            {
                return "LightPink";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
