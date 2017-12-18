using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class SearchFormViewModel<T>:BaseViewModel
    {
        public SearchFormViewModel(T t,IEnumerable<T> list)
        {
            CurrentItem = t;
            SearchItems = new ObservableCollection<T>(list);
        }
        private string _SearchString;
        public string SearchString {
            get
            {
                return _SearchString;
            }
            set
            {
                _SearchString=value;
                OnPropertyChanged("SearchString");
            }
        }
        private T _CurrentItem;
        public T CurrentItem 
        {
            get 
            {
                return _CurrentItem;
            }
            set
            {
                _CurrentItem = value;
                OnPropertyChanged("CurrentItem");
            }
        }

        public ObservableCollection<T> SearchItems { get; set;}
        protected override void Search()
        {
            base.Search();
        }
    }
}
