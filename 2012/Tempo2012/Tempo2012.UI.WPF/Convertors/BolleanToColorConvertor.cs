using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.treeviewmodel;

namespace Tempo2012.UI.WPF.Convertors
{
    public class BooleanToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool? val = value as bool?;
                if (val != null)
                {

                    if (val.GetValueOrDefault())
                    {
                        return "Black";
                    }
                    
                }
               
            }
            return "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
