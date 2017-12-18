using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Properties;
using Tempo2012.UI.WPF.ViewModels.treeviewmodel;
using Tempo2012.UI.WPF.Views.Dialogs;

namespace Tempo2012.UI.WPF.ViewModels.AccountManagment
{
    public class TreeManagerModalViewModel : BaseViewModel
    {
        public TreeManagerModalViewModel()
            : base()
        {
            this.IsShowAddNew = true;
            this.IsShowNavigation = false;
            Refresh();
        }

        private void Refresh()
        {
            _treeAcc=new ObservableCollection<TreeViewModel>();
            AllAccounts =
                new ObservableCollection<AccountsModel>(
                    Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            int oldnum = 0;
            //TreeViewModel curitem = new TreeViewModel();
            //foreach (var item in AllAccounts)
            //{
            //    TreeViewModel ni = new TreeViewModel();
            //    ni.CurrAcc = item;
            //    if (item.SubNum>0)
            //    {
            //        curitem.SubAccs.Add(ni);
            //    }
            //    else
            //    {
            //        _treeAcc.Add(ni);
            //        curitem = ni;
            //        oldnum = item.Num;
            //    }
            //}
            //OnPropertyChanged("Tree");
        }


        private ObservableCollection<TreeViewModel> _treeAcc;
        public ObservableCollection<TreeViewModel> Tree
        {
            get 
            {
                return _treeAcc;
            }
            set 
            { 
                _treeAcc=value;
                OnPropertyChanged("Tree");
            }
        }
        public ObservableCollection<AccountsModel> AllAccounts { get; set; }
        public void EditFromOutside()
        {
            if (CurrentAccount != null)
            {

                if (CurrentAccount.Num<1) return;
                if (CurrentAccount.SubNum == 0)
                {
                    MainAcc ds = new MainAcc(CurrentAccount, EditMode.Edit,true);
                    ds.ShowDialog();
                    if (ds.DialogResult.HasValue && ds.DialogResult.Value)
                    {
                        Refresh();
                    }
                }
                else

                {
                    MainAcc dSubAcc = new MainAcc(CurrentAccount, EditMode.Edit,false);
                    dSubAcc.ShowDialog();
                    if (dSubAcc.DialogResult.HasValue && dSubAcc.DialogResult.Value)
                    {
                        Refresh();
                    }
                }
               ;
            }
            else
            {
                MessageBoxWrapper.Show("Моля, изберете сметка");
            }
        }
        private AccountsModel _CurrentAccount;
        public AccountsModel CurrentAccount 
        {
            get 
            { 
                return _CurrentAccount;
            } 
            set 
            { 
                _CurrentAccount = value;
                OnPropertyChanged("CurrentAccount");
            } 
        }
        public override string TitleInsert
        {
            get
            {
                return "F2-Нова подсметка";
            }
        }

        public DelegateCommand SaldoCommand { get; set; }
    }
}
