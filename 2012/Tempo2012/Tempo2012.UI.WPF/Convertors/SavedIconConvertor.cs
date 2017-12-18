using System;
using System.Windows.Data;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Convertors
{
    public class SavedIconConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var val= value is int ? (int) value : -1;
                if (val != -1)
                {
                    if (val == 1)
                    {
                        return "../../Images/notes.png";
                    }
                }

            }
            return "../../Images/validation_correct.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}