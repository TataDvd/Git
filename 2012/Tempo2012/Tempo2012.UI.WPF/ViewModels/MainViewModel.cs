using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Models;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private Users currentuser;
        public MainViewModel()
        {
           ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
           ConfigParams = new ObservableCollection<ConfigParamModel>();
           ConfigParams.Add(new ConfigParamModel {Changer="firm",Description=currentconfig.CurrentFirma.Bulstad,Name="Фирма",Value=currentconfig.CurrentFirma.Name});
           ConfigParams.Add(new ConfigParamModel {Changer="data",Description="Днес "+DateTime.Now.ToShortDateString(),Name="Работна дата",Value=currentconfig.WorkDate.ToShortDateString()});
        }

        public MainViewModel(EntityFramework.Models.Users currentuser)
            :this()
        {
            // TODO: Complete member initialization
            this.currentuser = currentuser;
            ConfigParams.Add(new ConfigParamModel {Changer="user",Description=currentuser.Name,Name="Потребител",Value=currentuser.Username+" "+currentuser.Rights});
        }
        public ObservableCollection<ConfigParamModel> ConfigParams { get; set; }
        public void ChangeConfigValue(int index,string value,string name)
        {
            ConfigParams[index].Value = value;
            ConfigParams[index].Description = name;
                  
        }
    }
}
