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
    public class AccToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var treeViewModel = value as AccountsModel;
                if (treeViewModel != null)
                {
                    AccountsModel typeacc = treeViewModel;
                    if (typeacc.Num == 0)
                    {
                        return "Red";
                    }
                    if (typeacc.SubNum > 0)
                    {
                        return "Green";
                    }
                    if (typeacc.Num ==-1)
                    {
                        return "Pink";
                    }
                }
               
            }
            return "Yellow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
