using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DataGrid2DLibrary;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    public class DetailsUniverseViewModel:BaseViewModel
    {
        public const int PAGECOUNT = 20;
        private List<List<string>> _fields;
        private ContoViewModel Cvm;
        private decimal _suma;
        private int Tip;
        private string _title;
        private int Count;

        public DetailsUniverseViewModel(EntityFramework.Models.AccountsModel dAccountsModel,string filter,ContoViewModel cvm,int tip, EditMode mode)
        {
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
            //if (dAccountsModel.AnaliticalNum == 5)
            //{
            //    var v = Context.GetDetailsContoToAcc(dAccountsModel.Id, dAccountsModel.TypeAccount, filter);
            //    _fields = new List<List<string>>();
            //    if (v != null)
            //    {
            //        foreach (var item in v)
            //        {
            //            _fields.Add(new List<string>(item));
            //        }
            //    }
            //    else
            //    {
            //        var r = new List<string>();
            //        var atr = Context.LoadAllAnaliticfields(dAccountsModel.Id);
            //        foreach (SaldoAnaliticModel saldoAnaliticModel in atr)
            //        {
            //            r.Add(saldoAnaliticModel.Name);

            //        }
            //        r.Add("НС");
            //        r.Add("ОД");
            //        r.Add("ОК");
            //        r.Add("КС");
            //        r.Add("Дата");
            //        _fields.Add(r);
            //    }
            //    Fields = new List<List<string>>(_fields.Where(e => e[e.Count - 1] != "0.00")); ;
            //    OnPropertyChanged("Fields");
            //}
            //else
            //if (dAccountsModel.AnaliticalNum == 3)
            //{
            //    var v = Context.GetDetailsContoToAccMat(dAccountsModel.Id, dAccountsModel.TypeAccount, filter);
            //    _fields = new List<List<string>>();
            //    if (v != null)
            //    {
            //        foreach (var item in v)
            //        {
            //            _fields.Add(new List<string>(item));
            //        }
            //    }
            //    else
            //    {
            //        var r = new List<string>();
            //        var atr = Context.LoadAllAnaliticfields(dAccountsModel.Id);
            //        foreach (SaldoAnaliticModel saldoAnaliticModel in atr)
            //        {
            //            r.Add(saldoAnaliticModel.Name);

            //        }
            //        r.Add("НС");
            //        r.Add("ОД");
            //        r.Add("ОК");
            //        r.Add("КС");
            //        r.Add("Дата");
            //        _fields.Add(r);
            //    }
            //    Fields = new List<List<string>>(_fields);
            //    OnPropertyChanged("Fields");
            //}
            //if (dAccountsModel.AnaliticalNum == 6)
            //{
            //    var v = Context.GetDetailsContoToAccVal(dAccountsModel.Id, dAccountsModel.TypeAccount, filter);
            //    _fields = new List<List<string>>();
            //    if (v != null)
            //    {
            //        foreach (var item in v)
            //        {
            //            _fields.Add(new List<string>(item));
            //        }
            //    }
            //    else
            //    {
            //        var r = new List<string>();
            //        var atr = Context.LoadAllAnaliticfields(dAccountsModel.Id);
            //        foreach (SaldoAnaliticModel saldoAnaliticModel in atr)
            //        {
            //            r.Add(saldoAnaliticModel.Name);

            //        }
            //        r.Add("НС");
            //        r.Add("ОД");
            //        r.Add("ОК");
            //        r.Add("КС");
            //        r.Add("Дата");
            //        _fields.Add(r);
            //    }
            //    Fields = new List<List<string>>(_fields);
            //    OnPropertyChanged("Fields");
            //}
            //else
            //{
             List<List<string>> v = null;
             Count = Context.GetDetailsContoToAccUniCount(dAccountsModel.Id, dAccountsModel.TypeAccount, filter);
            if (Count < PAGECOUNT)
            {
                var c = Context.GetDetailsContoToAccUni(dAccountsModel.Id, dAccountsModel.TypeAccount, dAccountsModel.Kol, dAccountsModel.Val, filter, 0, Count);
                v = c.Select(i => i.ToList()).ToList();
            }
            else
            {
                var c = Context.GetDetailsContoToAccUni(dAccountsModel.Id, dAccountsModel.TypeAccount, dAccountsModel.Kol, dAccountsModel.Val, filter, 0, PAGECOUNT);
                v = c.Select(i => i.ToList()).ToList();
            }
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
                Fields = new List<List<string>>(_fields);
                OnPropertyChanged("Fields");
            //}
           
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public List<List<string>> Fields { get; set;}
        public int CurrentRowIndex { get; set; }

        internal void SaveConto(object item)
        {
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
                            saldoItem.Value = element[element.Count-4];
                            continue;
                            
                        }
                        if (saldoItem.Name == "Сума валута")
                        {
                            saldoItem.Value = element[element.Count-4];
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
                            saldoItem.Value = element[element.Count - 4];
                            i++;
                            continue;

                        }
                        if (saldoItem.Name == "Сума валута")
                        {
                            saldoItem.Value = element[element.Count - 4];
                            i++;
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

        internal void Filter()
        {
            Fields=new List<List<string>>(_fields.Where(e=>e[e.Count-1]!="0.00"));
            OnPropertyChanged("Fields");
        }

        internal void All()
        {
            Fields=new List<List<string>>(_fields);
            OnPropertyChanged("Fields");
        }
    }
}
