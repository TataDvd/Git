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
    public enum TypeAccountEnum
    {
        IsActive=1,
        IsActivePasiv=2,
        IsPasiv=3,
        IsTranzitiv=4
    }
    public enum LevelAccout
    {
        IsSintetic = 1,
        IsAnalitic =2 
    }
    public enum TypeSaldo
    {
        IsCompens=1,
        IsExpand=2
    }
    
    public class AccountsViewModel:BaseViewModel
    {
        public AccountsViewModel():base()
        {
            AllNationalAccounts = new ObservableCollection<LookUpSpecific>(context.GetAllNationalAccounts());
            AllAnaliticalAccount =new ObservableCollection<AnaliticalAccount>(context.GetAllAnaliticalAccount());
            AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(context.GetAllAnaliticalAccountType());
            AllAnaliticalFields =new ObservableCollection<AnaliticalFields>(context.GetAllAnaliticalFields());
            AllConnectors =new ObservableCollection<ConectorAnaliticField>(context.GetAllConnectorAnaliticField());
            AllAccountsFirms = new ObservableCollection<AccountsModel>(context.GetAllAccounts());
            AllAccounts = new ObservableCollection<AccountsModel>(context.GetAllAccounts().Where(e => e.FirmaId == ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            AllLookups=new ObservableCollection<LookUpMetaData>(context.GetAllLookups());
            SelectedAnaliticalFields = new ObservableCollection<AnaliticalFields>();
            if (AllAccounts.Count > 0) _CurrentAccount = AllAccounts.Last();
            else
            {
                _CurrentAccount = new AccountsModel
                {
                    AnaliticalNum = 1,
                    LevelAccount = 1,
                    SubNum = 1,
                    FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    Id = 1,
                    TypeSaldo=1,
                    TypeAccount=1,
                    Num=1,
                    PartidNum=0
                };
                AllAccounts.Add(_CurrentAccount);
            }
               
            
        }
        protected override void  Save()
        {
            //List<AccountsModel> fm = new List<AccountsModel>(AllAccountsFirms);

            OnPropertyChanged("CurrentAccount");
            if (this.CanSave())
            {
                AccountsModel transport = _CurrentAccount.Clone();
                if (Mode == EditMode.Add)
                {

                    if (context.UpdateAccount(transport, true))
                    {
                        base.Save();
                        CurrentAccount = transport;
                    }
                    else
                    {
                        MessageBox.Show("Грешка");
                    }
                }
                else
                {
                    if (!context.UpdateAccount(transport, false))
                    {
                        MessageBox.Show("Грешка");

                    }
                    else
                    {
                        base.Save();
                    }
                }
            }
            else
            {
                MessageBox.Show("Невалидни данни! Моля проверете данните маркирани с червено!");
            }
            context.UpdateAccount(_CurrentAccount, true);
        }

        protected override void  Delete()
        {
            AllAccounts.Remove(_CurrentAccount);
            AllAccountsFirms.Remove(_CurrentAccount);
            _CurrentAccount = AllAccounts.Last();
        }

        protected override void Add()
        {
            base.Add();
            var model =_CurrentAccount.Clone();
            AllAccounts.Add(model);
            AllAccountsFirms.Add(model);
            _CurrentAccount = model;
        }

           
        private TypeSaldo _TypeSaldoIn;
        public TypeSaldo TypeSaldoIn
        {
            get { return _TypeSaldoIn; }
            set
            {
                _TypeSaldoIn = value;
                if (value == TypeSaldo.IsCompens) _CurrentAccount.TypeSaldo = 1;
                if (value == TypeSaldo.IsExpand)  _CurrentAccount.TypeSaldo = 2;
                OnPropertyChanged("TypeSaldoIn");
            }
        }
        private LevelAccout _LevelAccountIn;
        public LevelAccout LevelAccountIn {
            get { return _LevelAccountIn; }
            set
            {
                _LevelAccountIn = value;
                if (value == LevelAccout.IsAnalitic) _CurrentAccount.LevelAccount = 2;
                if (value == LevelAccout.IsSintetic) _CurrentAccount.LevelAccount = 1;
                OnPropertyChanged("LevelAccountIn");
            }
        }
        private TypeAccountEnum _TypeAccountEnumIn;
        public TypeAccountEnum TypeAccountEnumIn
        {
            get { return _TypeAccountEnumIn; }
            set
            {
                if (value==TypeAccountEnum.IsActive) _CurrentAccount.TypeAccount = 1;
                if (value==TypeAccountEnum.IsActivePasiv) _CurrentAccount.TypeAccount = 2;
                if (value==TypeAccountEnum.IsPasiv) _CurrentAccount.TypeAccount = 3;
                if (value==TypeAccountEnum.IsTranzitiv) _CurrentAccount.TypeAccount = 4;
                _TypeAccountEnumIn = value;
                OnPropertyChanged("TypeAccountEnumIn");
                
            }
        }

        private AnaliticalAccountType _TypeAccountIn;
        public AnaliticalAccountType TypeAccountIn
        {
            get { return _TypeAccountIn; }
            set
            {
                _TypeAccountIn = value;
                OnPropertyChanged("TypeAccountIn");
            }
        }
        private AccountsModel _CurrentAccount;
        public string NameMain
        {
            get
            {
                return _CurrentAccount.NameMain;
            }
            set
            {
                _CurrentAccount.NameMain = value;
                OnPropertyChanged("NameMain");
            }
        }
        public string NameMainEng
        {
            get
            {
                return _CurrentAccount.NameMainEng;
            }
            set
            {
            	_CurrentAccount.NameMainEng = value;
                OnPropertyChanged("NameMainEng");
            }
        }
        public int Id
        {
            get { return _CurrentAccount.Id; }
            set
            {
                _CurrentAccount.Id= value;
                OnPropertyChanged("Id");
            }
        }
        public int CurrLevelAccout
        {
            get { return _CurrentAccount.LevelAccount; }
            set
            {
                _CurrentAccount.LevelAccount = value;
                OnPropertyChanged("CurrLevelAccout");
            }
        }
        public int FirmaId
        {
            get { return _CurrentAccount.FirmaId; }
            set
            {
                _CurrentAccount.FirmaId = value;
                OnPropertyChanged("FirmaId");
            }
        }
        public int PartidNum
        {
            get { return _CurrentAccount.PartidNum; }
            set 
            {
                _CurrentAccount.PartidNum = value;
                OnPropertyChanged("PartidNum");
            }
        }
        public int SubNum
        {
            get
            {
                return _CurrentAccount.SubNum;
            }
            set
            {
                _CurrentAccount.SubNum = value;
                OnPropertyChanged("SubNum");
            }
        }
        public int Num
        {
            get { return _CurrentAccount.Num; }
            set
            {
                _CurrentAccount.Num= value;
                OnPropertyChanged("Num");
            }
        }
        public AccountsModel CurrentAccount
        {
            get
            {
                return _CurrentAccount;
            }
            set
            {
                if (value == null) return;
                _CurrentAccount = value;

                if (_CurrentAccount.LevelAccount == 1)
                {
                    LevelAccountIn = LevelAccout.IsSintetic;
                }
                else
                {
                    LevelAccountIn = LevelAccout.IsAnalitic;
                }
                if (_CurrentAccount.TypeSaldo == 1)
                {
                    TypeSaldoIn = TypeSaldo.IsCompens;
                }
                else { TypeSaldoIn = TypeSaldo.IsExpand; }
                if (_CurrentAccount.TypeAccount == 1) TypeAccountEnumIn = TypeAccountEnum.IsActive;
                if (_CurrentAccount.TypeAccount == 2) TypeAccountEnumIn = TypeAccountEnum.IsActivePasiv;
                if (_CurrentAccount.TypeAccount == 3) TypeAccountEnumIn = TypeAccountEnum.IsPasiv;
                if (_CurrentAccount.TypeAccount == 4) TypeAccountEnumIn = TypeAccountEnum.IsTranzitiv;
                CurrentAllAnaliticalAccount = AllAnaliticalAccount.Where(e => e.Id == value.AnaliticalNum).FirstOrDefault();
                OnPropertyChanged("NameMain");
                OnPropertyChanged("NameMainEng");
                OnPropertyChanged("Id");
                OnPropertyChanged("CurrLevelAccout"); 
                OnPropertyChanged("FirmaId");
                OnPropertyChanged("SubNum");
                OnPropertyChanged("CurrentAccount");
                OnPropertyChanged("TypeSaldoIn");
                OnPropertyChanged("LevelAccountIn");
                OnPropertyChanged("TypeAccountEnumIn");
                OnPropertyChanged("AllAnaliticalAccount");
            }
        }
        private AnaliticalAccount _CurrentAllAnaliticalAccount;
        public AnaliticalAccount CurrentAllAnaliticalAccount
        {
            get { return _CurrentAllAnaliticalAccount;}
            set
            {
                _CurrentAllAnaliticalAccount = value;
                if (value != null)
                {
                    _CurrentAccount.AnaliticalNum = value.Id;
                    TypeAccountIn = AllAnaliticTypes.Where(e => e.Id == value.TypeID).FirstOrDefault();
                    if (TypeAccountIn != null)
                    {
                        SelectedConnectors = new ObservableCollection<ConectorAnaliticField>(AllConnectors.Where(e => e.AnaliticalNameID == TypeAccountIn.Id));
                        SelectedAnaliticalFields.Clear();
                        foreach (var curr in SelectedConnectors)
                        {
                            var addfield = AllAnaliticalFields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                            if (addfield!=null) SelectedAnaliticalFields.Add(addfield);
                        }
                    }
                }
                OnPropertyChanged("SelectedAnaliticalFields");
                OnPropertyChanged("CurrentAllAnaliticalAccount");
            }
            
        }
        private AnaliticalAccountType _TypeAccount;
        public AnaliticalAccountType TypeAccount
        {
            get
            {
                return _TypeAccount;
            }
            set
            {
                _TypeAccount = value;
                OnPropertyChanged("TypeAccount");
            }
        }
        public ObservableCollection<AccountsModel> AllAccountsFirms { get; set; }
        public ObservableCollection<AccountsModel> AllAccounts { get; set;}
        public ObservableCollection<AnaliticalAccount> AllAnaliticalAccount{get;set;}
        public ObservableCollection<AnaliticalFields> AllAnaliticalFields { get; set; }
        public ObservableCollection<ConectorAnaliticField> AllConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> AllAnaliticTypes { get; set;}
        public ObservableCollection<AccountsModel> SelectedAccounts { get; set; }
        public ObservableCollection<AnaliticalAccount> SelectedAnaliticalAccount { get; set; }
        public ObservableCollection<AnaliticalFields> SelectedAnaliticalFields { get; set; }
        public ObservableCollection<ConectorAnaliticField> SelectedConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> SelectedAnaliticTypes { get; set; }
        public ObservableCollection<LookUpSpecific> AllNationalAccounts { get; set; }
        public ObservableCollection<LookUpMetaData> AllLookups { get; set; }
        
    }
}
