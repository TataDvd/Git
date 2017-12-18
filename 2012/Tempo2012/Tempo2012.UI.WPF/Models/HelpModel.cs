using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class HelpModel : INotifyPropertyChanged
    {
        private string _TopicId;
        public string TopicId
        {
            get { return _TopicId; }
            set
            {
                _TopicId = value;
                OnPropertyChanged("TopicId");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
