using ReportBuilder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    public class ViesGvm : BaseViewModel,IReportBuilder
    {

        public Dictionary<int, List<string>> Rowfoother { get; set; }
        private int _CurrentRowIndex;
        public int CurrentRowIndex
        {
            get
            {
                return _CurrentRowIndex;
            }
            set
            {
                if (_CurrentRowIndex == value) return;
                _CurrentRowIndex = value;

            }
        }
        public void LoadSettings(string Path)
        {

            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);


        }

        public void SaveSettings(string Path)
        {

            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);

        }
        public DelegateCommand MoveNextPageCommand { get; set; }

        public DelegateCommand MovePreviusPageCommand { get; set; }

        public DelegateCommand MoveLastPageCommand { get; set; }

        public DelegateCommand MoveFirstPageCommand { get; set; }
        public DelegateCommand MoveToPageCommand { get; set; }
        public ViesGvm()
        {
            
                this.CalculateFields();
            
            HideCombo = true;
            SetPager();
            IsShowReport = true;
            IsShowNavigation = false;
        }

        private void SetPager()
        {
            AllPages = FetchCount() / 30 + 1;
            CurrentPage = 1;
            TargetPage = 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            this.MoveNextPageCommand = new DelegateCommand((o) => this.MoveNextPage());
            this.MovePreviusPageCommand = new DelegateCommand((o) => this.MovePreviusPage());
            this.MoveLastPageCommand = new DelegateCommand((o) => this.MoveLastPage());
            this.MoveFirstPageCommand = new DelegateCommand((o) => this.MoveFirstPage());
            this.MoveToPageCommand = new DelegateCommand((o) => this.MoveToPage());
        }

        private void MoveToPage()
        {
            if (TargetPage != CurrentPage && TargetPage <= AllPages && TargetPage > 0)
            {
                CurrentPage = TargetPage;
                FetchRange((CurrentPage - 1) * 30, 30);
            }
        }

        private void MoveFirstPage()
        {
            if (CurrentPage == 1) return;
            CurrentPage = 1;
            TargetPage = 1;
            FetchRange((CurrentPage - 1) * 30, 30);
        }


        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                FromToPages = string.Format("{0} от {1}", value, AllPages);
            }
        }

        private int _targetPage;
        public int TargetPage
        {
            get { return _targetPage; }
            set
            {
                _targetPage = value;
                OnPropertyChanged("TargetPage");
            }
        }

        private void MoveLastPage()
        {
            if (CurrentPage == AllPages) return;
            CurrentPage = AllPages;
            TargetPage = AllPages;
            FetchRange((CurrentPage - 1) * 30, 30);
        }

        private int _allPages;
        public int AllPages
        {
            get { return _allPages; }
            set { _allPages = value; }
        }

        private string _fromToPages;
        public string FromToPages
        {
            get { return _fromToPages; }
            set { _fromToPages = value; OnPropertyChanged("FromToPages"); }
        }

        private void MoveNextPage()
        {
            if (CurrentPage == AllPages)
            {
                CurrentPage = 1;
                TargetPage = CurrentPage;
            }
            else
            {
                CurrentPage += 1;
                TargetPage = CurrentPage;
            }
            FetchRange((CurrentPage - 1) * 30, 30);
        }

        private void MovePreviusPage()
        {
            if (CurrentPage == 1)
            {
                CurrentPage = AllPages;
                TargetPage = CurrentPage;
            }
            else
            {
                CurrentPage -= 1;
                TargetPage = CurrentPage;
            }
            FetchRange((CurrentPage - 1) * 30, 30);
        }
        //public ViesGvm(int codelookup)
        //{
        //    this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
        //    if (this._Lookups != null)
        //    {
        //        Lookup = _Lookups.FirstOrDefault(e => e.Id == codelookup);
        //        this.CalculateFields();
        //    }
        //    HideCombo = false;

        //}

        private void CalculateFields()
        {
            this.Fields = new ObservableCollection<ObservableCollection<string>>();
            LookupModelm = new LookupModel();
            LookupModelm.Fields = new List<TableField> () ;
            LookupModelm.Fields.Add(new TableField
            {
                Name = "Ключ",
                DbField = "Id",
                NameEng = "Id",
                Length = 3,
                IsRequared = true,
               
        
            });
            
            LookupModelm.Fields.Add(new TableField
            {
                Name = "Период",
                DbField = "integer",
                NameEng = "PERIOD",
                Length = 10
            });
            LookupModelm.Fields.Add(new TableField
            {
                Name = "VIN Контрагент",
                DbField = "VARCHAR(16)",
                NameEng = "VIN",
                Length = 16
            });
            LookupModelm.Fields.Add(new TableField
            {
                Name = "Код Операция",
                DbField = "integer",
                NameEng = "KOD",
                Length=10
            });
            LookupModelm.Fields.Add(new TableField
            {
                Name = "VIN заместващ Контрагент",
                DbField = "VARCHAR(16)",
                NameEng = "VINDEST",
                Length=16
            });
            LookupModelm.Fields.Add(new TableField
            {
                Name = "Период Операция",
                DbField = "VARCHAR(10)",
                NameEng = "PERIODOP",
                Length=10
            });
            LookupModelm.Fields.Add(new TableField
            {
                Name = "Фирма Код",
                DbField = "integer",
                NameEng = "FIRMAID"
            });
            LookupModelm.LookUpMetaData = new LookUpMetaData { Tablename = "VIESG", Name = "Виес Г" };
            Fields = GetDictionary(LookupModelm, SqlFilter, " FIRST 30", "");
            PopulateReportItems();
            Filters = new ObservableCollection<Filter>(GetFilters(LookupModelm));
            SetPager();

        }
        public ObservableCollection<ObservableCollection<string>> GetDictionary(LookupModel lm, string sqlfilter, string range, string orderby)
        {
            ObservableCollection<ObservableCollection<string>> res = new ObservableCollection<ObservableCollection<string>>();
            ObservableCollection<string> title = new ObservableCollection<string>();
            foreach (var item in lm.Fields)
            {
                title.Add(item.Name);
            }
            res.Add(title);

            var rez = Context.GetLookupDictionary(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, sqlfilter, range, orderby);
            foreach (Dictionary<string, object> dictionary in rez)
            {
                title = new ObservableCollection<string>(lm.Fields.Select(item => dictionary[item.NameEng].ToString()).ToList());
                res.Add(title);
            }
            return res;
        }
        private IEnumerable<Filter> GetFilters(LookupModel lm)
        {

            List<Filter> res = new List<Filter>();
            foreach (var item in lm.Fields.Skip(1))
            {
                res.Add(new Filter { FilterField = item.NameEng, FilterName = item.Name });
            }
            return res;
        }

        public ObservableCollection<Filter> Filters { get; set; }

        public ObservableCollection<ObservableCollection<string>> Fields
        {
            get;
            set;
        }

        private ObservableCollection<LookUpMetaData> _Lookups;
        public ObservableCollection<LookUpMetaData> Lookups
        {
            get
            {
                return _Lookups;
            }
            set
            {
                _Lookups = value;
                OnPropertyChanged("Lookups");
            }
        }

        //private LookUpMetaData _Lookup;
        //public LookUpMetaData Lookup
        //{
        //    get
        //    {
        //        return _Lookup;
        //    }
        //    set
        //    {
        //        _Lookup = value;
        //        if (value == null) return;
        //        SqlFilter = "";
        //        LookupModelm = Context.GetLookup(_Lookup.Id);
        //        LookupModelm.Fields.Add(new TableField
        //        {
        //            Name = "Фирма Код",
        //            DbField = "integer",
        //            NameEng = "FIRMAID"
        //        });
        //        CalculateFields();
        //        OnPropertyChanged("Fields");
        //        OnPropertyChanged("Filters");
        //        OnPropertyChanged("Lookup");
        //    }
        //}

        protected override void Add()
        {
            editdialog(false);

        }
        protected override void Update()
        {
            editdialog(true);

        }

        protected override void Report()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }

        protected override void Delete()
        {
            
                if ((this.CurrentRowIndex == -1)) return;
                if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    
                        if (Context.DeleteRow(new List<string>(this.Fields[this.CurrentRowIndex + 1]), LookupModelm))
                        {
                            CalculateFields();
                            OnPropertyChanged("Fields");
                            MessageBoxWrapper.Show("Записът е изтрит");
                        }
                        else
                        {
                            MessageBoxWrapper.Show("Грешка при триене");
                        }
                    
                }


            

        }

        private void editdialog(bool state)
        {
            List<FieldValuePair> current = new List<FieldValuePair>();
            
                if ((this.CurrentRowIndex == -1) && state) return;



                for (var i = 0; i < this.Fields[this.CurrentRowIndex + 1].Count - 1; i++)
                {
                    if (state)
                    {
                        current.Add(new FieldValuePair
                        {
                            Name = Fields[0][i],
                            Value = Fields[this.CurrentRowIndex + 1][i],
                            Length = LookupModelm.Fields[i].Length,
                            ReadOnly = (LookupModelm.Fields[i].NameEng == "Id") ? false : true,
                            Type = LookupModelm.Fields[i].DbField,
                            IsRequared = LookupModelm.Fields[i].IsRequared,
                            IsUnique = LookupModelm.Fields[i].IsUnique,
                            Tn = LookupModelm.Fields[i].Tn,
                            RTABLENAME = LookupModelm.Fields[i].RTABLENAME,
                            FieldName = LookupModelm.Fields[i].NameEng
                        });
                    }
                    else
                    {
                        if (this.CurrentRowIndex == -1)
                        {
                            current.Add(new FieldValuePair
                            {
                                Name = Fields[0][i],
                                Value = "",
                                Length = LookupModelm.Fields[i].Length,
                                ReadOnly = (LookupModelm.Fields[i].NameEng == "Id") ? false : true,
                                Type = LookupModelm.Fields[i].DbField,
                                IsRequared = LookupModelm.Fields[i].IsRequared,
                                IsUnique = LookupModelm.Fields[i].IsUnique,
                                Tn = LookupModelm.Fields[i].Tn,
                                RTABLENAME = LookupModelm.Fields[i].RTABLENAME,
                                FieldName = LookupModelm.Fields[i].NameEng
                            });
                        }
                        else {
                            current.Add(new FieldValuePair
                            {
                                Name = Fields[0][i],
                                Value = (LookupModelm.Fields[i].NameEng == "Id")? " ":Fields[this.CurrentRowIndex + 1][i],
                                Length = LookupModelm.Fields[i].Length,
                                ReadOnly = (LookupModelm.Fields[i].NameEng == "Id") ? false : true,
                                Type = LookupModelm.Fields[i].DbField,
                                IsRequared = LookupModelm.Fields[i].IsRequared,
                                IsUnique = LookupModelm.Fields[i].IsUnique,
                                Tn = LookupModelm.Fields[i].Tn,
                                RTABLENAME = LookupModelm.Fields[i].RTABLENAME,
                                FieldName = LookupModelm.Fields[i].NameEng
                            });
                        }
                    }

                }
            
           
            LookupsEdidViewModels vm = new LookupsEdidViewModels(current, LookupModelm.LookUpMetaData.Tablename, !state);
            EditInsertLookups ds = new EditInsertLookups(vm);
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                if (state)
                {
                    //update

                    Context.UpdateRow(ds.GetNewFields(), LookupModelm);

                }
                else
                {
                    
                    var list = new List<string>(ds.GetNewFields());
                    Context.SaveRow(list, LookupModelm, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                    
                }
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
       }


        private bool _hideCombo;
        private string SqlFilter;
        private List<ReportItem> _reportItems;

        public bool HideCombo
        {
            get { return _hideCombo; }
            set { _hideCombo = value; OnPropertyChanged("HideCombo"); }
        }

        public void Refresh(Filter tag)
        {
            //if (_Lookup == null) return;
            Fields = new ObservableCollection<ObservableCollection<string>>();
            if (tag.FilterWord != null)
            {
                SqlFilter = string.Format("AND \"{0}\" like '%{1}%'", tag.FilterField, tag.FilterWord.ToUpper());
            }
            else
            {
                SqlFilter = "";
            }
            OrderBY = string.Format(" order by \"{0}\"", tag.FilterField);
            AllPages = FetchCount() / 30 + 1;
            CurrentPage = 1;
            Fields = GetDictionary(LookupModelm, SqlFilter, " FIRST 30 ", OrderBY);
            OnPropertyChanged("Fields");
        }

        private void PopulateReportItems()
        {
            _reportItems = new List<ReportItem>();
            if (Fields != null)
                foreach (var field in Fields[0])
                {
                    _reportItems.Add(new ReportItem { IsShow = true, Name = field, Height = 30, Width = 20 });
                }
        }

        public int FetchCount()
        {
            //if (_Lookup == null) return 0;
            return Context.GetFilteredLookupCount(LookupModelm.LookUpMetaData.Tablename,
                                                 ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, SqlFilter);
        }

        public void FetchRange(int startIndex, int count)
        {

            //if (_Lookup == null) return;
            Fields = new ObservableCollection<ObservableCollection<string>>();
            string range = string.Format("FIRST {0} SKIP {1} ", count, startIndex);
            Fields = GetDictionary(LookupModelm, SqlFilter, range, OrderBY);
            OnPropertyChanged("Fields");
        }

        protected string OrderBY
        {
            get;
            set;
        }

        public LookupModel LookupModelm { get; set; }

        private DateTime fromDate;
        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
            }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
            }
        }
        public List<List<string>> GetItems()
        {
            var rez = Context.GetLookupDictionary(LookupModelm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
            return rez.Select(dictionary => new List<string>(LookupModelm.Fields.Select(item => dictionary[item.NameEng].ToString()).ToList())).ToList();
        }
        public void TransverLookUp()
        {
            StringBuilder sb = new StringBuilder();
            var rez = new List<Dictionary<string, object>>(Context.GetLookupDictionary(LookupModelm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            foreach (var item in rez)
            {
                string vals = "";
                string keys = "";
                foreach (var sitem in item)
                {
                    if (sitem.Key.ToUpper() == "ID") continue;
                    vals += string.Format("'{0}',", sitem.Value.ToString());
                    keys += string.Format("\"{0}\",", sitem.Key);
                }
                keys = keys.Remove(keys.Length - 1, 1);
                vals = vals.Remove(vals.Length - 1, 1);
                string command = string.Format("Insert into \"{0}\"({1}) VALUES({2});", LookupModelm.LookUpMetaData.Tablename, keys, vals);
                sb.AppendLine(command);
            }
            string path = Path.Combine(Entrence.CurrentFirmaPathReport, LookupModelm.LookUpMetaData.Tablename + ".txt");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(sb.ToString());
            }
            Process.Start(path);
        }
        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            var ret = new List<string>();
            ret.Add(String.Format("Дата на извлечението: {0}", DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}", Entrence.UserName));
            return ret;
        }

        public List<string> GetFuther()
        {
            return null;
        }

        public string Filename
        {
            get
            {
                if (LookupModelm != null && LookupModelm.LookUpMetaData != null)
                    return "nom" + LookupModelm.LookUpMetaData.Name;
                return "nomeclaturenoname";
            }
        }

        private string title;
        public string SubTitle
        {
            get
            {
                if (LookupModelm != null && LookupModelm.LookUpMetaData != null)
                    return "Справка " + LookupModelm.LookUpMetaData.Name;
                return "Справка";
            }
            set { title = value; }
        }
        public string Title
        {
            get; set;
        }
        public IEnumerable<ReportItem> ReportItems
        {
            get { return _reportItems; }
            set { _reportItems = new List<ReportItem>(value); }
        }

        public List<string> GetSubTitles()
        {
            return null;
        }

        public List<List<string>> GetTXTAntetka()
        {
            return null;
        }

        public string DeleteAllItems()
        {
            return Context.FbBatchExecution(string.Format("Delete from \"{0}\"", LookupModelm.LookUpMetaData.Tablename));
        }

        public string DeleteAllItemsFirma()
        {
            return Context.FbBatchExecution(string.Format("Delete from \"{0}\" where FIRMAID={1}", LookupModelm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
        }


    }
}

