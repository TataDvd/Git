using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Tempo2012.UI.WPF.Convertors
{
    internal class DateToStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DateTime.Now;

            DateTime dateTime;
            if (DateTime.TryParse(value.ToString(),out dateTime))
            {
                return dateTime;
            }
            return DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            string rez = (value is DateTime ? (DateTime) value : new DateTime()).ToShortDateString();
            return rez;
        }

        #endregion
    }
}
