using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace CoffeeLibrary
{
  
    public class NomenclatureWizardModel : INotifyPropertyChanged
    {
        public NomenclatureWizardModel(List<LookUpMetaData> nomeclatureMetaDatas)
        {
            _allselectedItems = new List<LookUpMetaData>(nomeclatureMetaDatas);
            _selectedItems=new List<LookUpMetaData>(_allselectedItems.Count);
        }
        public LookUpMetaData Workitem = new LookUpMetaData { Description = "", Name = "Номенклатура1", Id = 1 };
        public string Name
        {
            get { return Workitem.Name; }
            set
            {
                Workitem.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return Workitem.Description; }
            set
            {
                Workitem.Description = value;
                OnPropertyChanged("Description");
            }
        }
        
        private List<LookUpMetaData> _selectedItems;
        public List<LookUpMetaData> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; OnPropertyChanged("SelectedItems"); }
        }
        private List<LookUpMetaData> _allselectedItems;
        public List<LookUpMetaData> AllSelectedItems
        {
            get { return _allselectedItems; }
            set { _allselectedItems = value; OnPropertyChanged("AllSelectedItems"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
     
}
