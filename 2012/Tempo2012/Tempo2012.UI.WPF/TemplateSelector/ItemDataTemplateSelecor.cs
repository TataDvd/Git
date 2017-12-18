using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.TemplateSelector
{
    public class ItemDataTemplateSelecor : DataTemplateSelector 
    {
        public DataTemplate IntTemplate { get; set; }
        public DataTemplate DateTemplate { get; set; }
        public DataTemplate LookUpTemplate { get; set;}
        public DataTemplate MoneyTemplate { get; set;}
        public DataTemplate StringTemplate { get; set;}
        public DataTemplate DKTemplate { get; set;}
        public DataTemplate DTemplate { get; set; }
        public DataTemplate KTemplate { get; set; }
        public DataTemplate ValTemplate { get; set; }
        public DataTemplate KolTemplate { get; set; }
        public DataTemplate KursTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item,
         DependencyObject container)
        {
            if (item != null)
            {
                var saldoItem = item as SaldoItem;
                if (saldoItem != null)
                {
                    if (saldoItem.IsKol) return KolTemplate;
                    if (saldoItem.IsVal) return ValTemplate;
                    if (saldoItem.IsKurs) return KursTemplate;
                    if (saldoItem.IsDK)
                    {
                        if (saldoItem.IsK)
                        {
                            return KTemplate;
                        }
                        if (saldoItem.IsD)
                        {
                            return DTemplate;
                        }
                        return DKTemplate;
                    }
                    if (saldoItem.IsLookUp) return LookUpTemplate;
                   
                    switch (saldoItem.Type)
                    {
                        case SaldoItemTypes.Currency:return MoneyTemplate;
                        case SaldoItemTypes.Date: return DateTemplate;
                        case SaldoItemTypes.String: return MoneyTemplate;
                        case SaldoItemTypes.Integer:return IntTemplate;
                                
                    }

                }
            }

            return StringTemplate;
        }
    }
}
