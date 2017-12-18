using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.FakeData;
using System.Windows.Input;
using System.Windows;

namespace Tempo2012.UI.WPF.ViewModels
{

    public class MainAccountsViewModel : BaseViewModel
    {
        private AccountsModel _CurrentAccount;
        public AccountsModel CurrentAccount { get { return _CurrentAccount; } set { _CurrentAccount = value; } }

        public MainAccountsViewModel()
            : base()
        {
            AllAccounts = new ObservableCollection<AccountsModel>(context.GetAllAccounts().Where(e => e.FirmaId == ConfigTempoSinglenton.GetInstance().CurrentFirma.Id && e.SubNum==0 && e.AnaliticalNum==0 && e.PartidNum==0));
            if (AllAccounts.Count > 0) _CurrentAccount = AllAccounts.Last();
            else
            {
                _CurrentAccount = new AccountsModel
                {
                    AnaliticalNum = 0,
                    LevelAccount = 1,
                    SubNum = 0,
                    FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    Id = 1,
                    TypeSaldo = 1,
                    TypeAccount = 1,
                    Num = 1,
                    PartidNum = 0
                };
                AllAccounts.Add(_CurrentAccount);
            }
            AllNationalAccounts = new ObservableCollection<LookUpSpecific>(context.GetAllNationalAccounts());
        }

        public ObservableCollection<AccountsModel> AllAccounts { get; set; }

        public ObservableCollection<LookUpSpecific> AllNationalAccounts { get; set; }
    }
}
