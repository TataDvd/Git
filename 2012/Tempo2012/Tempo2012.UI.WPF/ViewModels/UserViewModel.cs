using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class UserViewModel:BaseViewModel
    {
        public UserViewModel()
        {
            IsShowAddNew = false;
            IsShowSearch = false;
            IsShowReport = false;
            Users = new ObservableCollection<User>(Context.GetAllUsers());
            if (Users.Count > 0) CurrentUser = Users[0];
        }

        public string Name
        {
            get { return CurrentUser.Name; }
            set { CurrentUser.Name = value;OnPropertyChanged("Name"); }
        }

        public string UserName
        {
            get { return CurrentUser.UserName; }
            set { CurrentUser.UserName = value;OnPropertyChanged("UserName"); }
        }

        public string PassWord
        {
            get { return CurrentUser.PassWord; }
            set { CurrentUser.PassWord = value;OnPropertyChanged("PassWord"); }
        }

        public bool CanAnaliticalReport { get { return CurrentUser.CanAnaliticalReport; } set { CurrentUser.CanAnaliticalReport = value; OnPropertyChanged("CanAnaliticalReport"); } }
        public bool CanUpdateConto { get { return CurrentUser.CanUpdateConto; } set { CurrentUser.CanUpdateConto = value; OnPropertyChanged("CanUpdateConto"); } }
        public bool CanUpdateSaldo { get { return CurrentUser.CanUpdateSaldo; } set { CurrentUser.CanUpdateSaldo = value; OnPropertyChanged("CanUpdateSaldo"); } }
        public bool CanUpdateAcc { get { return CurrentUser.CanUpdateAcc; } set { CurrentUser.CanUpdateAcc = value; OnPropertyChanged("CanUpdateAcc"); } }
        public bool CanStoreReports { get { return CurrentUser.CanStoreReports; } set { CurrentUser.CanStoreReports = value; OnPropertyChanged("CanStoreReports"); } }
        public bool CanReportPeriodi { get { return CurrentUser.CanReportPeriodi; } set { CurrentUser.CanReportPeriodi = value; OnPropertyChanged("CanReportPeriodi"); } }
        public bool CanOborotReport { get { return CurrentUser.CanOborotReport; } set { CurrentUser.CanOborotReport = value; OnPropertyChanged("CanOborotReport"); } }
        public bool CanNewCurrency { get { return CurrentUser.CanNewCurrency; } set { CurrentUser.CanNewCurrency = value; OnPropertyChanged("CanNewCurrency"); } }
        public bool CanHronologicalReport { get { return CurrentUser.CanHronologicalReport; } set { CurrentUser.CanHronologicalReport = value; OnPropertyChanged("CanHronologicalReport"); } }
        public bool CanFinishYear { get { return CurrentUser.CanFinishYear; } set { CurrentUser.CanFinishYear = value; OnPropertyChanged("CanFinishYear"); } }
        public bool CanFinishMonth { get { return CurrentUser.CanFinishMonth; } set { CurrentUser.CanFinishMonth = value; OnPropertyChanged("CanFinishMonth"); } }
        public bool CanDeleteConto { get { return CurrentUser.CanDeleteConto; } set { CurrentUser.CanDeleteConto = value; OnPropertyChanged("CanDeleteConto"); } }
        public bool CanClasses { get { return CurrentUser.CanClasses; } set { CurrentUser.CanClasses = value; OnPropertyChanged("CanClasses"); } }
        public bool CanBalansReport { get { return CurrentUser.CanBalansReport; } set { CurrentUser.CanBalansReport = value; OnPropertyChanged("CanBalansReport"); } }
        public bool CanAddStore { get { return CurrentUser.CanAddStore; } set { CurrentUser.CanAddStore = value; OnPropertyChanged("CanAddStore"); } }
        public bool CanAddCurrencyRates { get { return CurrentUser.CanAddCurrencyRates; } set { CurrentUser.CanAddCurrencyRates = value; OnPropertyChanged("CanAddCurrencyRates"); } }
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("UserName");
                OnPropertyChanged("PassWord");
                OnPropertyChanged("CanAnaliticalReport");
                OnPropertyChanged("CanUpdateConto");
                OnPropertyChanged("CanUpdateSaldo");
                OnPropertyChanged("CanUpdateAcc");
                OnPropertyChanged("CanStoreReports");
                OnPropertyChanged("CanReportPeriodi");
                OnPropertyChanged("CanOborotReport");
                OnPropertyChanged("CanNewCurrency");
                OnPropertyChanged("CanBalansReport");
                OnPropertyChanged("CanClasses");
                OnPropertyChanged("CanDeleteConto");
                OnPropertyChanged("CanFinishMonth");
                OnPropertyChanged("CanFinishYear");
                OnPropertyChanged("CanAddStore");
                OnPropertyChanged("CanAddCurrencyRates");
               
            }
        }

        public ObservableCollection<User> Users { get; set;}

        protected override void MoveFirst()
        {
            CurrentUser = Users.First();
        }

        protected override void MoveNext()
        {
            int index = Users.IndexOf(CurrentUser);

            if (index < Users.Count - 1)
                CurrentUser= Users.ElementAt(index + 1);
           
        }

        protected override void MovePrevius()
        {
            int index = Users.IndexOf(CurrentUser);

            if (index >= 1)
                CurrentUser = Users.ElementAt(index - 1);
        }
        protected override void MoveLast()
        {
            CurrentUser = Users.Last();
        }

        protected override void Add()
        {
            CurrentUser=new User();
            base.Add();
        }
        protected override void Save()
        {
            if (Mode==EditMode.Add) Users.Add(CurrentUser);
            Context.SaveUser(CurrentUser);
            //View();
        }
    }
}
