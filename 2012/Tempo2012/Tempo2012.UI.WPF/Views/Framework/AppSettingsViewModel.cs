using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{
    public class AppSettingsViewModel:BaseViewModel
    {
        public AppSettingsViewModel()
        {
            var conf = ConfigTempoSinglenton.GetInstance().FirmSettings;
            Settings=new ObservableCollection<FirmSettingViewModel>();
            foreach (FirmSettingModel firmSettingModel in conf.Where(e=>e.FirmaId==ConfigTempoSinglenton.GetInstance().CurrentFirma.Id && e.HoldingId==ConfigTempoSinglenton.GetInstance().ActiveHolding))
            {
                Settings.Add(new FirmSettingViewModel(firmSettingModel));
            }
        }

        protected override void Update()
        {
            var conf = ConfigTempoSinglenton.GetInstance();
            conf.SaveConfiguration();
            App.LoadConfig();
        }

        public ObservableCollection<FirmSettingViewModel> Settings { get; set;}
    }
}
