using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tempo2012.UI.WPF.Models
{
    public class ConfigParamModel : INotifyPropertyChanged
    {
        private string name;
        public virtual string Name {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        private string val;
        public virtual string Value
        { get { return val; } set { val = value; OnPropertyChanged("Value"); } }
        private string description;
        public virtual string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }
        public virtual string Changer { get; set; }

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
