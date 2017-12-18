using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Models
{
    public class DdsDnevnicItem : INotifyPropertyChanged
    {
        private DdsItemModel ddsItemModel;
        public DdsDnevnicItem(DdsItemModel ddsItem)
        {
            ddsItemModel = ddsItem;
        }

        public DdsDnevnicItem()
        {
            ddsItemModel = new DdsItemModel();
        }
        public decimal DdsSuma {
            get
            {
                return ddsItemModel.DdsSuma;
            }
            set
            {
                ddsItemModel.DdsSuma = value;
                OnPropertyChanged("DdsSuma");
                OnPropertyChanged("DdsTotal");
                OnPropertyChanged("Dds");
            }
        }
        private decimal _percent;
        public decimal DdsPercent
        {
            get { return ddsItemModel.DdsPercent; }
            set
            {
                ddsItemModel.DdsPercent = value;
                OnPropertyChanged("DdsPercent");
                OnPropertyChanged("DdsTotal");
                OnPropertyChanged("Dds");
            }
        }

        public bool In {
            get
            {
                return ddsItemModel.In;
            }
            set
            {
                ddsItemModel.In = value; OnPropertyChanged("In");
            }
        }
        public decimal Dds
        {
            get { return ddsItemModel.Dds; }
            set
            {
                ddsItemModel.Dds = value;
                OnPropertyChanged("In");
                OnPropertyChanged("Dds");
                OnPropertyChanged("DdsSuma");
                OnPropertyChanged("DdsTotal");
                OnPropertyChanged("DdsPercent");
            }
        }
        
        public decimal DdsTotal
        {
            get
            {
                return ddsItemModel.DdsTotal;
            }
            set
            {
                ddsItemModel.DdsTotal = value;
                OnPropertyChanged("DdsTotal");
                OnPropertyChanged("DdsSuma");
                OnPropertyChanged("Dds");

            }
        }
        private string _name;
        public string Name {
            get {
                return ddsItemModel.Name;
            } 
            set {
                ddsItemModel.Name = value;
                OnPropertyChanged("Name"); 
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
        
        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
     
}
