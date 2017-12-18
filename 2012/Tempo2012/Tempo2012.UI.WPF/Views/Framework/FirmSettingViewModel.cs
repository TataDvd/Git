using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{
    public class FirmSettingViewModel:BaseViewModel
    {
        public FirmSettingViewModel(FirmSettingModel model)
        {
            _fm = model;
        }
        private FirmSettingModel _fm;

        public string Key
        {
            get { return _fm.Name; }
            set {
                _fm.Name = value;
                OnPropertyChanged("Key");
            }
        }
        public string Value
        {
            get { return _fm.Value;}
            set {
                _fm.Value = value; 
                OnPropertyChanged("Value");
            }
        }
    }
}