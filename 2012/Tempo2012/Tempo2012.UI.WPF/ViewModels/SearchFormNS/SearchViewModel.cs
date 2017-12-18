using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using ReportBuilder;

namespace Tempo2012.UI.WPF.ViewModels.SearchFormNS
{
    public class SearchViewModel:BaseViewModel
    {
        private ISerchable serchable;
        public SearchViewModel(ISerchable serc)
        {
            serchable = serc;
            Items=new DataTable();
            foreach (var c in serchable.Columns())
            {
                Items.Columns.Add(c);
            }
            Context.ExecuteQuery(serchable.GetQuery(), Items);
            Colection= new ObservableCollection<string>(serchable.Columns());
            
        }

        public DataTable Items { get; set;}
        public ObservableCollection<string> Colection { get; set;}
    }
}
