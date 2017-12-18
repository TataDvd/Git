using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Tempo2012.UI.WPF.Convertors
{
    class BoolToOpositeBoolConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            bool currval = (bool)value;
            if (currval) return false;
            return true;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            bool currval = (bool)value;
            if (currval) return false;
            return true;
        }
    }
}
