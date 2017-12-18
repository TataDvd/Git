using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.TemplateSelector
{
    public class ItemLookupTemplateSelecor : DataTemplateSelector 
    {
        public DataTemplate Template { get; set; }
        public DataTemplate LookUpTemplate { get; set;}
        public DataTemplate DateTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item,
         DependencyObject container)
        {
            if (item != null)
            {
                var saldoItem = item as FieldValuePair;
                if (saldoItem != null)
                {
                    if (saldoItem.IsLookUp) return LookUpTemplate;
                }
                if (saldoItem != null && saldoItem.Type != null && saldoItem.Type.ToUpper()=="DATE")
                {
                    return DateTemplate;
                }
            }
            return Template;
        }
    }
}
