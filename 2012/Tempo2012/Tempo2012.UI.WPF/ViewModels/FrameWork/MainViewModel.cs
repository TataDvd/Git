using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Models;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using Tempo2012.UI.WPF.Dialogs;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private User currentuser;
        private string _firmaname;
        private string _workdate;
        public string FirmaName
        {
            get
            {
                return _firmaname;
            }
            set
            {
                _firmaname = value;
                OnPropertyChanged("FirmaName");
            }

        }

        private string _dn;
        public string Dn
        {
            get { return _dn; }
            set 
            {
                _dn = value;
                OnPropertyChanged("Dn"); 
            }
        }

        private string _bulstad; 
        public string Bulstad
        {
            get { return _bulstad; }
            set
            {
                _bulstad = value;
                OnPropertyChanged("Bulstad");
            }
        }
        private string _regdds;
        private string _holding;

        public string RegDds
        {
            get { return _regdds; }
            set
            {
                _regdds = value;
                OnPropertyChanged("RegDds");
            }
        }
        public string WorkDate
        {
            get { return _workdate; } 
            set 
            {
                _workdate = value;
                OnPropertyChanged("WorkDate");
            }
        }

        public string User
        {
            get { if (currentuser != null) return currentuser.Name;
                return "";
            }
            set
            {
                currentuser.Name = value;
                OnPropertyChanged("User");
            }
        }
        public MainViewModel()
        {
           ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
           ConfigParams = new ObservableCollection<ConfigParamModel>();
           _firmaname = currentconfig.CurrentFirma.Name;
           _dn=currentconfig.CurrentFirma.DDSnum;
           _bulstad = currentconfig.CurrentFirma.Bulstad;
           _regdds = currentconfig.CurrentFirma.RegisterDds ? "Регистрирана по ДДС" : "Нерегистрирана по ДДС";
           ConfigParams.Add(new ConfigParamModel {Changer="firm",Description=currentconfig.CurrentFirma.Bulstad,Name="Фирма",Value=currentconfig.CurrentFirma.Name});
           _workdate = currentconfig.WorkDate.ToLongDateString();
            if (currentconfig.Holdings != null)
                if (currentconfig.Holdings.Count > currentconfig.ActiveHolding-1)
                    _holding = currentconfig.Holdings[currentconfig.ActiveHolding-1].Name;
            ConfigParams.Add(new ConfigParamModel {Changer="data",Description="Днес "+DateTime.Now.ToShortDateString(),Name="Работна дата",Value=currentconfig.WorkDate.ToShortDateString()});
            if (currentconfig.Periods.FirstOrDefault(e1 => e1.Fr <= currentconfig.WorkDate && e1.To >= currentconfig.WorkDate && e1.Firma == currentconfig.ActiveFirma && e1.Holding == currentconfig.ActiveHolding) != null)
            {
                
                    DataSelector ds = new DataSelector(currentconfig.WorkDate, "Избери работна дата");
                    ds.ShowDialog();
                    if (ds.DialogResult.HasValue && ds.DialogResult.Value)
                    {

                        currentconfig.WorkDate = ds.SelectedDate;
                        WorkDate = ds.SelectedDate.ToLongDateString();
                        currentconfig.SaveConfiguration();
                        //confi.DataContext = vm.ConfigParams;
                    }
                
            }
        }

        public MainViewModel(User currentuser)
            :this()
        {
            // TODO: Complete member initialization
            this.currentuser = currentuser;
            ConfigParams.Add(new ConfigParamModel {Changer="user",Description=currentuser.Name,Name="Потребител",Value=currentuser.UserName});
        }
        public ObservableCollection<ConfigParamModel> ConfigParams { get; set; }
        public void ChangeConfigValue(int index,string value,string name)
        {
            ConfigParams[index].Value = value;
            ConfigParams[index].Description = name;
                  
        }

        public string Holding
        {
            get { return _holding; }
            set { _holding = value; OnPropertyChanged("Holding"); }
        }
    }
}
