using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class ReorderViewModel:BaseViewModel
    {
        protected override void Add()
        {
            Context.Reorder();
        }
    }
}
