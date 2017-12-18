using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class ContrSelectorViewModel:BaseViewModel
    {
        public ContrSelectorViewModel() 
        {
            ItemsDebit =new ObservableCollection<SaldoItem>(LoadCreditAnaliticAtributes());
        }

        

        public ObservableCollection<SaldoItem> ItemsDebit { get; set; }

       
        public IEnumerable<SaldoItem> LoadCreditAnaliticAtributes()
        {
            List<SaldoItem> saldoItems = new List<SaldoItem>();
            SaldoItem saldoItem = new SaldoItem();
            //saldoItem.Name = NameLookUp;
            //saldoItem.Relookup = LookUpCode;
            saldoItem.IsLookUp = true;
            saldoItems.Add(saldoItem);
            return saldoItems;
        }

        public int FirmaId { get; set; }
        public int LookupCode {
            get
            {
                return ItemsDebit[0].Relookup;
            }
            set
            {
                ItemsDebit[0].Relookup = value;
            }
        }
       
        public string NameLookup {
            get
            {
                return ItemsDebit[0].Name;
            }
            set
            {
                ItemsDebit[0].Name = value;
            }
        }
    }
}
