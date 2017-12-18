using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Properties;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using System.Windows;


namespace Tempo2012.UI.WPF.ViewModels
{
    public class SysLookUpViewModel:BaseViewModel
    {
        private string SqlFilter;
        private int _CurrentRowIndex;

        public ObservableCollection<Filter> Filters { get; set; }

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
        
        public SysLookUpViewModel()
        {
            this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllSysLookups());
            if (this._Lookups != null)
            {
                Lookup =_Lookups.Count>0?_Lookups[0]:null;
            }
            
        }

        public DelegateCommand MoveNextPageCommand { get; set; }

        public DelegateCommand MovePreviusPageCommand { get; set; }

        public DelegateCommand MoveLastPageCommand { get; set; }

        public DelegateCommand MoveFirstPageCommand { get; set; }

        private void CalculateFields()
        {
            //this.Fields = new ObservableCollection<ObservableCollection<string>>();
            //if (_Lookup == null) return;
            //LookupModel lm = Context.GetSysLookup(_Lookup.Id);
            //var title = new ObservableCollection<string>();
            //foreach (var field in lm.Fields)
            //{
            //     title.Add(field.Name);
            //}
            //Fields.Add(title);
            //var list = Context.GetSysLookup(_Lookup.Tablename);
            //foreach (var li in list)
            //{
            //    var ader = new ObservableCollection<string>(li);
            //    Fields.Add(ader);
            //}
            this.Fields = new ObservableCollection<ObservableCollection<string>>();
            if (_Lookup == null) return;
            Fields = GetDictionary(LookupModelm, SqlFilter, " FIRST 30");
            Filters = new ObservableCollection<Filter>(GetFilters(LookupModelm));
            SetPager();
        }



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

        private LookUpMetaData _Lookup;
        public LookUpMetaData Lookup
        {
            get
            {
                return _Lookup;
            }
            set
            {
                _Lookup = value;
                if (value == null) return;
                SqlFilter = "";
                LookupModelm = Context.GetSysLookup(_Lookup.Id);
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Filters");
                OnPropertyChanged("Lookup");
            }
        }
       
