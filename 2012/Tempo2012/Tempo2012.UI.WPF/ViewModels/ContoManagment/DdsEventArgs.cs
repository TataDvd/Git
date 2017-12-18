using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public class DdsEventArgs:EventArgs
    {
        

        public DdsEventArgs(int kind)
        {
            Kind = kind;
        }
        public DdsEventArgs(int kind,bool issaved)
        {
            Kind = kind;
            IsSaved = issaved;
        }

        public DdsEventArgs(int kindActivity, bool p, string selestitem)
        {
            Kind = kindActivity;
            IsSaved = p;
            Selestitem = selestitem;
        }

        public DdsEventArgs(int kindActivity, bool p, string selestitem,string codeDoc)
        {
            Kind = kindActivity;
            IsSaved = p;
            Selestitem = selestitem;
            CodeDoc = codeDoc;
        }
        public int Kind { get; set;}

        public bool IsSaved { get; set; }

        public string Selestitem { get; set; }

        public string CodeDoc { get; set; }
    }

    public class DdsCancelEventArgs : EventArgs
    {


        public DdsCancelEventArgs(int kind)
        {
            Kind = kind;
        }
        public int Kind { get; set; }

       
    }
}
