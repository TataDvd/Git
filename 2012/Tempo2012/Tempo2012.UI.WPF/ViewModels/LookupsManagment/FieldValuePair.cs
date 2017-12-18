using System.Collections.ObjectModel;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class FieldValuePair:BaseViewModel
    {
        public string Name { get; set;}
        public string Value { get; set;}
        public string Type { get; set;}
        public string FieldName { get; set;}

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; OnPropertyChanged("ReadOnly"); }
        }

        public int Length { get; set;}
        public int MinWidth 
        {
            get
            {
                return Length * 7;
            }
        }
        private bool _isUnique;
        public bool IsUnique
        {
            get { return _isUnique; }
            set
            {
                _isUnique = value;
                OnPropertyChanged("IsUnique");
            }
        }

        private bool _isRequared;
        public bool IsRequared
        {
            get { return _isRequared; }
            set
            {
                _isRequared = value;
                OnPropertyChanged("IsRequared");
            }
        }
        public bool IsLookUp { get; set;}
        public string RFIELDNAME { get; set; }
        public string RTABLENAME { get; set; }
        public string Tn { get; set; }
        public string RFIELDKEY { get; set; }
        public int RCODELOOKUP { get; set;}
        public ObservableCollection<SaldoItem> LookUp { get; set;}
        private SaldoItem _selectedLookupItem;
        private bool _readOnly;

        public SaldoItem SelectedLookupItem
        {
            get { return _selectedLookupItem; }
            set {  _selectedLookupItem = value;
                OnPropertyChanged("SelectedLookupItem");
            }
        }
    }
}