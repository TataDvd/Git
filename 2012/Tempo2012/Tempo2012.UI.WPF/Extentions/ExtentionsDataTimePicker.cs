using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tempo2012.UI.WPF.Extentions
{
    public static class DatePickerExtensions
    {
        public static void FocusOnText(this DatePicker datePicker)
        {
            Keyboard.Focus(datePicker);

            var eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                             Keyboard.PrimaryDevice.ActiveSource,
                                             0,
                                             Key.Up);
            eventArgs.RoutedEvent = DatePicker.KeyDownEvent;

            datePicker.RaiseEvent(eventArgs);
        }
       
    }
}
