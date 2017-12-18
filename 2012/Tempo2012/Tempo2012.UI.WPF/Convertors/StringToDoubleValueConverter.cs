using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.Convertors
{
    public class StringToDoubleValueConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double num;
            string strvalue = value.ToString();
            if (double.TryParse(strvalue, out num))
            {
                return num;
            }
            return DependencyProperty.UnsetValue;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return String.Format(Vf.LevFormatUI,value);
        }
    }
}