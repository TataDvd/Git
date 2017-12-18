namespace Tempo2012.UI.WPF.ViewModels.ContragenManager
{
    public class Filter:BaseViewModel
    {
        private string _filterWord;
        public string FilterWord
        {
            get { return _filterWord; }
            set { _filterWord = value; OnPropertyChanged("FilterWord"); }
        }

        private string _filterField;
        public string FilterField
        {
            get { return _filterField; }
            set { _filterField = value; OnPropertyChanged("FilterField"); }
        }
        private string _filterName;
        public string FilterName
        {
            get { return _filterName; }
            set { _filterName = value; OnPropertyChanged("FilterName"); }
        }
    }
}