using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels.Saldos
{
    public class EditorAddSaldosViewModel : BaseViewModel
    {
        public bool IsEdit { get; set;}
        public DateTime MinDate
        {
            get { return new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 1, 1); }

        }
        public DateTime MaxDate
        {
            get { return new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 1, 1).AddYears(1).AddDays(-1); }

        }
        public EditorAddSaldosViewModel(AccountsModel acc, int groupid, long typeAnaliticalKey)
        {
            IsEdit = true;
            Title = string.Format("Редактиране на салдо по сметка {0}",acc);
            this.AccID = acc.Id;
            this.Acc = acc;
            this.GroupId = groupid;
            _items=new ObservableCollection<SaldoItem>();
             this.typeAnaliticalKey = typeAnaliticalKey;
            List<SaldoAnaliticModel> fields = new List<SaldoAnaliticModel>(Context.GetCurrentMovements(acc.Id,groupid));
            int tabindex = 1;
            foreach (SaldoAnaliticModel analiticalFields in fields)
            {
                //Titles.Add(analiticalFields.Name);
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.DBField == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                   
                }
                if (analiticalFields.DBField.Contains("DECIMAL"))
                {
                    saldotype = SaldoItemTypes.Currency;
                   
                }
                if (analiticalFields.DBField == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                    
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Value = analiticalFields.VAL + analiticalFields.VALS,
                    Valued = analiticalFields.VALUED.ToString(),
                    Fieldkey = analiticalFields.ACCFIELDKEY,
                    IsInUnigroup = analiticalFields.Required,
                    TabIndex=tabindex,
                    Vals = analiticalFields.VALS
                };
                tabindex++;
                    if (analiticalFields.ACCFIELDKEY == 30)
                    {
                        saldoItem.ValueCredit = analiticalFields.VALVALK;
                        saldoItem.ValueDebit = analiticalFields.VALVALD;
                        saldoItem.IsVal=true;
                        saldoItem.IsDK = true;
                    }
                    if (analiticalFields.ACCFIELDKEY == 31)
                    {
                        saldoItem.ValueCredit=analiticalFields.VALKOLK;
                        saldoItem.ValueDebit = analiticalFields.VALKOLD;
                        saldoItem.IsKol=true;
                        saldoItem.IsDK = true;
                    }
                    if (analiticalFields.ACCFIELDKEY == 29)
                    {
                        saldoItem.ValueCredit = analiticalFields.VALUEMONEY;
                        saldoItem.ValueDebit = analiticalFields.VALUED;
                    saldoItem.IsDK = true;
                    }
                
                //if (analiticalFields.LOOKUPID != 0)
                //{
                //    saldoItem.Key = analiticalFields.LOOKUPFIELDKEY.ToString();
                //    saldoItem.IsLookUp = true;
                //    saldoItem.Relookup = analiticalFields.LOOKUPID;
                //    LookupModel lm = context.GetLookup(analiticalFields.LOOKUPID);
                //    var list = context.GetLookup(lm.LookUpMetaData.Tablename);
                //    int k = 0;
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();

                //        foreach (string s in enumerable)
                //        {

                //            if (i == 2) saldoitem.Value = s;
                //            if (i == 1) saldoitem.Key = s;
                //            if (k == 0 && i == 1)
                //            {
                //                saldoItem.Key = s;
                //                k++;
                //            }
                //            if (k == 1 && i == 2)
                //            {
                //                saldoItem.Value = s;
                //                k++;
                //            }
                //            i++;
                //        }
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
                _items.Add(saldoItem);
            }

        }

        public int GroupId
        {
            get;
            set;
        }

        public EditorAddSaldosViewModel(AccountsModel acc, IEnumerable<SaldoItem> saldoItems,bool keepval)
        {
            IsEdit = false;
            Title = string.Format("Добавяне на салдо по сметка {0}",acc);
            if (saldoItems == null)
            {
                saldoItems = this.LoadAccFieldsMetaData(acc);
            }
            if (!keepval)
            {
                foreach (var item in saldoItems)
                {

                    if (item.IsDK)
                    {
                        item.Value = Vf.LevFormatUI;
                        item.Valued = Vf.LevFormatUI;
                    }
                    else
                    {
                        item.Value = "";
                        item.Valued = "";
                    }
                }
            }
            _items = new ObservableCollection<SaldoItem>(saldoItems);
            this.Acc = acc;
            this.AccID = acc.Id;
        }

        private  IEnumerable<SaldoItem> LoadAccFieldsMetaData(AccountsModel acc)
        {
            AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            AllAnaliticalAccount=new ObservableCollection<AnaliticalAccount>(Context.GetAllAnaliticalAccount());
            AllAnaliticalFields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            AllConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
            AlaMapToType = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            SelectedAnaliticalFields = new ObservableCollection<AnaliticalFields>();
            SelectedAnaliticalTypeFields = new ObservableCollection<AnaliticalFields>();
           
            SelectedAnaliticalFields.Clear();
            var CurrentAllAnaliticalAccount = AllAnaliticalAccount.FirstOrDefault(e => e.Id ==acc.AnaliticalNum);
            //AnaliticalAccountType CurrentAllTypeAccount = AllAnaliticTypes.FirstOrDefault(e => e.Id == CurrentAllAnaliticalAccount.Id);
            SelectedConnectors =
                       new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                           AllConnectors.Where(e => CurrentAllAnaliticalAccount != null && e.AnaliticalNameID == CurrentAllAnaliticalAccount.Id));
            foreach (var curr in SelectedConnectors.OrderBy(e=>e.SortOrder))
            {
                var addfield = AllAnaliticalFields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                if (addfield != null)
                {
                    addfield.Requared = curr.Required;
                    if (addfield != null) SelectedAnaliticalFields.Add(addfield);
                }
            }
            Context.LoadMapToLookUps(SelectedAnaliticalFields, acc.Id, acc.AnaliticalNum);
            SelectedConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                    AlaMapToType.Where(e => e.AnaliticalFieldId == CurrentAllAnaliticalAccount.TypeID));
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
            ObservableCollection<AnaliticalFields> sFieldses = new ObservableCollection<AnaliticalFields>();
            CurrentAllTypeAccount = AllAnaliticTypes.FirstOrDefault(e => e.Id == CurrentAllAnaliticalAccount.TypeID);
            if (CurrentAllTypeAccount!=null)
            {
                        if (CurrentAllTypeAccount.Sl)
                        {
                            sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума лв."));
                        }
                        if (CurrentAllTypeAccount.Sv)
                        {
                            sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума валута"));
                        }
                        if (CurrentAllTypeAccount.Kol)
                        {
                            sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Количество"));
                        }
                       
                    }
                    else
                    {
                         sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума лв."));
                       
                    }
            Titles = new ObservableCollection<string>();
            Contents = new ObservableCollection<ObservableCollection<string>>();
            ObservableCollection<string> vals = new ObservableCollection<string>();
            ObservableCollection<SaldoItem> _saldoItems = new ObservableCollection<SaldoItem>();
            int tabitem = 1;
            foreach (AnaliticalFields analiticalFields in sFieldses)
            {
                Titles.Add(analiticalFields.Name + "Кредит");
                Titles.Add(analiticalFields.Name + "Дебит");

                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                }
                if (analiticalFields.FieldType.Contains("DECIMAL"))
                {
                    saldotype = SaldoItemTypes.Currency;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Length = 50,
                    Value = Vf.LevFormatUI,
                    Valued = Vf.LevFormatUI,
                    IsDK = true,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    TabIndex =tabitem
                };
                tabitem++;
                if (analiticalFields.Name == "Количество") saldoItem.IsKol = true;
                if (analiticalFields.Name == "Сума валута") saldoItem.IsVal = true;
                if (analiticalFields.Name == "Валутен курс") saldoItem.IsKurs = true;
                _saldoItems.Add(saldoItem);

            }
            foreach (AnaliticalFields analiticalFields in SelectedAnaliticalFields)
            {
                Titles.Add(analiticalFields.Name);
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                string defvalue = "";
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                    defvalue = "0";
                }
                if (analiticalFields.FieldType == "decimal")
                {
                    saldotype = SaldoItemTypes.Currency;
                    defvalue = Vf.LevFormatUI;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                    defvalue = DateTime.Now.ToShortDateString();
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Value = defvalue,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    TabIndex = tabitem
                };
                tabitem++;
                if (analiticalFields.IdLookUp != 0)
                {
                    saldoItem.IsLookUp = true;
                    saldoItem.Relookup = analiticalFields.IdLookUp;
                }
                //    LookupModel lm = Context.GetLookup(analiticalFields.IdLookUp);
                //    var list = Context.GetLookup(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    int k = 0;
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();

                //        foreach (string s in enumerable)
                //        {

                //            if (i == 2) saldoitem.Value = s;
                //            if (i == 1) saldoitem.Key = s;
                //            if (k == 0 && i == 1)
                //            {
                //                saldoItem.Key = s;
                //                k++;
                //            }
                //            if (k == 1 && i == 2)
                //            {
                //                saldoItem.Value = s;
                //                k++;
                //            }
                //            i++;
                //        }
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
                _saldoItems.Add(saldoItem);

            }
            foreach (AnaliticalFields analiticalFields in SelectedAnaliticalTypeFields)
            {
                Titles.Add(analiticalFields.Name);
                string defvalue = "";
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                    defvalue = "0";
                }
                if (analiticalFields.FieldType == "decimal")
                {
                    saldotype = SaldoItemTypes.Currency;
                    defvalue = Vf.LevFormatUI;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                    defvalue = DateTime.Now.ToShortDateString();
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Value = defvalue,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    TabIndex = tabitem
                };
                tabitem++;
                if (analiticalFields.IdLookUp != 0)
                {
                    saldoItem.IsLookUp = true;
                    saldoItem.Relookup = analiticalFields.IdLookUp;
                }
                //    LookupModel lm = Context.GetLookup(analiticalFields.IdLookUp);
                //    var list = Context.GetLookup(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();
                //        foreach (string s in enumerable)
                //        {

                //            if (i == 2) saldoitem.Value = s;
                //            if (i == 1) saldoitem.Key = s;

                //            i++;
                //        }
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
                //if (!string.IsNullOrWhiteSpace(analiticalFields.RTABLENAME))
                //{

                //    saldoItem.IsLookUp = true;
                //    saldoItem.RCODELOOKUP = analiticalFields.RCODELOOKUP;
                //    var list = Context.GetLookup(analiticalFields.RTABLENAME, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    int k = 0;
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();

                //        foreach (string s in enumerable)
                //        {

                //            if (i == 2) saldoitem.Value = s;
                //            if (i == 1) saldoitem.Key = s;
                //            if (k == 0 && i == 1)
                //            {
                //                saldoItem.Key = s;
                //                k++;
                //            }
                //            if (k == 1 && i == 2)
                //            {
                //                saldoItem.Value = s;
                //                k++;
                //            }
                //            i++;
                //        }
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}

                _saldoItems.Add(saldoItem);
            }
            return _saldoItems;
        }

        public ObservableCollection<AnaliticalAccount> AllAnaliticalAccount { get; set; }


        private ObservableCollection<SaldoItem> _items;

        public ObservableCollection<SaldoItem> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        private SaldoItem _currentItem;

        public SaldoItem CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                OnPropertyChanged("CurrentItem");
            }
        }

        private string _title;
        private  long typeAnaliticalKey;
     
      

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        protected override bool CanSave()
        {
            return IsValid;
        }
        public static readonly string[] ValidatedProperties =
            {
                "Items"
                
            };


        public string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                
                case "Items":
                    error = ValidateItems();
                    break;
                
            }

            return error;
        }
        private string ValidateItems()
        {
            return (from saldoItem in Items where string.IsNullOrWhiteSpace(saldoItem.Value) where !saldoItem.IsVal where !saldoItem.IsKol select "Невалидна стойност на поле " + saldoItem.Name).FirstOrDefault();
        }
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
        protected override void Save()
        {
            int sortorder = 0;
                foreach (var currentsaldos in Items)
                {
                    int key = 0;
                    if (!int.TryParse(currentsaldos.Key,out key))
                    {
                        key = 0;
                    }
                    var saldo =new SaldoAnaliticModel();
                    saldo.ACCFIELDKEY = currentsaldos.Fieldkey;
                    saldo.ACCID = AccID;
                    saldo.DATA = DateTime.Now;
                    saldo.SORTORDER = sortorder;
                    sortorder++;
                    int test;
                    if (int.TryParse(currentsaldos.Key, out test))
                    {
                        saldo.LOOKUPFIELDKEY = test;
                    }
                    saldo.TYPEACCKEY = typeAnaliticalKey;
                    saldo.VALUEDATE = currentsaldos.ValueDate;
                    if (currentsaldos.Type==SaldoItemTypes.Date)
                    {
                        if (string.IsNullOrWhiteSpace(currentsaldos.Value))
                        {
                            currentsaldos.Value = currentsaldos.ValueDate.ToShortDateString();
                        }
                    }
                    saldo.VAL = currentsaldos.Value;
                    saldo.VALS= currentsaldos.Lookupval;
                    saldo.VALUENUM = currentsaldos.ValueInt;
                    
                            
                    saldo.GROUP = this.GroupId;
                    if (currentsaldos.IsDK)
                    {
                        saldo.VALKOLD = currentsaldos.Fieldkey == 31 ? currentsaldos.ValueDebit : 0;
                        saldo.VALKOLK = currentsaldos.Fieldkey == 31 ? currentsaldos.ValueCredit : 0;
                        saldo.VALVALK = currentsaldos.Fieldkey == 30 ? currentsaldos.ValueDebit : 0;
                        saldo.VALVALD = currentsaldos.Fieldkey == 30 ? currentsaldos.ValueCredit : 0;
                        saldo.VALUEMONEY = currentsaldos.Fieldkey == 29? currentsaldos.ValueCredit:0;
                        saldo.VALUED =  currentsaldos.Fieldkey == 29?currentsaldos.ValueDebit:0;
                        
                    }
                    saldo.LOOKUPID = currentsaldos.Relookup;
                    Context.SaveMovement(saldo);
                }
            
            
        }

        public bool CheckSaldoItem()
        {
            int uniqcount = 0;
            foreach (SaldoItem currentsaldos in Items)
            {
                if (currentsaldos.IsInUnigroup)
                {
                    uniqcount++;
                }
            }
            if (uniqcount == 0) return true;
            string sumalvk, sumalvd,sumalvksub, sumalvdsub;
            List<List<string>> rez = Context.GetAllMovements(Acc.Id,Acc.Num,Acc.FirmaId, out sumalvk, out sumalvd, out sumalvksub,out sumalvdsub);
            foreach (List<string> list in rez)
            {
               int i = 0;
               bool doobre = true;
                //var sortedItems =
                //    from w in Items
                //    orderby w.Group,w.Fieldkey,w.Id
                //    select w;

                foreach (SaldoItem currentsaldos in Items)
               {
                   
                   if (currentsaldos.IsInUnigroup)
                   {
                       if (currentsaldos.IsLookUp)
                       {
                           if (!list[i].Equals(currentsaldos.Key))
                           {
                               doobre = false;
                           }
                       }
                       else
                       {
                           if (!list[i].Equals(currentsaldos.Value))
                           {
                               doobre = false;
                           }
                       }

                   }
                   if ((currentsaldos.Fieldkey==29)||(currentsaldos.Fieldkey==30)||(currentsaldos.Fieldkey==31))
                   {
                       i+=2;
                   }
                   else
                   {
                       i++;
                   }
                   
               }
              if (doobre)
                   {
                       return false;
                   }
            }
            return  true;
        }

        protected override void Update()
        {
            foreach (var currentsaldos in Items)
            {
                if (!currentsaldos.IsDK) continue;
                var ni = new SaldoAnaliticModel
                             {
                                 ACCFIELDKEY = currentsaldos.Fieldkey,
                                 ACCID = AccID,
                                 DATA = DateTime.Now,
                                 LOOKUPFIELDKEY =
                                     string.IsNullOrWhiteSpace(currentsaldos.Key)
                                         ? 0
                                         : int.Parse(currentsaldos.Key),
                                 TYPEACCKEY = typeAnaliticalKey,
                                 VALUEDATE = currentsaldos.ValueDate,
                                 VAL = currentsaldos.Value,
                                 VALUEMONEY = 0,
                                 VALUENUM = currentsaldos.ValueInt,
                                 VALUED = 0,
                                 GROUP = this.GroupId,
                                 VALKOLD = 0,
                                 VALKOLK = 0,
                                 VALVALD = 0,
                                 VALVALK = 0,
                                 LOOKUPID = currentsaldos.Relookup,
                                 VALS = currentsaldos.Vals,
                             };
                if (currentsaldos.Fieldkey == 31)
                {
                    ni.VALKOLD = currentsaldos.ValueDebit;
                    ni.VALKOLK = currentsaldos.ValueCredit;

                }
                if (currentsaldos.Fieldkey == 30)
                {
                    ni.VALVALD = currentsaldos.ValueDebit;
                    ni.VALVALK = currentsaldos.ValueCredit;
                }
                if (currentsaldos.Fieldkey == 29)
                {
                    ni.VALUED = currentsaldos.ValueDebit;
                    ni.VALUEMONEY = currentsaldos.ValueCredit;
                }
                Context.UpdateMovement(ni);
            }
        }

        
        public int AccID { get; set; }

        public AccountsModel Acc { get; set; }

        public ObservableCollection<AnaliticalAccountType> AllAnaliticTypes { get; set; }

        public ObservableCollection<AnaliticalFields> AllAnaliticalFields { get; set; }

        public ObservableCollection<MapAnanaliticAccToAnaliticField> AllConnectors { get; set; }

        public ObservableCollection<MapAnanaliticAccToAnaliticField> AlaMapToType { get; set; }

        public ObservableCollection<AnaliticalFields> SelectedAnaliticalFields { get; set; }

        public ObservableCollection<AnaliticalFields> SelectedAnaliticalTypeFields { get; set; }

        public ObservableCollection<MapAnanaliticAccToAnaliticField> SelectedConnectors { get; set; }

        public AnaliticalAccountType CurrentAllTypeAccount { get; set; }

        public ObservableCollection<string> Titles { get; set; }

        public ObservableCollection<ObservableCollection<string>> Contents { get; set; }
    }
}