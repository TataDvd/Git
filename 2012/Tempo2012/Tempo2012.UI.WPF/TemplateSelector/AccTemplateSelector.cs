using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.treeviewmodel;

namespace Tempo2012.UI.WPF.TemplateSelector
{
    
    public class AccTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MainAccTemplate { get; set; }
        public DataTemplate SubAccTemplate { get; set; }
        public DataTemplate LookUpTemplate { get; set; }
        public DataTemplate LookUpElementTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
          DependencyObject container)
        {
             if (item != null)
             {
                 var treeViewModel = item as TreeViewModel;
                 if (treeViewModel != null)
                {
                    AccountsModel typeacc = treeViewModel.CurrAcc;
                    if (typeacc.Num==0)
                    {
                        return LookUpTemplate;
                    }
                    if (typeacc.SubNum>0)
                    {
                        return SubAccTemplate;
                    }
                }
             }

            return MainAccTemplate;
        }
    }
}
