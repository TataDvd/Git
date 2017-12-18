using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Convertors
{
    public class BoolToVisibleConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var treeViewModel = (bool)value;
                if(treeViewModel)
                {
                   return Visibility.Visible;
                }

            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var treeViewModel =(Visibility)value;
                if (treeViewModel==Visibility.Visible)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
