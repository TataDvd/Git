using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public class SetFocusEventArg:EventArgs
    {
        public SetFocusEventArg(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; set; } 
    }
}
