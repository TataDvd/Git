using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Convertors
{
    public class AccToVisibleConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var treeViewModel = value as AccountsModel;
                if (treeViewModel != null)
                {
                    if (treeViewModel.Num >= 0)
                    {
                        return Visibility.Visible;
                    }
                    
                }

            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
