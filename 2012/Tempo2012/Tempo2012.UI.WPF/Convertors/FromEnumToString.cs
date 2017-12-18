using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Tempo2012.UI.WPF.Convertors
{
    class FromEnumToString : IValueConverter
    {
            #region IValueConverter Members
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string strValue = value.ToString();
                string rezim="";
                switch (strValue)
                {
                    case "Add":{rezim="Режим : Добавяне"; break;}
                    case "Edit": { rezim = "Режим : Редактиране"; break; }
                    case "View": { rezim = "Режим : Разглеждане"; break; }
                }
                return rezim;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return null;
            }
            #endregion
        }
    
}
