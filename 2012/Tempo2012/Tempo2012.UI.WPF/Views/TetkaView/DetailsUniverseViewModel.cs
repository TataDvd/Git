using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DataGrid2DLibrary;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    public class DetailsUniverseViewModel:BaseViewModel
    {
        public const int PAGECOUNT = 20;
        private List<List<string>> _fields;
        private ContoViewModel Cvm;
        private decimal _suma;
        private decimal _sumaval;
        private int Tip;
        private string _title;
        private int Count;
        public AccountsModel Acc;
        public ObservableCollection<Filter> Filters { get; set; }
        public DetailsUniverseViewModel(AccountsModel dAccountsModel,string filter,ContoViewModel cvm,int tip, EditMode mode)
        {
            Filters = new ObservableCollection<Filter>();
            Acc = dAccountsModel;
            if (mode == EditMode.Edit)
            {
                IsEditMode = System.Windows.Visibility.Hidden;
            }
            else
            {
                IsEditMode = System.Windows.Visibility.Visible;
            }
            Title = "Детайли за " + dAccountsModel.ShortName;
            Tip = tip;
            Cvm = cvm;
            
             List<List<string>> v = null;
             var c = Context.GetDetailsContoToAccUni(dAccountsModel.Id, dAccountsModel.TypeAccount, dAccountsModel.Kol, dAccountsModel.Val, filter);
             if (c!= null)v = c.Select(i => i.ToList()).ToList();
            
            
                _fields = new List<List<string>>();
                if (v != null)
                {
                    foreach (var item in v)
                    {
                        _fields.Add(new List<string>(item));
                    }
                }
                else
                {
                    var r = new List<string>();
                    var atr = Context.LoadAllAnaliticfields(dAccountsModel.Id);
                    foreach (SaldoAnaliticModel saldoAnaliticModel in atr)
                    {
                        r.Add(saldoAnaliticModel.Name);

                    }
                    r.Add("НС");
                    r.Add("ОД");
                    r.Add("ОК");
                    r.Add("КС");
                if (dAccountsModel.Val == 1)
                {
                    r.Add("НСВ");
                    r.Add("ОДВ");
                    r.Add("ОКВ");
                    r.Add("КСВ");
                }
                if (dAccountsModel.Kol == 1)
                    {
                        r.Add("НСК");
                        r.Add("ОДК");
                        r.Add("ОКК");
                        r.Add("КСК");
                    }
                    _fields.Add(r);
                }
            if (dAccountsModel.Kol == 1)
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 5] != Vf.KolFormatUI));
            }
            else if (dAccountsModel.Val == 1)
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 5] != Vf.ValFormatUI));
            }
            else
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 1] != Vf.LevFormatUI));
            }
                
            
           OnPropertyChanged("Fields");
        }

        internal void SaveContos(IList selectedItems)
        {
            decimal kurrazsum = 0;
            Cvm.notupdated=true;
            foreach (var item in selectedItems)
            {
               
                kurrazsum+=Math.Round(SaveConto(item),2);

            }
            var AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            if (kurrazsum < 0)
            {
                var model = AllAccountsK.FirstOrDefault(e => e.Num == 624 && e.SubNum == 0);
                if (model != null)
                {
                    Cvm.DAccountsModel = model;
                    Cvm.Oborot = kurrazsum;
                    foreach (SaldoItem saldoItem in Cvm.ItemsCredit)
                    {
                        if (saldoItem.Name == "Сума валута")
                        {
                            saldoItem.ValueVal = 0;
                            saldoItem.Value = "0";
                            saldoItem.ValueKurs = saldoItem.MainKurs;
                            saldoItem.KursDif = 0;
                        }

                    }
                    Cvm.SaveF4();
                }
            }
            else if (kurrazsum > 0)
            {
                var model = AllAccountsK.FirstOrDefault(e => e.Num == 724 && e.SubNum == 0);
                if (model != null)
                {
                    Cvm.DAccountsModel = Cvm.CAccountsModel;
                    Cvm.ItemsDebit = Cvm.ItemsCredit;
                    Cvm.CAccountsModel = model;
                    Cvm.Oborot = kurrazsum;
                    foreach (SaldoItem saldoItem in Cvm.ItemsDebit)
                    {
                        if (saldoItem.Name == "Сума валута")
                        {
                            saldoItem.ValueVal = 0;
                            saldoItem.Value = "0";
                            saldoItem.ValueKurs = saldoItem.MainKurs;
                            saldoItem.KursDif = 0;
                        }

                    }
                    Cvm.SaveF4();
                }
            }

            Cvm.notupdated = false;
    }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public List<List<string>> Fields { get; set;}
        public int CurrentRowIndex { get; set; }

        internal decimal SaveConto(object item)
        {
            
            decimal kurrazsum = 0;
            List<string> element = new List<string>();
            var citem = item as System.Data.DataRowView;
            if (citem != null)
            {
                foreach (var o in citem.Row.ItemArray)
                {
                    Ref<string> sum = o as Ref<string>;
                    if (sum != null) element.Add(sum.Value.Trim());
                }
                int i = 0;
                string[] stringSeparators = new string[] { "---" };
                if (Tip == 1)
                {

                    foreach (SaldoItem saldoItem in Cvm.ItemsDebit)
                    {
                        if (saldoItem.Name == "Количество")
                        {
                            Cvm.Oborot = decimal.Parse(element[element.Count - 1]);
                            saldoItem.ValueKol = decimal.Parse(element[element.Count - 5]);
                            saldoItem.Value = element[element.Count - 4];
                            continue;

                        }
                        if (saldoItem.Name == "Сума валута")
                        {
                            Cvm.Oborot = decimal.Parse(element[element.Count - 1]);
                            saldoItem.ValueVal = decimal.Parse(element[element.Count - 5]);
                            saldoItem.ValueKurs = saldoItem.ValueVal != 0 ? Cvm.Oborot / saldoItem.ValueVal : 0;
                            saldoItem.MainKurs = saldoItem.ValueVal != 0 ? Cvm.Oborot / saldoItem.ValueVal : 0;
                            saldoItem.KursDif = 0;
                            saldoItem.Value = element[element.Count - 5];
                            foreach (var credititem in Cvm.ItemsCredit)
                            {
                                if (credititem.Name == "Сума валута")
                                {
                                    credititem.ValueVal = saldoItem.ValueVal;
                                    credititem.ValueKurs = saldoItem.ValueKurs;
                                    credititem.KursDif = credititem.ValueVal * (credititem.ValueKurs - credititem.MainKurs);
                                    kurrazsum += credititem.KursDif;
                                    //ВАЛ.СУМА * (КУРС - ОПОРЕН КУРС)
                                }
                            }
                            continue;
                        }
                        if (element[i] != null)
                        {
                            if (element[i].Contains("---"))
                            {
                                var spliti = element[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                saldoItem.Value = spliti[0];
                                saldoItem.Lookupval = spliti[1];
                            }
                            else
                            {
                                saldoItem.Value = element[i].Trim();
                            }
                        }
                        i++;
                    }
                }
                else
                {
                    foreach (SaldoItem saldoItem in Cvm.ItemsCredit)
                    {
                        if (saldoItem.Name == "Количество")
                        {
                            Cvm.Oborot = decimal.Parse(element[element.Count - 1]);
                            saldoItem.ValueKol = decimal.Parse(element[element.Count - 5]);
                            saldoItem.Value = element[element.Count - 5];
                            continue;

                        }
                        if (saldoItem.Name == "Сума валута")
                        {
                            Cvm.Oborot = decimal.Parse(element[element.Count - 5]) * saldoItem.MainKurs;
                            saldoItem.ValueVal = decimal.Parse(element[element.Count - 5]);
                            saldoItem.Value = element[element.Count - 5];

                            continue;

                        }
                        if (element[i] != null)
                        {
                            if (element[i].Contains("---"))
                            {
                                var spliti = element[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                saldoItem.Value = spliti[0];
                                saldoItem.Lookupval = spliti[1];
                            }
                            else
                            {
                                saldoItem.Value = element[i].Trim();
                            }
                        }
                        i++;
                    }
                }
                if (element != null) Cvm.CurrentWraperConto.Oborot = Decimal.Parse(element[element.Count - 1]);
                Cvm.SaveF4();
                
            }
            return kurrazsum;
      
            
        }

        internal void Clear()
        {
            Cvm.Oborot = 0;
            int test;
            if (int.TryParse(Cvm.CurrentWraperConto.CurrentConto.DocNum, out test))
            {
                Cvm.CurrentWraperConto.DocId = (test + 1).ToString();
            }
            if (Tip == 1)
            {
                foreach (SaldoItem saldoItem in Cvm.ItemsDebit)
                {
                    saldoItem.Value = "";
                    saldoItem.Lookupval = "";

                }
            }
            else
            {
                foreach (SaldoItem saldoItem in Cvm.ItemsCredit)
                {
                    saldoItem.Value = "";
                    saldoItem.Lookupval = "";

                }
            }
        }

        private System.Windows.Visibility _IsEditMode;
        public System.Windows.Visibility IsEditMode
        {
            get
            {
                return _IsEditMode;
            }
            set
            {
                _IsEditMode = value;
                OnPropertyChanged("IsEditMode");
            }
        }
        public decimal Suma
        {
            get { return _suma; }
            set { _suma = value;OnPropertyChanged("Suma");}
        }

        public decimal SumaVal
        {
            get { return _sumaval; }
            set { _sumaval = value; OnPropertyChanged("SumaVal"); }
        }
        internal void Filter()
        {
            if (Acc.Kol == 1)
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 5] != Vf.KolFormatUI));
            }
            else if (Acc.Val == 1)
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 5] != Vf.ValFormatUI));
            }
            else
            {
                Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 1] != Vf.LevFormatUI));
            }
            OnPropertyChanged("Fields");
        }

        internal void All()
        {
            Fields=new List<List<string>>(_fields);
            OnPropertyChanged("Fields");
        }
    }
}
