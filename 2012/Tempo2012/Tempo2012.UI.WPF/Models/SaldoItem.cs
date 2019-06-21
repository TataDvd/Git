using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Models
{
    [Serializable]
    public class SaldoItem : BaseViewModel 
    {
        public delegate void ChangeKindCurrency(object sender, ChangeKindCurrencyArg e);
        public event ChangeKindCurrency ChangedKindCurrency;

        private void OnChangeKindCurrency(string kc)
        {
            if (ChangedKindCurrency != null)
            {
                ChangedKindCurrency.Invoke(this,new ChangeKindCurrencyArg(kc));
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged("IsValid");
            }
        }
        private bool _isVal;
        public bool IsVal
        {
            get { return _isVal; }
            set
            {
                _isVal = value;
                OnPropertyChanged("IsVal");
            }
        }
        private bool _isKol;
        public bool IsKol
        {
            get { return _isKol; }
            set
            {
                _isKol = value;
                OnPropertyChanged("IsKol");
            }
        }
        private bool _isValidd;
        public bool IsValidd
        {
            get { return _isValidd; }
            set
            {
                _isValidd = value;
                OnPropertyChanged("IsValidd");
            }
        }
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set 
            {
                key = value;
                OnPropertyChanged("Key");
            }

        }

        private int _fieldkey;
        public int Fieldkey
        {
            get { return _fieldkey; }
            set { _fieldkey = value; }
        }

        public bool IsInUnigroup { get; set;}
        public bool IsDK { get; set; }
        public bool IsLookUp { get; set;}
        public int Length { get; set;}
        public int Relookup { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value;OnPropertyChanged("Name"); }
        }

        public int Group { get; set;}
        private SaldoItemTypes _type;
        public SaldoItemTypes Type 
        {
            get
            {
                return _type;
            }
            set 
            {
                _type = value;
 
            }

        }
        private int _valueInt;
        public int ValueInt
        {
            get { return _valueInt; }
            set
            {
                _valueInt = value;
                _value = value.ToString();
                OnPropertyChanged("ValueInt");
                OnPropertyChanged("Value");
            }
        }

        private decimal _valuedecimal;
        public decimal Valuedecimal
        {
            get { return _valuedecimal; }
            set
            {
                _valuedecimal = value;
                _value = value.ToString();
                OnPropertyChanged("Valuedecimal");
                OnPropertyChanged("Value");
            }
        }

        private DateTime _valueDate;
        public DateTime ValueDate
        {
            get { return _valueDate; }
            set
            {
                _valueDate = value;
                _value = value.ToShortDateString();
                OnPropertyChanged("ValueDate");
                OnPropertyChanged("Value");
            }
        }

        
        public override string ToString()
        {
            return string.Format("{0}",Name);
        }
        private List<SaldoItem> _LookUp = new List<SaldoItem>();
        public List<SaldoItem> LookUp 
        { 
            get 
            {
                return _LookUp;
            } 
            set 
            {
                _LookUp = value; OnPropertyChanged("LookUp");
            }
        }
        public List<string> LookUpInteli
        {
            get {
                return LookUp.Select(saldoItem => string.Format("{0}|{1}|{2}", saldoItem.Key, saldoItem.Bulstad, saldoItem.Value)).ToList();
            }
        }

        private string _bulstad;
        public string Bulstad
        {
            get { return _bulstad; }
            set { _bulstad = value; OnPropertyChanged("Bulstad");}
        }

        private string _vat;
        public string Vat
        {
            get { return _vat; }
            set { _vat = value; OnPropertyChanged("Vat"); }
        }
        private string _value;
        public string Value
        {
            get
            {
                if (Type==SaldoItemTypes.Date)
                {
                    if (string.IsNullOrWhiteSpace(_value)||(_value=="0"))
                    {
                        _value = DateTime.Now.ToShortDateString();
                    }
                }
                return _value;
            }
            set
            {
                switch (Type)
                {
                    case SaldoItemTypes.Currency:
                        {
                            decimal valuedecimal;
                            var ns = System.Globalization.NumberStyles.AllowDecimalPoint |
                     System.Globalization.NumberStyles.AllowLeadingSign;
                            IsValidd = decimal.TryParse(value, ns, Thread.CurrentThread.CurrentCulture,
                                                        out valuedecimal);
                            if (IsValid) { 
                                Valuedecimal = valuedecimal;
                                
                            }
                            break;
                        }
                    case SaldoItemTypes.Integer:
                        {
                            int valueint=0;
                            IsValid=int.TryParse(value,out valueint);
                            if (IsValid) ValueInt = valueint;
                            break;
                        }
                    case SaldoItemTypes.Date:
                        {
                            DateTime valuedate = DateTime.Now;
                            IsValid = DateTime.TryParse(value, out valuedate);
                            if (IsValid)
                            {
                                ValueDate = valuedate;
                            }
                            else
                            {
                                if (value.Contains("/"))
                                {
                                    var split = value.Split('/');
                                    if (split.Count() == 2)
                                    {
                                        ValueDate = new DateTime(int.Parse(split[2]), int.Parse(split[0]), int.Parse(split[1]));
                                    }
                                }
                                else
                                ValueDate = ConfigTempoSinglenton.GetInstance().WorkDate;
                            }
                            break;
                        }
                    case SaldoItemTypes.String:
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                IsValid = true;
                            }
                            else
                            {
                                IsValid = false;
                            }
                            break;
                        }

                }
                _value = Type==SaldoItemTypes.Date ? ValueDate.ToShortDateString() : value;
                if (IsDK)
                {
                    _valued = Vf.LevFormatUI;
                    OnPropertyChanged("Valued");
                }
                OnPropertyChanged("Value");
                if (Name == "Вид валута")
                {
                    OnChangeKindCurrency(value);
                }
            }
        }

        private string _valued;
        
        public string Valued
        {
            get { return _valued; }
            set
            {
                switch (Type)
                {
                    case SaldoItemTypes.Currency:
                        {
                            decimal valuedecimal;
                            var ns = System.Globalization.NumberStyles.AllowDecimalPoint |
                     System.Globalization.NumberStyles.AllowLeadingSign;
                            IsValidd = decimal.TryParse(value, ns, Thread.CurrentThread.CurrentCulture,
                                                        out valuedecimal);
                            if (IsValid) Valuedecimald = valuedecimal;
                            break;
                        }
                    case SaldoItemTypes.Integer:
                        {
                            int valueint = 0;
                            IsValidd = int.TryParse(value, out valueint);
                            if (IsValid) ValueIntd = valueint;
                            break;
                        }
                    case SaldoItemTypes.Date:
                        {
                            DateTime valuedate = DateTime.Now;
                            IsValidd = DateTime.TryParse(value, out valuedate);
                            if (IsValid) ValueDated = valuedate;
                            break;
                        }
                    case SaldoItemTypes.String:
                        {
                            IsValidd = true;
                            break;
                        }

                }
                _valued = value;
                if (IsDK)
                {
                    _value = Vf.LevFormatUI;
                    OnPropertyChanged("Value");
                }
                OnPropertyChanged("Valued");
               
            }
        }

        private decimal _valueDebit;
        public decimal ValueDebit
        {
            get { return _valueDebit; }
            set
            {
                _valueDebit = value;
                if (value > 0)
                {
                    ValueCredit = 0;
                }
                OnPropertyChanged("ValueDebit");
            }
        }

        private decimal _valueCredit;
        public decimal ValueCredit
        {
            get { return _valueCredit; }
            set
            {
                _valueCredit = value;
                if (value > 0)
                {
                    ValueDebit = 0;
                }
                OnPropertyChanged("ValueCredit");
            }
        }
        public decimal Valuedecimald { get; set; }

        public int ValueIntd { get; set; }

        public DateTime ValueDated { get; set; }

        public string RTableName { get; set; }

        public int RCODELOOKUP { get; set; }

        private SaldoItem _selectedLookupItem;
        public SaldoItem SelectedLookupItem
        {
            get { return _selectedLookupItem; }
            set
            {
                _selectedLookupItem = value;
                if(_selectedLookupItem!=null)
                {
                    Lookupval = _selectedLookupItem.Value;
                    //LookUpInteliText = SelectedLookupItem.Key + " " + SelectedLookupItem.Bulstad + " " + SelectedLookupItem.Value;
                }
                OnPropertyChanged("SelectedLookupItem");
            }
        }

        private bool _isK;
        public bool IsK
        {
            get { return _isK;}
            set { _isK = value; OnPropertyChanged("IsK");}
        }

        private bool _isD;
        public bool IsD
        {
            get { return _isD; }
            set { _isD = value;OnPropertyChanged("IsD");}
        }

        public Visibility IsShowNew
        {
            get { return _isShowNew; }
            set { _isShowNew = value; OnPropertyChanged("IsShowNew"); }
        }

        public long Id
        {
            get; set;
        }

        private string _infoTitle;
        public string InfoTitle
        {
            get { return _infoTitle; }
            set { _infoTitle = value;OnPropertyChanged("InfoTitle"); }
        }
        
        private decimal _infoValue;
        public decimal InfoValue
        {
            get { return _infoValue; }
            set { _infoValue = value;OnPropertyChanged("InfoValue");}
        }

        private bool _isKurs;
        public bool IsKurs
        {
            get { return _isKurs; }
            set { _isKurs = value;OnPropertyChanged("IsKurs"); }
        }

        private decimal _valueVal;
        public decimal ValueVal
        {
            get { return _valueVal;
               
            }
            set { _valueVal = value;
                  //SumaLeva = ValueVal*ValueKurs;
                  OnPropertyChanged("ValueVal"); }
        }

        private decimal _valueKurs;
        public decimal ValueKurs
        {
            get { return _valueKurs; }
            set { _valueKurs = value;
                //SumaLeva = ValueVal*ValueKurs;
                //KursDif = (ValueKurs - MainKurs)*ValueVal;
                OnPropertyChanged("ValueKurs"); }
        }
        private decimal _kursDif;
        public decimal KursDif
        {
            get { return _kursDif; }
            set
            {
                _kursDif = value;
                OnPropertyChanged("KursDif");
            }
        }

        private decimal _mainKurs;
        public decimal MainKurs
        {
            get { return _mainKurs; }
            set { _mainKurs = value;
            //KursDif = (ValueKurs - MainKurs) * ValueVal;
                OnPropertyChanged("MainKurs"); }
        }

        public decimal ValueKol
        {
            get { return _valueKol; }
            set { _valueKol = value;OnPropertyChanged("ValueKol"); }
        }

        public decimal OnePrice
        {
            get { return _onePrice; }
            set { _onePrice = value;OnPropertyChanged("OnePrice"); }
        }

        public decimal SumaLeva;

        public int TabIndex { get; set;}

        public string Vals { get; set; }

        private string _lookupval;
        public string Lookupval
        {
            get { return _lookupval; }
            set { _lookupval = value; OnPropertyChanged("LOOKUPVAL");}
        }

        
        public IEnumerable<Filter> GetFilters()
        {
            List<Filter> res = new List<Filter>();
            LookupModel lm = SysLookup ? Context.GetSysLookup(Relookup) : Context.GetLookup(Relookup);
            SerachedField=Entrence.ConfigFirmaToLookup.GetField(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,lm.LookUpMetaData.Id);
            foreach (var item in lm.Fields.Skip(1))
            {
                res.Add(new Filter{FilterField = item.NameEng,FilterName = item.Name});
            }
            return res.OrderBy(e => e.FilterField);
        }

        
        public ObservableCollection<ObservableCollection<string>> GetDictionary(string p,string orderby)
        {
            ObservableCollection<ObservableCollection<string>> res = new ObservableCollection<ObservableCollection<string>>();
            LookupModel lm =SysLookup?Context.GetSysLookup(Relookup): Context.GetLookup(Relookup);
            if (lm.LookUpMetaData == null) return new ObservableCollection<ObservableCollection<string>>();
            ObservableCollection<string> title = new ObservableCollection<string>();
            foreach (var item in lm.Fields.Skip(1))
            {
                title.Add(item.Name);
            }
            res.Add(title);
            string sqlfilter = p;
            var rez =SysLookup?Context.GetSysLookupDictionary(lm.LookUpMetaData.Tablename,sqlfilter,"First 20",orderby):  Context.GetLookupDictionary(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, sqlfilter, " FIRST 20 ",orderby);
            _itemsKeys=new List<int>();
            foreach (Dictionary<string, object> dictionary in rez)
            {
                title = new ObservableCollection<string>(lm.Fields.Select(item => dictionary[item.NameEng].ToString()).ToList());
                _itemsKeys.Add(int.Parse(title[0]));
                res.Add(new ObservableCollection<string>(title.Skip(1)));
            }
            return res;
        }


        private List<int> _itemsKeys;
        private decimal _valueKol;
        private decimal _onePrice;
        private Visibility _isShowNew= Visibility.Visible;

        public int GetLookUpId(int id)
        {
            if (_itemsKeys!=null && id < _itemsKeys.Count)
            {
                return _itemsKeys[id];
            }
            return 0;
        }

        public int LiD { get; set; }

        public string SerachedField { get; set; }

        public bool SysLookup { get; set; }
    }
}
