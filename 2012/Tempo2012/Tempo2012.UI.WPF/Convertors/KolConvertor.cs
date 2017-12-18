using System;
using System.Threading;
using System.Windows.Data;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.Convertors
{
    public class KolConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (string.IsNullOrWhiteSpace(value.ToString())) return string.Format(Vf.KolFormat, 0);
            decimal val;
            var ns = System.Globalization.NumberStyles.AllowDecimalPoint |
                     System.Globalization.NumberStyles.AllowLeadingSign;
            if (decimal.TryParse(value.ToString(), ns, Thread.CurrentThread.CurrentCulture, out val))
            {
                return string.Format(Vf.KolFormat, val);
            }
            return string.Format(Vf.KolFormat, 0);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal val;
            var ns = System.Globalization.NumberStyles.AllowDecimalPoint |
                     System.Globalization.NumberStyles.AllowLeadingSign;
            if (decimal.TryParse(value.ToString(), ns, Thread.CurrentThread.CurrentCulture, out val))
            {
                return val;
            }
            return 0m;
        }
    }
}