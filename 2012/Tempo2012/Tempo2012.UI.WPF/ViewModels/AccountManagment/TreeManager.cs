using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Properties;
using Tempo2012.UI.WPF.ViewModels.treeviewmodel;
using Tempo2012.UI.WPF.Views.Dialogs;



namespace Tempo2012.UI.WPF.ViewModels.AccountManagment
{
    public class TreeManagerViewModel : BaseViewModel
    {
        public TreeManagerViewModel()
            : base()
        {
            this.IsShowAddNew = true;
            this.IsShowNavigation = false;
            this.SaldoCommand = new DelegateCommand((o) => this.Saldo());
            Refresh();
        }

        private void Saldo()
        {
            if (CurrentAccount != null)
            {
                 SaldosAnalitic ds = new SaldosAnalitic(CurrentAccount);
                 ds.ShowDialog();
          

            }
        }

        public void Saldo(AccountsModel ca)
        {
            SaldosAnalitic ds = new SaldosAnalitic(ca);
            ds.ShowDialog();
        }
        private void Refresh()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _treeAcc=new ObservableCollection<TreeViewModel>();
            AllAccounts =
                new ObservableCollection<AccountsModel>(
                    Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            int oldnum = 0;
            TreeViewModel curitem = new TreeViewModel();
            foreach (var item in AllAccounts)
            {
                TreeViewModel ni = new TreeViewModel();
                ni.CurrAcc = item;
                if (item.SubNum>0)
                {
                    curitem.SubAccs.Add(ni);
                    if (item.AnaliticalNum > 0)
                    {
                          var nomen = Context.LoadMapToLookUps(item.Id, item.AnaliticalNum);
                       
                        foreach (var curritem in nomen)
                        {    TreeViewModel nii = new TreeViewModel();
                             nii.CurrAcc = new AccountsModel { NameMain ="Връзка към номенклатура "+curritem.Key,Num=-2 };
                             ni.SubAccs.Add(nii);
                             //foreach (string s in curritem.Value)
                             //{
                             //    TreeViewModel niii = new TreeViewModel();
                             //    niii.CurrAcc = new AccountsModel { NameMain = s, Num = -1 };
                             //    nii.SubAccs.Add(niii);
                             //}
                        }
                    }
                }
                else
                {
                    _treeAcc.Add(ni);
                    if (item.AnaliticalNum > 0)
                    {
                        //var butaforgo = new TreeViewModel();
                        //butaforgo.IsExpanded = true;
                        //butaforgo.CurrAcc=new AccountsModel{NameMain = "- ",Num=-2};
                        //ni.SubAccs.Add(butaforgo);
                        var nomen = Context.LoadMapToLookUps(item.Id, item.AnaliticalNum);
                       
                        foreach (var list in nomen)
                        {  
                            TreeViewModel nii = new TreeViewModel();
                            nii.CurrAcc = new AccountsModel { NameMain = "Връзка към номенклатура " + list.Key, Num = -1 };
                            ni.SubAccs.Add(nii);
                            //foreach (string s in list.Value)
                            //{
                            //    TreeViewModel niii = new TreeViewModel();
                            //    niii.CurrAcc = new AccountsModel { NameMain = s, Num = -1 };
                            //    nii.SubAccs.Add(niii);
                            //}
                        }
                    }
                    curitem = ni;
                    oldnum = item.Num;
                }
            }
            OnPropertyChanged("Tree");
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        protected override void Update()
        {
            EditFromOutside();
        }
        protected override void Add()
        {
            if (CurrentAccount == null)
            {
                MessageBoxWrapper.Show("Не сте избрали сметка. Моля маркирайте някоя сметка","Предупреждение");
                return;
            }

            if (CurrentAccount.Num <= 0) return;
            if (CurrentAccount.Num > 0 && CurrentAccount.SubNum > 0)
            {
                
            }
            else
            {
                AddSubAcc();
                 
            }

           
           
        }
        protected override void AddNew()
        {
             AddAcc();
        }

        private void AddAcc()
        {
            MainAcc mainAcc = new MainAcc(new AccountsModel { FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, TypeAccountEx = 1,LevelAccount = 1}, EditMode.Add,true);
            mainAcc.ShowDialog();
            Refresh();
           

        }
        public void AddSubAcc()
        {
            if (CurrentAccount!=null && CurrentAccount.Num>0)
            {
                MainAcc subAcc = new MainAcc(new AccountsModel
                {
                    Num = CurrentAccount.Num,
                    FirmaId = CurrentAccount.FirmaId,
                    TypeAccountEx = CurrentAccount.TypeAccountEx,
                    LevelAccount = CurrentAccount.LevelAccount,
                    TypeAccount = CurrentAccount.TypeAccount
                }, EditMode.Add, false, CurrentAccount.ShortName);
                subAcc.ShowDialog();
                Refresh();
               
                
            }
            else
            {
                MessageBoxWrapper.Show("Не сте маркирали основна сметка!");
            }

        }

        protected override void Delete()
        {
            if (CurrentAccount != null)
            {
                if (CurrentAccount.Num <0) return;
                if (MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_Delete_res3, Resources.AnaliticManagerViewModel_Delete_res4, MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    if (Context.DeleteAccount(CurrentAccount.Id))
                    {
                        CurrentAccount = null;
                        Refresh();
                     }
                    else
                    {
                        MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_Delete_res2);
                    }
                }
                
            }
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
                    MainAcc ds = new MainAcc(CurrentAccount, EditMode.Edit,true,"",true);
                    ds.ShowDialog();
                    if (ds.DialogResult.HasValue && ds.DialogResult == true)
                    {
                        Refresh();
                    }
                }
                else

                {
                    var parent = AllAccounts.FirstOrDefault(e => e.Num == CurrentAccount.Num && e.SubNum == 0);
                    if (parent != null)
                    {
                        MainAcc dSubAcc = new MainAcc(CurrentAccount, EditMode.Edit,false,parent.ShortName,true);
                        dSubAcc.ShowDialog();
                        if (dSubAcc.DialogResult.HasValue && dSubAcc.DialogResult == true)
                        {
                            Refresh();
                        }
                    }
                    else
                    {
                        MainAcc dSubAcc = new MainAcc(CurrentAccount, EditMode.Edit,false,"",true);
                        dSubAcc.ShowDialog();
                        if (dSubAcc.DialogResult.HasValue && dSubAcc.DialogResult == true)
                        {
                            Refresh();
                        }
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