        protected override void Add()
        {
            editdialog(false);
           
        }
        protected override void Update()
        {
            editdialog(true);
            
        }
        protected override void Delete()
        {
            LookupModel lookup = null;
            if (Lookup != null)
            {
                if ((this.CurrentRowIndex == -1)) return;
                lookup = Context.GetSysLookup(Lookup.Id);
                lookup.LookUpMetaData = Lookup;
                if (MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_Delete_res3, Resources.AnaliticManagerViewModel_Delete_res4, MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    if (Context.DeleteRow(new List<string>(this.Fields[this.CurrentRowIndex + 1]),lookup))
                    {
                        CalculateFields();
                        OnPropertyChanged("Fields");
                        MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_Delete_res5);
                    }
                    else
                    {
                        MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_Delete_res2);
                    }
                }
                

            }

        }

        private void editdialog(bool state)
        {
            List<FieldValuePair> current = new List<FieldValuePair>();
            LookupModel lookup = null;
            if (Lookup != null)
            {
                if ((this.CurrentRowIndex == -1) && state) return;
                lookup = Context.GetSysLookup(Lookup.Id);
                
               
                for (var i = 0; i < this.Fields[this.CurrentRowIndex + 1].Count; i++)
                {
                    if (state)
                    {
                        current.Add(new FieldValuePair
                        {
                            Name = Fields[0][i],
                            Value = Fields[this.CurrentRowIndex + 1][i],
                            Length = lookup.Fields[i].Length,
                            ReadOnly = !(lookup.Fields[i].NameEng == "Id" ||lookup.Fields[i].NameEng == "ID"),
                            IsRequared = lookup.Fields[i].IsRequared,
                            IsUnique = lookup.Fields[i].IsUnique,
                            RCODELOOKUP = lookup.Fields[i].RCODELOOKUP,
                            RFIELDNAME = lookup.Fields[i].RFIELDNAME,
                            RTABLENAME = lookup.Fields[i].RTABLENAME,
                            RFIELDKEY = lookup.Fields[i].RFIELDKEY,
                            Tn=lookup.Fields[i].Tn,
                            Type = lookup.Fields[i].DbField,
                            FieldName = lookup.Fields[i].NameEng
                        });
                    }
                    else
                    {
                        if (i > 0)
                        {
                            current.Add(new FieldValuePair
                            {
                                Name = Fields[0][i],
                                Value = "",
                                Length = lookup.Fields[i].Length,
                                ReadOnly =
                                    (lookup.Fields[i].NameEng == "Id" || lookup.Fields[i].NameEng == "ID")
                                        ? false
                                        : true,
                                IsRequared = lookup.Fields[i].IsRequared,
                                IsUnique = lookup.Fields[i].IsUnique,
                                RCODELOOKUP = lookup.Fields[i].RCODELOOKUP,
                                RFIELDNAME = lookup.Fields[i].RFIELDNAME,
                                RTABLENAME = lookup.Fields[i].RTABLENAME,
                                RFIELDKEY = lookup.Fields[i].RFIELDKEY,
                                Tn = lookup.Fields[i].Tn,
                                Type = lookup.Fields[i].DbField,
                                FieldName = lookup.Fields[i].NameEng
                            });
                        }
                    }

                }
            }
            else
            {
                MessageBoxWrapper.Show(Resources.AnaliticManagerViewModel_editdialogres1);
                return;
            }
            LookupsEdidViewModels vm = new LookupsEdidViewModels(current,lookup.LookUpMetaData.Tablename,false);
            EditInsertLookups ds = new EditInsertLookups(vm);
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                if (state)
                {
                    //update
                    Context.UpdateRowSys(ds.GetNewFields(),lookup);
                }
                else
                { 
                    //nov red
                    Context.SaveRow(ds.GetNewFields(), lookup);
                }
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
        }
        private void SetPager()
        {
            AllPages = FetchCount() / 30 + 1;
            CurrentPage = 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            this.MoveNextPageCommand = new DelegateCommand((o) => this.MoveNextPage());
            this.MovePreviusPageCommand = new DelegateCommand((o) => this.MovePreviusPage());
            this.MoveLastPageCommand = new DelegateCommand((o) => this.MoveLastPage());
            this.MoveFirstPageCommand = new DelegateCommand((o) => this.MoveFirstPage());
        }
        private IEnumerable<Filter> GetFilters(LookupModel lm)
        {

            List<Filter> res = new List<Filter>();
            int i = 0;
            foreach (var item in lm.Fields.Skip(1))
            {
                res.Add(new Filter { FilterField = item.NameEng, FilterName = item.Name });
            }
            return res;
        }
        private void MoveFirstPage()
        {
            if (CurrentPage == 1) return;
            CurrentPage = 1;
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


        private void MoveLastPage()
        {
            if (CurrentPage == AllPages) return;
            CurrentPage = AllPages;
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
            }
            else
            {
                CurrentPage += 1;
            }
            FetchRange((CurrentPage - 1) * 30, 30);
        }

        private void MovePreviusPage()
        {
            if (CurrentPage == 1)
            {
                CurrentPage = AllPages;
            }
            else
            {
                CurrentPage -= 1;
            }
            FetchRange((CurrentPage - 1) * 30, 30);
        }
        public void FetchRange(int startIndex, int count)
        {

            if (_Lookup == null) return;
            Fields = new ObservableCollection<ObservableCollection<string>>();
            string range = string.Format("FIRST {0} SKIP {1} ", count, startIndex);
            Fields = GetDictionary(LookupModelm, SqlFilter, range);
            OnPropertyChanged("Fields");
        }

        public LookupModel LookupModelm { get; set; }
        internal void Refresh(Filter tag)
        {
            if (_Lookup == null) return;
            Fields = new ObservableCollection<ObservableCollection<string>>();
            SqlFilter = string.Format("UPPER (\"{0}\") like '%{1}%'", tag.FilterField, tag.FilterWord.ToUpper());
            AllPages = FetchCount() / 30 + 1;
            CurrentPage = 1;
            Fields = GetDictionary(LookupModelm, SqlFilter, " FIRST 30 ");
            OnPropertyChanged("Fields");
        }

        public int FetchCount()
        {
            if (_Lookup == null) return 0;
            return Context.GetFilteredSysLookupCount(LookupModelm.LookUpMetaData.Tablename,
                                                 SqlFilter);
        }
        public ObservableCollection<ObservableCollection<string>> GetDictionary(LookupModel lm, string sqlfilter, string range)
        {
            ObservableCollection<ObservableCollection<string>> res = new ObservableCollection<ObservableCollection<string>>();
            ObservableCollection<string> title = new ObservableCollection<string>();
            foreach (var item in lm.Fields)
            {
                title.Add(item.Name);
            }
            res.Add(title);

            var rez = Context.GetSysLookupDictionary(lm.LookUpMetaData.Tablename,sqlfilter,range,"");
            foreach (Dictionary<string, object> dictionary in rez)
            {
                title = new ObservableCollection<string>(lm.Fields.Select(item => dictionary[item.NameEng].ToString()).ToList());
                res.Add(title);
            }
            return res;
        }


    }
}
