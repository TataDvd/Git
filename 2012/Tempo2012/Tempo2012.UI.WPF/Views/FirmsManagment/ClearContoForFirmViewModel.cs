using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.Views.FirmsManagment
{
    public  class ClearContoForFirmViewModel:BaseViewModel
    {
        public ClearContoForFirmViewModel()
        {
            _allFirms = new ObservableCollection<FirmModelWraper>();
            foreach (var item in Context.GetAllFirma())
            {
                _allFirms.Add(new FirmModelWraper(item));
            }
        }
        private ObservableCollection<FirmModelWraper> _allFirms;
        public ObservableCollection<FirmModelWraper> AllFirms
        {
            get { return _allFirms; }
            set { _allFirms = value;OnPropertyChanged("AllFirms"); }
        }
       
        private FirmModelWraper _currentFirmaWraper;
        public FirmModelWraper CurrentFirmaWraper
        {
            get { return _currentFirmaWraper; }
            set
            {
                _currentFirmaWraper = value;
                OnPropertyChanged("CurrentFirmaWraper");

            }

        }
        protected override void AddNew()
        {  
            if (CurrentFirmaWraper==null)
            {
                MessageBoxWrapper.Show("Не сте избрали фирма за почистване");
                return;
            }
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                var oldcursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Context.DeleteContos(CurrentFirmaWraper.Id,reportMenuProvider.Vm.FromDate(),reportMenuProvider.Vm.ToDate());
                Mouse.OverrideCursor = oldcursor;
            }
        }
        protected override void Add()
        {
            if (CurrentFirmaWraper==null)
            {
                MessageBoxWrapper.Show("Не сте избрали фирма за почистване");
                return;
            }
            if (MessageBoxWrapper.Show(string.Format("Сигурен ли си ,че искаш да изтриеш всички контировки на фирма - {0}?", CurrentFirmaWraper.Name), "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                var oldcursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Context.DeleteAllConto(CurrentFirmaWraper.Id);
                Mouse.OverrideCursor = oldcursor;
            }
        }
    }
}