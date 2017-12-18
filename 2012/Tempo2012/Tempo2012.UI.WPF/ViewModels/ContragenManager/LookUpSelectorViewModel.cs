using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels.ContragenManager
{
    public class LookUpSelectorViewModel:BaseViewModel
    {
        private SaldoItem WorkSaldoItem;
        public LookUpSelectorViewModel(SaldoItem si)
        {
            WorkSaldoItem = si;
            Fields = WorkSaldoItem.GetDictionary("","");
            Filters = new ObservableCollection<Filter>(WorkSaldoItem.GetFilters());
        }

        private ObservableCollection<string> _selectedItem;
        public ObservableCollection<string> SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public ObservableCollection<Filter> Filters { get; set;}

        public ObservableCollection<ObservableCollection<string>> Fields { get; set; }

        public void Refresh(Filter tag)
        {
            Fields = WorkSaldoItem.GetDictionary(string.Format("AND (\"{0}\" like '%{1}%' OR UPPER (\"{0}\") like '%{2}%')", tag.FilterField, tag.FilterWord, tag.FilterWord.ToUpper()), string.Format(" order by \"{0}\"", tag.FilterField));
            OnPropertyChanged("Fields");
        }
    }
}
