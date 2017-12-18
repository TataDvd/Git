using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class CheckBoxEx : CheckBox
    {
        private string OldValue;
        protected override void OnChecked(System.Windows.RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (string.IsNullOrEmpty(OldValue))
            {
                OldValue = Content.ToString();
            }
            Content = OldValue + " Разрешено";

        }

        protected override void OnUnchecked(System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldValue))
            {
                OldValue = Content.ToString();
            }
            Content = OldValue + " Забранено";
        }

    }
}