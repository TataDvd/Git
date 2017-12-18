using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.TemplateSelector
{
     class MainAccTemplateSelecor : DataTemplateSelector
    {
        public DataTemplate Main{ get; set; }
        public DataTemplate Sub { get; set; }
        public override DataTemplate SelectTemplate(object item,
         DependencyObject container)
        {
            if (item != null)
            {
                var acc = item as AccountsDialogViewModel;
                if (acc != null)
                {
                    if (acc.IsMain) return Main;
                }
            }
            return Sub;
        }
    }
}
