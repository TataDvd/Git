using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class FieldValuePair
    {
        public string Name { get; set;}
        public string Value { get; set;}
        public string Type { get; set;}
        public bool ReadOnly { get; set;}
        public int Length { get; set;}
        public int MinWidth 
        {
            get
            {
                return Length * 7;
            }
        }
    }
    public class LookupsEdidViewModels:BaseViewModel
    {
        public LookupsEdidViewModels(IEnumerable<FieldValuePair> fields)
        {
            _Fields = new ObservableCollection<FieldValuePair>(fields);
        }
        public LookupsEdidViewModels()
        {
            _Fields = new ObservableCollection<FieldValuePair>();
            _Fields.Add(new FieldValuePair { Name = "Test", Type = "DB", Value = "1" });
            _Fields.Add(new FieldValuePair { Name = "Test1", Type = "DB1", Value = "Ибре" });
            _Fields.Add(new FieldValuePair { Name = "Test2", Type = "DB2", Value = "Оро" });
        }
        private ObservableCollection<FieldValuePair> _Fields { get; set; }
        public ObservableCollection<FieldValuePair> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
                OnPropertyChanged("Fields");
            }
        }
        
    }
}
