using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Views.Dialogs;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class ReorderViewModel:BaseViewModel
    {
        protected override void Add()
        {
            MonthSelector ms = new MonthSelector(ConfigTempoSinglenton.GetInstance().WorkDate.Month.ToString(), "Ибери номер месец за преномериране");
            ms.ShowDialog();
            if (ms.DialogResult.HasValue && ms.DialogResult.Value)
            {
                Context.Reorder(new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,ms.Month,1));
                
            }
            
        }
    }
}
