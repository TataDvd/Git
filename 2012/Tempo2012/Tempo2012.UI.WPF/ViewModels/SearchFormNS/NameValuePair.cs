using System;
using System.ComponentModel;
using Tempo2012.EntityFramework.Interface;

namespace Tempo2012.UI.WPF.ViewModels.SearchFormNS
{
    [Serializable]
    public class NameValuePair : INameValuePair
    {
        //#region INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        //}
        //protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    var handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}
        //#endregion

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value;}
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value;}
        }
    }
}