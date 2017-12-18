using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.ViewModels
{
    
    public class CopyFirmToFirmViewModel:BaseViewModel
    {

        public CopyFirmToFirmViewModel()
            : base()
        {
            _allFirms = new ObservableCollection<FirmModelWraper>();
            foreach ( var item in Context.GetAllFirma())
            {
                _allFirms.Add(new FirmModelWraper(item));
            }
            CopyCommand = new DelegateCommand((o) => this.CopyFirmToFirm(),(o)=>this.CanCopyFirmToFirm());
        }

        private bool CanCopyFirmToFirm()
        {
            return (CurrentFirmaWraperDest != null) && (CurrentFirmaWraperSource != null);
        }

        private void CopyFirmToFirm()
        {
            var allacc = Context.GetAllAccounts(CurrentFirmaWraperSource.Id);
            var oldcursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            var AllAnaliticalFields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            var AllConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
            var AlaMapToType = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            foreach (AccountsModel accountsModel in allacc)
            {
                accountsModel.FirmaId = CurrentFirmaWraperDest.Id;
                var SelectedConnectors =
                       new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                           AllConnectors.Where(e => e.AnaliticalNameID == accountsModel.AnaliticalNum));
                ObservableCollection<AnaliticalFields> SelectedAnaliticalFields=new ObservableCollection<AnaliticalFields>();
                foreach (var curr in SelectedConnectors)
                {
                    var addfield = AllAnaliticalFields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                    if (addfield != null)
                    {
                        addfield.Requared = curr.Required;
                        if (addfield != null) SelectedAnaliticalFields.Add(addfield);
                    }
                }
                Context.LoadMapToLookUps(SelectedAnaliticalFields, accountsModel.Id, accountsModel.AnaliticalNum);
                var CurrentAllTypeAccount = AllAnaliticTypes.Where(e => e.Id == accountsModel.TypeAnaliticalKey).FirstOrDefault();
                //SelectedConnectors =
                //    new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                //        AlaMapToType.Where(e => e.AnaliticalFieldId == accountsModel.TypeAnaliticalKey));
                //ObservableCollection<AnaliticalFields> SelectedAnaliticalTypeFields=new ObservableCollection<AnaliticalFields>();
                //foreach (var curr in SelectedConnectors)
                //{
                //    var addfield = AllAnaliticalFields.FirstOrDefault(e => e.Id == curr.AnaliticalNameID);
                //    if (addfield != null)
                //    {
                //        addfield.Requared = curr.Required;
                //        SelectedAnaliticalTypeFields.Add(addfield);
                //    }
                //}
                string errormessage;
                if (!Context.UpdateAccount(accountsModel, true, SelectedAnaliticalFields, out errormessage))
                {
                    MessageBoxWrapper.Show(string.Format(" Грешка при копиране на сметкоплана от фирма {0} е копиран на фирма {1} заради грешка {2}",
                        CurrentFirmaWraperSource.CurrentFirma.Name,
                        CurrentFirmaWraperDest.CurrentFirma.Name,
                        errormessage), "Предупреждение");
                }
            }
            Mouse.OverrideCursor = oldcursor;
            MessageBoxWrapper.Show(string.Format("Сметкоплана от Фирма {0} е копиран на {1}", CurrentFirmaWraperSource.CurrentFirma.Name,CurrentFirmaWraperDest.CurrentFirma.Name),"Известие");


        }

       

        public ICommand CopyCommand { get; private set;}

        private ObservableCollection<FirmModelWraper> _allFirms;
        public ObservableCollection<FirmModelWraper> AllFirms
        {
            get { return _allFirms; }
            set { _allFirms = value; }
        }

        private FirmModelWraper _currentFirmaWraperDest;
        public FirmModelWraper CurrentFirmaWraperDest
        {
            get { return _currentFirmaWraperDest; }
            set {
                _currentFirmaWraperDest = value;
                 OnPropertyChanged("CurrentFirmaWraperDest");
                
            }

        }
        private FirmModelWraper _currentFirmaWraperSource;
        public FirmModelWraper CurrentFirmaWraperSource
        {
            get { return _currentFirmaWraperSource; }
            set {
                _currentFirmaWraperSource = value;
                 OnPropertyChanged("CurrentFirmaWraperSource");
       
            }
        }

        
    }
}
