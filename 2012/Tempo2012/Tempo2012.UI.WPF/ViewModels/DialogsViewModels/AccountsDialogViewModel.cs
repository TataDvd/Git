using System;
using System.Diagnostics;
using System.Linq;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.MainValidators;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;


namespace Tempo2012.UI.WPF.ViewModels
{
    public enum TypeAccountEnum
    {
        IsActive = 1,
        IsActivePasiv = 2,
        IsPasiv = 3,
        IsTranzitiv = 4
    }

    public enum LevelAccout
    {
        IsSintetic = 1,
        IsAnalitic = 2
    }

    public enum TypeSaldo
    {
        IsCompens = 1,
        IsExpand = 2
    }

    public enum TypeAccountEx
    {
        IsNormal=1,
        IsBujest = 2,
        IsOwn = 3
    }
    public class AccountsDialogViewModel : BaseViewModel,IDataErrorInfo
    {
        public AccountsDialogViewModel(AccountsModel acc, EditMode mode,bool ismain,string parent="",bool saveAndClose = false)
            : base()
        {
            SaveAndClose = saveAndClose;
            ParentNum = parent;
            AllNationalAccounts = new ObservableCollection<LookUpSpecific>(Context.GetAllNationalAccounts());
            AllAnaliticalAccount = new ObservableCollection<AnaliticalAccount>(Context.GetAllAnaliticalAccount());
            AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            AllAnaliticalFields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            AllConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
            AlaMapToType = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            //AllAccountsFirms = new ObservableCollection<AccountsModel>(context.GetAllAccounts());
            AllAccounts =
                new ObservableCollection<AccountsModel>(
                    Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            SelectedAnaliticalFields = new ObservableCollection<AnaliticalFields>();
            SelectedAnaliticalTypeFields = new ObservableCollection<AnaliticalFields>();
            decimal sumalvksub, sumalvdsub;
            Context.GetAllMovementsSaldos(acc.Id, acc.Num, acc.FirmaId, out sumalvksub,out sumalvdsub);
            acc.SubSaldoKL = sumalvksub;
            acc.SubSaldoDL = sumalvdsub;
            string sumavd, sumavk, sumakd, sumakk, sumalvk, sumalvd;
            Context.GetAllMovementsSalosVK(acc.Id, out sumalvk, out sumalvd, out sumavd, out sumavk, out sumakd, out sumakk);
            acc.SaldoDV=decimal.Parse(sumavd);
            acc.SaldoKV=decimal.Parse(sumavk);
            if (acc.SaldoDV>0 ||acc.SaldoKV>0)
            {
                ValutaVisible = true;
            }
            if (acc.SaldoKK>0 || acc.SaldoDK>0)
            {
                ColVisible = true;
            }
            if (mode!= EditMode.Edit && ismain )
            {
                var lookup = saveAndClose?AllNationalAccounts.FirstOrDefault(е => е.CodetId == acc.Num.ToString()):AllNationalAccounts.FirstOrDefault(); 
                if (lookup != null)
                {
                    //acc.NameMain = lookup.Name;
                    //int i;
                    //if (int.TryParse(lookup.CodetId,out i))
                    //{
                    //    acc.Num = i;
                    //}
                }
                else
                {
                    NoAcc = true;
                    return;
                }
                //if (lookup!= null)
                //{
                //    acc.NameMain = lookup.Name;
                //}
                //else
                //{
                //    //System.Windows.MessageBoxWrapper.Show(string.Format("Няма сметка с номер {0} в Националния сметкоплан",acc.Num));
                //}
            }
            CurrentAccount = acc;
            Mode = mode;
            if (mode==EditMode.Edit)
            {
                Update();
            }
            if (mode==EditMode.Add)
            {
                Add();
            }
        }

        public ObservableCollection<AccountsModel> AllAccountsFirms { get; set; }
        public ObservableCollection<AccountsModel> AllAccounts { get; set; }
        public ObservableCollection<AnaliticalAccount> AllAnaliticalAccount { get; set; }
        public ObservableCollection<AnaliticalFields> AllAnaliticalFields { get; set; }
        public ObservableCollection<MapAnanaliticAccToAnaliticField> AllConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> AllAnaliticTypes { get; set; }
        public ObservableCollection<AccountsModel> SelectedAccounts { get; set; }
        public ObservableCollection<AnaliticalAccount> SelectedAnaliticalAccount { get; set; }
        public ObservableCollection<AnaliticalFields> SelectedAnaliticalFields { get; set; }
        public ObservableCollection<MapAnanaliticAccToAnaliticField> SelectedConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> SelectedAnaliticTypes { get; set; }
        public ObservableCollection<LookUpSpecific> AllNationalAccounts { get; set; }
        public ObservableCollection<MapAnanaliticAccToAnaliticField> AlaMapToType { get; set; }
        public ObservableCollection<AnaliticalFields> SelectedAnaliticalTypeFields { get; set; }

        private bool _showMain;

        public bool ShowMain
        {
            get { return _showMain; }
            set
            {
                _showMain = value;
                OnPropertyChanged("ShowMain");
            }
        }

        public string Txt
        {
            get { return _txt; }
            set 
            {  _txt = value;
                if (AllNationalAccounts.FirstOrDefault(e=>e.ToString().Contains(value)) == null)
                {
                    Num = 0;
                    NameMain = "";
                }
                OnPropertyChanged("Txt"); 
            }
        }

        private AnaliticalFields currselectedItem;

        public AnaliticalFields CurrSelectedItem
        {
            get { return currselectedItem; }
            set
            {
                currselectedItem = value;
                OnPropertyChanged("CurrSelectedItem");
            }
        }

        protected override void Save()
        {
            OnPropertyChanged("CurrentAccount");
            if (this.CanSave())
            {
                string errormesage;
                AccountsModel transport = _currentAccount.Clone();
                if (Mode == EditMode.Add)
                {

                    if (Context.UpdateAccount(transport, true, SelectedAnaliticalFields,out errormesage))
                    {
                        issave = true;
                        if (!IsMain)
                        {
                            SubNum++;
                        }
                        else
                        {
                            Num = 0;
                        }
                        NameMain = "";
                        MessageBoxWrapper.Show("Сметката е записана");
                    }
                    else
                    {
                        MessageBoxWrapper.Show(errormesage);
                        issave = false;
                    }
                }
                else
                {
                    if (!Context.UpdateAccount(transport, false, SelectedAnaliticalFields,out errormesage))
                    {
                        MessageBoxWrapper.Show(errormesage);
                        issave = false;
                    }

                }
            }
            else
            {
                MessageBoxWrapper.Show("Невалидни данни! Моля проверете данните маркирани с червено!");
                issave = false;
            }

        }


        private TypeSaldo _TypeSaldoIn;

        public TypeSaldo TypeSaldoIn
        {
            get { return _TypeSaldoIn; }
            set
            {
                _TypeSaldoIn = value;
                if (value == TypeSaldo.IsCompens) _currentAccount.TypeSaldo = 1;
                if (value == TypeSaldo.IsExpand) _currentAccount.TypeSaldo = 2;
                OnPropertyChanged("TypeSaldoIn");
            }
        }
              
        private LevelAccout _LevelAccountIn;

        public LevelAccout LevelAccountIn
        {
            get { return _LevelAccountIn; }
            set
            {
                _LevelAccountIn = value;
                if (value == LevelAccout.IsAnalitic) {_currentAccount.LevelAccount = 2;
                    _isAnalitic = true;
                    if (_currentAccount != null)
                        if (CurrentAllAnaliticalAccount != null)
                            _currentAccount.AnaliticalNum = CurrentAllAnaliticalAccount.Id;
                }
                if (value == LevelAccout.IsSintetic) 
                {
                    _currentAccount.LevelAccount = 1;
                    _isAnalitic = false;
                    if (_currentAccount != null) _currentAccount.AnaliticalNum = 0;
                }

                OnPropertyChanged("LevelAccountIn");
                OnPropertyChanged("IsAnalitic");
                OnPropertyChanged("EnableSaldo");
            }
        }

        private TypeAccountEx _typeAccountEx;
        public TypeAccountEx TypeAccountExIn
        {
            get { return _typeAccountEx; }
            set
            {
                if (value == TypeAccountEx.IsNormal) _currentAccount.TypeAccountEx = 1;
                if (value == TypeAccountEx.IsBujest) _currentAccount.TypeAccountEx = 2;
                if (value == TypeAccountEx.IsOwn) _currentAccount.TypeAccountEx = 3;
                
                _typeAccountEx = value;
                OnPropertyChanged("TypeAccountExIn");

            }
        }
        private TypeAccountEnum _TypeAccountEnumIn;

        public TypeAccountEnum TypeAccountEnumIn
        {
            get { return _TypeAccountEnumIn; }
            set
            {
                if (value == TypeAccountEnum.IsActive) _currentAccount.TypeAccount = 1;
                if (value == TypeAccountEnum.IsActivePasiv) _currentAccount.TypeAccount = 2;
                if (value == TypeAccountEnum.IsPasiv) _currentAccount.TypeAccount = 3;
                if (value == TypeAccountEnum.IsTranzitiv) _currentAccount.TypeAccount = 4;
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

        private AccountsModel _currentAccount;

        public AccountsModel CurrentAccount
        {
            get { return _currentAccount; }
            set
            {
                if (value == null) return;
                _currentAccount = value;

                LevelAccountIn = _currentAccount.LevelAccount == 1 ? LevelAccout.IsSintetic : LevelAccout.IsAnalitic;
                TypeSaldoIn = _currentAccount.TypeSaldo == 1 ? TypeSaldo.IsCompens:TypeSaldo.IsExpand;
                if (_currentAccount.TypeAccount == 1) TypeAccountEnumIn = TypeAccountEnum.IsActive;
                if (_currentAccount.TypeAccount == 2) TypeAccountEnumIn = TypeAccountEnum.IsActivePasiv;
                if (_currentAccount.TypeAccount == 3) TypeAccountEnumIn = TypeAccountEnum.IsPasiv;
                if (_currentAccount.TypeAccount == 4) TypeAccountEnumIn = TypeAccountEnum.IsTranzitiv;

                if (_currentAccount.TypeAccountEx == 1) TypeAccountExIn = TypeAccountEx.IsNormal;
                if (_currentAccount.TypeAccountEx == 2) TypeAccountExIn = TypeAccountEx.IsBujest;
                if (_currentAccount.TypeAccountEx == 3) TypeAccountExIn = TypeAccountEx.IsOwn;
                CurrentAllAnaliticalAccount =
                    AllAnaliticalAccount.FirstOrDefault(e => e.Id == value.AnaliticalNum);
                OnPropertyChanged("NameMain");
                OnPropertyChanged("NameMainEng");
                OnPropertyChanged("Id");
                OnPropertyChanged("CurrLevelAccout");
                OnPropertyChanged("FirmaId");
                OnPropertyChanged("SubNum");
                OnPropertyChanged("AnaliticalNum");
                OnPropertyChanged("CurrentAccount");
                OnPropertyChanged("TypeSaldoIn");
                OnPropertyChanged("LevelAccountIn");
                OnPropertyChanged("TypeAccountEnumIn");
                OnPropertyChanged("TypeAccountExIn");
                OnPropertyChanged("IsValid");
                OnPropertyChanged("Saldo");
                OnPropertyChanged("SaldoDebit");

            }
        }

        public string NameMain
        {
            get { if (_currentAccount != null) return _currentAccount.NameMain; return ""; }
            set
            {
                _currentAccount.NameMain = value;
                OnPropertyChanged("NameMain");
                OnPropertyChanged("IsValid");
            }
        }

        public string NameMainEng
        {
            get { if (_currentAccount != null) return _currentAccount.NameMainEng;
                return "";
            }
            set
            {
                _currentAccount.NameMainEng = value;
                OnPropertyChanged("NameMainEng");
            }
        }

        public int Id
        {
            get { if (_currentAccount != null) return _currentAccount.Id;
                return 0;
            }
            set
            {
                _currentAccount.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public int CurrLevelAccout
        {
            get { if (_currentAccount != null) return _currentAccount.LevelAccount;
                return 0;
            }
            set
            {
                _currentAccount.LevelAccount = value;
                OnPropertyChanged("CurrLevelAccout");
            }
        }

        public int FirmaId
        {
            get { if (_currentAccount != null) return _currentAccount.FirmaId;
                return 0;
            }
            set
            {
                _currentAccount.FirmaId = value;
                OnPropertyChanged("FirmaId");
            }
        }

        public int PartidNum
        {
            get { if (_currentAccount != null) return _currentAccount.PartidNum;
                return 0;
            }
            set
            {
                _currentAccount.PartidNum = value;
                OnPropertyChanged("PartidNum");
            }
        }

        public int SubNum
        {
            get { if (_currentAccount != null) return _currentAccount.SubNum;
                return 0;
            }
            set
            {
                _currentAccount.SubNum = value;
                OnPropertyChanged("SubNum");
            }
        }

        public int Num
        {
            get { if (_currentAccount != null) return _currentAccount.Num;
                return 0;
            }
            set
            {
                _currentAccount.Num = value;
                OnPropertyChanged("Num");
                OnPropertyChanged("IsValid");
            }
        }

        public bool EnableSaldo
        {
            get
            {
                return !IsAnalitic;
            }
        }

        public decimal SaldoDebit
        {
            get { if (_currentAccount != null) return _currentAccount.SaldoDV;
                return 0;
            }
            set
            {
                _currentAccount.SaldoDV = value;
                OnPropertyChanged("SaldoDebit");
            }
        }
         public decimal Saldo
        {
            get { return _currentAccount.SaldoKL; }
            set
            {
                if (_currentAccount != null) _currentAccount.SaldoKL = value;
                OnPropertyChanged("Saldo");
            }
        }
         public decimal SubSaldoDebit
         {
             get { if (_currentAccount != null) return _currentAccount.SubSaldoDL;
                 return 0;
             }
             set
             {
                 _currentAccount.SubSaldoDL = value;
                 OnPropertyChanged("SubSaldoDebit");
                 OnPropertyChanged("TotalSaldoDebit");
             }
         }
         public decimal SubSaldo
         {
             get { if (_currentAccount != null) return _currentAccount.SubSaldoKL;
                 return 0;
             }
             set
             {
                 _currentAccount.SubSaldoKL = value;
                 OnPropertyChanged("SubSaldo");
                 OnPropertyChanged("TotalSaldo");
             }
         }
        public decimal TotalSaldoDebit
         {
             get { if (_currentAccount != null) return _currentAccount.TotalSaldoDV;
                 return 0;
             }
         }
         public decimal TotalSaldo
         {
             get { if (_currentAccount != null) return _currentAccount.TotalSaldoKL;
                 return 0;
             }
         }
        private AnaliticalAccount _currentAllAnaliticalAccount;

        public AnaliticalAccount CurrentAllAnaliticalAccount
        {
            get { return _currentAllAnaliticalAccount; }
            set
            {
                _currentAllAnaliticalAccount = value;
                if (value != null)
                {
                    _currentAccount.AnaliticalNum = value.Id;
                    if (value.Name.ToUpper().Contains("ВАЛУТ")) ValutaVisible = true;
                    if (value.Name.ToUpper().Contains("МАТЕРИАЛИ")) ColVisible = true;
                    SelectedConnectors =
                        new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                            AllConnectors.Where(e => e.AnaliticalNameID == value.Id));
                    SelectedAnaliticalFields.Clear();
                    foreach (var curr in SelectedConnectors)
                    {
                        var addfield = AllAnaliticalFields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                        if (addfield != null)
                        {
                            addfield.Requared = curr.Required;
                            if (addfield != null) SelectedAnaliticalFields.Add(addfield);
                        }
                    }
                    Context.LoadMapToLookUps(SelectedAnaliticalFields, _currentAccount.Id, _currentAccount.AnaliticalNum);
                    CurrentAllTypeAccount = AllAnaliticTypes.Where(e => e.Id == value.TypeID).FirstOrDefault();
                    SelectedConnectors =
                        new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                            AlaMapToType.Where(e => e.AnaliticalFieldId == value.TypeID));
                    SelectedAnaliticalTypeFields.Clear();
                    foreach (var curr in SelectedConnectors)
                    {
                        var addfield = AllAnaliticalFields.FirstOrDefault(e => e.Id == curr.AnaliticalNameID);
                        if (addfield != null)
                        {
                            addfield.Requared = curr.Required;
                            SelectedAnaliticalTypeFields.Add(addfield);
                        }
                    }
                }
                OnPropertyChanged("SelectedAnaliticalFields");
                OnPropertyChanged("CurrentAllAnaliticalAccount");
                OnPropertyChanged("SelectedAnaliticalTypeFields");
                OnPropertyChanged("CurrentAllTypeAccount");
            }

        }

        private AnaliticalAccountType _currentAllTypeAccount;

        public AnaliticalAccountType CurrentAllTypeAccount
        {
            get { return _currentAllTypeAccount; }
            set
            {
                _currentAllTypeAccount = value;
                if (value != null)
                {

                    SelectedConnectors =
                        new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                            AlaMapToType.Where(e => e.AnaliticalFieldId == value.Id));
                    SelectedAnaliticalTypeFields.Clear();
                    foreach (var curr in SelectedConnectors)
                    {
                        var addfield = AllAnaliticalFields.FirstOrDefault(e => e.Id == curr.AnaliticalNameID);
                        if (addfield != null) SelectedAnaliticalTypeFields.Add(addfield);
                    }

                }
                OnPropertyChanged("SelectedAnaliticalTypeFields");
                OnPropertyChanged("CurrentAllTypeAccount");
            }

        }

        private AnaliticalFields _currentSelectedAnaliticalField;

        public AnaliticalFields CurrentSelectedAnaliticalField
        {
            get { return _currentSelectedAnaliticalField; }
            set
            {
                if (value == null) return;
                _currentSelectedAnaliticalField = value;
                _currentSelectedAnaliticalField.NameAcc = _currentAccount.NameMain;
                OnPropertyChanged("CurrentSelectedAnaliticalField");
            }
        }

        private AnaliticalAccountType _typeAccount;

        public AnaliticalAccountType TypeAccount
        {
            get { return _typeAccount; }
            set
            {
                _typeAccount = value;
                OnPropertyChanged("TypeAccount");
            }
        }


        public bool ClearConection()
        {
            if (CurrentSelectedAnaliticalField != null)
            {
               
                    CurrentSelectedAnaliticalField.NameFieldLookUp = "";
                    CurrentSelectedAnaliticalField.NameLookUp = "";
                    CurrentSelectedAnaliticalField.IdField = 0;
                    CurrentSelectedAnaliticalField.IdLookUp = 0;
                    //context.DeleteMovement(this._currentAccount.Id, 0);
                    OnPropertyChanged("CurrentSelectedAnaliticalField");
                    Save();
                    return true;
                
                
            }
            return false;
        }

        public string Title
        {
            get
            {
                string result = "";
                if (this.Mode == EditMode.Add)
                {
                    result="Добавяне";
                    if (SaveAndClose) result += " и избор ";
                    if (CurrentAccount != null && CurrentAccount.Num > 0)
                    {
                        result += " на подсметка";
                    }
                    else
                    {
                        result += " на сметка";
                    }
               
                }
                else
                {
                    result ="Редактиране";
                    if (CurrentAccount != null && CurrentAccount.SubNum > 0)
                    {
                        result += " на подсметка";
                    }
                    else
                    {
                        result += " на сметка";
                    }
               
                }
                return result;

            }

        }

        private bool _isAnalitic;
        public bool IsAnalitic
        {
            get { return _isAnalitic; }
            set
            {
                _isAnalitic=value;
                if (_isAnalitic)
                {
                    _LevelAccountIn=LevelAccout.IsAnalitic;
                    
                }
                else
                {
                    _LevelAccountIn=LevelAccout.IsSintetic;
          
                }
                OnPropertyChanged("IsAnalitic");
                OnPropertyChanged("EnableSaldo");
                OnPropertyChanged("LevelAccountIn");
                
            }
        }

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        protected override bool CanSave()
        {
            return IsValid;
        }
        static readonly string[] ValidatedProperties = 
        { 
            "Num", 
            "NameMain"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Num":
                    if(MainValidators.IsValidRegex(Num.ToString(),"[0-9]{10}$") || Num<0)
                    {
                        error = "Не сте избрали сметка от националния сметкоплан";
                    }
                    break;

                case "NameMain":
                    error = this.ValidateNameMain();
                    break;

               
            }

            return error;
        }



        string ValidateNameMain()
        {
            if (MainValidators.IsStringMissing(this.NameMain))
            {
                return "Задължително поле: Име на сметка";
            }

            return null;
        }

      


        #endregion // Validation


        public string Error
        {
            get { return null; }
        }

        private bool _isMain;
        public bool IsMain
        {
            get { return _isMain; }
            set
            {
                _isMain = value;
                OnPropertyChanged("IsMain");
            }
        }

        public string this[string columnName]
        {
            get { return this.GetValidationError(columnName); }
        }

        public void RefreshProperty(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }


        private bool _valutaVisible;
        public bool ValutaVisible
        {
            get { return _valutaVisible; }
            set { _valutaVisible = value; OnPropertyChanged("ValutaVisible"); }
        }

        private bool _colVisible;
        private bool issave;
        private string _parentNum;
        private string _txt;

        public bool ColVisible
        {
            get { return _colVisible; }
            set { _colVisible = value; OnPropertyChanged("ColVisible"); }
        }

        public bool SaveOut()
        {
            Save();
           
            return issave;
        }

        public string ParentNum
        {
            get { return _parentNum; }
            set { _parentNum = value; OnPropertyChanged("ParentNum"); }
        }

        public bool SaveAndClose { get; set; }

        public bool NoAcc { get; set; }
    }
    
}