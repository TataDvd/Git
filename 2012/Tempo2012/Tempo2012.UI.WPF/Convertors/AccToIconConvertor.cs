using System;
using System.Windows.Data;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Convertors
{
    public class AccToIconConvertor : IValueConverter
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
                        return "../../Images/Apps-knotes-icon.png";
                    }
                    if (typeacc.SubNum > 0)
                    {
                        return "../../Images/Applications-iconsilver.png";
                    }
                    if (typeacc.Num == -1)
                    {
                        return "../../Images/Apps-knotes-iconsmall.png";
                    }
                    if (typeacc.Num == -2)
                    {
                        return "";
                    }
                }

            }
            return "../../Images/Applications-icon.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}