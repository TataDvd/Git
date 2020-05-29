using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using ReportBuilder;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.Views.ReportManager;
using ReportItem = ReportBuilder.ReportItem;
using System.Threading;
using System.Windows;

namespace Tempo2012.UI.WPF.ViewModels.Dnevnici
{
    public class DdsViewModel : BaseViewModel, IReportBuilder, IDataErrorInfo
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public delegate void RefreshElement(object sender, DdsEventArgs e);
        public delegate void CancelSaveElement(object sender, DdsCancelEventArgs e);

        public event RefreshElement RefreshExecuted;
        public event RefreshElement DdsSaved;
        public event CancelSaveElement CancelSave;
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }
        protected virtual void OnDdsSaved(DdsEventArgs e)
        {
            RefreshElement handler = DdsSaved;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDdsCancelSave(DdsCancelEventArgs e)
        {
            CancelSaveElement handler = CancelSave;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnRefreshExecuted(DdsEventArgs e)
        {
            if (RefreshExecuted != null)
            {
                RefreshExecuted(this, e);
            }
        }
        public string SubTitle
        {
            get
            {
                return ddsDnevnikModel.Title;
            }
            set
            {
                ddsDnevnikModel.Title = value;
            }
        }
        public string Title
        {
            get; set;
        }
        public string Linked
        {
            get
            {
                if (ddsDnevnikModel.IsLinked)
                {
                    if (ddsDnevnikModel.IsSuma==1)
                    {
                        return string.Format("Сборна Сума");
                    }
                    if (_kor) return string.Format("Сума в дневник {0}",_oldsum);

                    return "Записана в дневника";
                }
                return "Не е записана в дневника";
            }
        }

        public bool IsLinked
        {
            get
            {
                return ddsDnevnikModel.IsLinked;
            }
        }

        public string Total
        {
            get { return "Сума "+ddsDnevnikModel.Total;}
        }

        public string DdsIncluded
        {
            get { return ddsDnevnikModel.DdsIncluded;}
        }

        public bool CanSaveDds
        {
            get { return _canSaveDds; }
            set { _canSaveDds = value; OnPropertyChanged("CanSaveDds"); }
        }

        public DdsViewModel(DdsDnevnikModel ddsmodel)
        {
            ddsDnevnikModel = ddsmodel;
            KindDocLookup = new ObservableCollection<LookUpSpecific>(Context.GetAllDocTypes());
            if (ddsDnevnikModel.CodeDoc == null)
            {
                ddsDnevnikModel.CodeDoc = "01";
            }
            KindDoc = KindDocLookup.FirstOrDefault(e => e.CodetId == ddsDnevnikModel.CodeDoc);
            ActivityTypeLookup = new ObservableCollection<LookUpSpecific>
                                {
                                    new LookUpSpecific{CodetId = "01",Id=1,Name = "Покупки"},
                                    new LookUpSpecific{CodetId = "02",Id=2,Name = "Продажби"},
                                    new LookUpSpecific{CodetId = "03",Id=3,Name = "Други"},
                                };
            this.Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(" where NAMEENG='k'"));

            AllFields = new ObservableCollection<DdsDnevnicItem>();
            foreach (var items in ddsDnevnikModel.DetailItems)
            {
                AllFields.Add(new DdsDnevnicItem(items));
            }
            if (ddsmodel.LookupID > 0)
            {
                Lookup = Lookups.FirstOrDefault(e => e.Id == ddsmodel.LookupID);
                if (Lookup==null)
                {
                     Lookup = ddsmodel.KindActivity == 2 ? Lookups.FirstOrDefault(e => e.Name == "Клиенти") : Lookups.FirstOrDefault(e => e.Name == "Доставчици");
                    ddsmodel.LookupElementID = 0;
                }
            }
            else
            {
                Lookup = ddsmodel.KindActivity == 2 ? Lookups.FirstOrDefault(e => e.Name == "Клиенти") : Lookups.FirstOrDefault(e => e.Name == "Доставчици");

            }
            if (SelectedItem==null)
            {
                SelectedItem=new SaldoItem();
            }
            SelectedItem.Value = ddsDnevnikModel.ClNum;
            Bustad = ddsDnevnikModel.Bulstat;
            DdsId = ddsDnevnikModel.Nzdds;
            ClName = ddsDnevnikModel.NameKontr;
            if (Lookup != null) SelectedItem.Name = Lookup.Name;
            OnPropertyChanged("SelectedItem");
            ddsDnevnikModel.Total = ddsDnevnikModel.DetailItems.Sum(e => e.DdsSuma).ToString(Vf.LevFormatUI);
        }

        public DdsViewModel(DdsDnevnikModel ddsmodel, DdsDnevnicItem item)
        {
            ddsDnevnikModel = ddsmodel;
            KindDocLookup = new ObservableCollection<LookUpSpecific>(Context.GetAllDocTypes());
            if (ddsDnevnikModel.CodeDoc == null)
            {
                ddsDnevnikModel.CodeDoc = "01";
            }
            KindDoc = KindDocLookup.FirstOrDefault(e => e.CodetId == ddsDnevnikModel.CodeDoc);
            ActivityTypeLookup = new ObservableCollection<LookUpSpecific>
            {
                new LookUpSpecific {CodetId = "01", Id = 1, Name = "Покупки"},
                new LookUpSpecific {CodetId = "02", Id = 2, Name = "Продажби"},
                new LookUpSpecific {CodetId = "03", Id = 3, Name = "Други"},
            };
            this.Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(" where NAMEENG='k'"));

            AllFields = new ObservableCollection<DdsDnevnicItem>();
            foreach (var litem in ddsDnevnikModel.DetailItems)
            {
                if (ddsDnevnikModel.IsSuma == 0)
                {
                    if (litem.Name == item.Name)
                    {
                        if (litem.DdsSuma > 0 && litem.DdsSuma != item.DdsSuma)
                        {
                            _kor = true;
                            _oldsum = litem.DdsSuma.ToString(Vf.LevFormatUI);
                        }
                        else
                        {
                            _kor = false;
                        }
                        litem.DdsSuma = item.DdsSuma;
                        litem.In = true;
                    }
                }
                else
                {
                    if (!ddsDnevnikModel.IsLinked)
                    {
                        if (litem.Name == item.Name)
                        {
                            litem.DdsSuma = item.DdsSuma;
                            ddsDnevnikModel.Total = item.DdsSuma.ToString(Vf.LevFormatUI);
                            litem.In = true;
                        }
                    }
                }
                AllFields.Add(new DdsDnevnicItem(litem));
            }

            if (ddsmodel.LookupID > 0)
            {
                Lookup = Lookups.FirstOrDefault(e => e.Id == ddsmodel.LookupID);
                if (Lookup == null)
                {
                    Lookup = ddsmodel.KindActivity == 2
                        ? Lookups.FirstOrDefault(e => e.Name == "Клиенти")
                        : Lookups.FirstOrDefault(e => e.Name == "Доставчици");
                    ddsmodel.LookupElementID = 0;
                }
            }
            else
            {
                Lookup = ddsmodel.KindActivity == 2
                    ? Lookups.FirstOrDefault(e => e.Name == "Клиенти")
                    : Lookups.FirstOrDefault(e => e.Name == "Доставчици");

            }
            if (ddsmodel.LookupElementID > 0)
            {
                string SerachedField = "";
                if (Lookup != null)
                {

                    SerachedField = Context.GetLookup(Lookup.Id).Fields[0].NameEng;

                }
                var list = Context.GetLookupDictionary(_lookUpMetaData.Tablename,
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    string.Format("And \"{0}\"={1}", SerachedField, ddsmodel.LookupElementID));
                foreach (var li in list)
                {
                    if (li != null)
                    {
                        if (li.ContainsKey("Name"))
                        {
                            SelectedItem.Lookupval = li["Name"].ToString();
                            ClName = SelectedItem.Lookupval;
                        }
                        if (li.ContainsKey("VAT")) DdsId = li["VAT"].ToString();
                        if (li.ContainsKey("BULSTAT")) Bustad = li["BULSTAT"].ToString();
                        if (li.ContainsKey("KONTRAGENT")) SelectedItem.Value = li["KONTRAGENT"].ToString();
                    }
                }
            }
            else
            {
                if (ddsmodel.ClNum != null)
                {
                    string SerachedField = "";
                    if (Lookup != null)
                    {

                        SerachedField = Context.GetLookup(Lookup.Id).Fields[1].NameEng;

                    }
                    var list = Context.GetLookupDictionary(_lookUpMetaData.Tablename,
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        string.Format("And \"{0}\"={1}", SerachedField, ddsmodel.ClNum));
                    foreach (var li in list)
                    {
                        if (li != null)
                        {
                            if (li.ContainsKey("Name"))
                            {
                                SelectedItem.Lookupval = li["Name"].ToString();
                                ClName = SelectedItem.Lookupval;
                            }
                            if (li.ContainsKey("VAT")) DdsId = li["VAT"].ToString();
                            if (li.ContainsKey("BULSTAT")) Bustad = li["BULSTAT"].ToString();
                            if (li.ContainsKey("KONTRAGENT")) SelectedItem.Value = li["KONTRAGENT"].ToString();
                            if (li.ContainsKey("Id"))
                            {
                                int id;
                                ddsDnevnikModel.LookupElementID = int.TryParse(li["Id"].ToString(),out id)?id:0;
                            }
                        }
                    }
                }
            }
        }

        public DdsDnevnikModel ddsDnevnikModel;
        public ObservableCollection<LookUpSpecific> KindDocLookup { get; set; }
        private LookUpSpecific _currentKindDoc;
        public LookUpSpecific CurrentKindDoc
        {
            get { return _currentKindDoc; }
            set { _currentKindDoc = value; OnPropertyChanged("CurrentKindDoc");
                CanSave();
            }
        }

        private LookUpSpecific _currentActivity;
        public LookUpSpecific CurrentActivity
        {
            get { return _currentActivity; }
            set
            {
                _currentActivity = value;
                OnPropertyChanged("LookUpSpecific");
                CanSave();
            }
        }

        private LookUpMetaData _lookUpMetaData;
        public LookUpMetaData Lookup
        {
            get { return _lookUpMetaData; }
            set
            {
                _lookUpMetaData = value;
                if (value==null) return;
                if (SelectedItem == null) SelectedItem = new SaldoItem();
                SelectedItem.Relookup = value.Id;
                SelectedItem.Name = value.Name;
                
                ddsDnevnikModel.LookupID = _lookUpMetaData.Id;
               
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("Lookup");
                OnPropertyChanged("SelectedLookup");
                CanSave();
            }
        }

        
        public ObservableCollection<LookUpSpecific> ActivityTypeLookup { get; set; }
        public ObservableCollection<LookUpMetaData> Lookups { get; set; }

        public string Branch
        {
            get { return ddsDnevnikModel.Branch; }
            set
            {
                ddsDnevnikModel.Branch = value;
                OnPropertyChanged("Branch");
            }
        }
        public string A8
        {
            get { return ddsDnevnikModel.A8; }
            set
            {
                ddsDnevnikModel.A8 = value;
                OnPropertyChanged("A8");
            }
        }
        public string ClNum
        {
            get { return ddsDnevnikModel.ClNum; }
            set
            {
                ddsDnevnikModel.ClNum = value;
                OnPropertyChanged("ClNum");
            }
        }
        public string Stoke
        {
            get
            {
                return ddsDnevnikModel.Stoke;
            }
            set
            {
                ddsDnevnikModel.Stoke = value;
                OnPropertyChanged("Stoke");
            }
        }
        public string DocID
        {
            get { return ddsDnevnikModel.DocId; }
            set
            {
                ddsDnevnikModel.DocId = value;
                OnPropertyChanged("DocID");
                CanSave();
            }
        }
        public DateTime Date
        {
            get { return ddsDnevnikModel.Date; }
            set
            {
                ddsDnevnikModel.Date = value;
                OnPropertyChanged("Date");
            }
        }
        public DateTime DataF
        {
            get { return ddsDnevnikModel.DataF;}
            set
            {
                ddsDnevnikModel.DataF = value;
                OnPropertyChanged("DataF");
            }
        }
        public int KindActivity
        {
            get { return ddsDnevnikModel.KindActivity; }
            set
            {
                ddsDnevnikModel.KindActivity = value;
                OnPropertyChanged("KindActivity");
            }
        }
        public string DdsId
        {
            get { if (SelectedItem != null) return SelectedItem.Vat;return null;}
            set
            {
                if (SelectedItem != null) SelectedItem.Vat = value;
                OnPropertyChanged("DdsId");
                CanSave();
            }
        }

        public string Bustad
        {
            get { if (SelectedItem != null) return SelectedItem.Bulstad;
                return null;
            }
            set
            {
                if (SelectedItem != null) SelectedItem.Bulstad = value;
                OnPropertyChanged("Bustad");
                CanSave();
            }
        }
        private LookUpSpecific _kindDoc;
        public LookUpSpecific KindDoc
        {
            get { return _kindDoc;}
            set
            {
                if (ddsDnevnikModel != null && value!=null)
                {
                    ddsDnevnikModel.KindDoc = value.Id;
                    ddsDnevnikModel.CodeDoc = value.CodetId;
                }
                _kindDoc = value;
                OnPropertyChanged("KindDoc");
            }
        }

        public int LookupID
        {
            get { return ddsDnevnikModel.LookupID; }
            set
            {
                ddsDnevnikModel.LookupID = value;
                OnPropertyChanged("LookupID");
            }
        }
        public int LookupElementID
        {
            get { return ddsDnevnikModel.LookupElementID; }
            set
            {
                ddsDnevnikModel.LookupElementID = value;
                OnPropertyChanged("LookupElementID");
            }
        }
        public ObservableCollection<DdsDnevnicItem> AllFields { get; set; }


        protected override bool CanSave()
        {
            CanSaveDds = ValidString(DocID)  && Lookup != null && SelectedItem!=null&&!String.IsNullOrWhiteSpace(SelectedItem.Vat);
            
            return CanSaveDds;
        }

        private bool ValidString(string bustad)
        {
            return !string.IsNullOrWhiteSpace(bustad);
        }

        protected override void Save()
        {   
            if (!CanSave()) return;
            if (SelectedItem != null)
            {
                ddsDnevnikModel.Bulstat = Bustad;
                //ddsDnevnikModel.NameKontr = SelectedItem.Lookupval;
                ddsDnevnikModel.Nzdds =DdsId;
                ddsDnevnikModel.ClNum = SelectedItem.Value;
                //ddsDnevnikModel.LookupElementID = SelectedItem.LiD > 0 ? SelectedItem.LiD : ddsDnevnikModel.LookupElementID;
            }
            Context.SaveDdsDnevnicModel(ddsDnevnikModel,ddsDnevnikModel.IsLinked);
            string selestitem="";
            foreach (var ddsItemModel in ddsDnevnikModel.DetailItems)
            {
                if (ddsItemModel.DdsSuma != 0)
                {
                    selestitem = ddsItemModel.Code;
                }
            }
            if (KindDoc != null)
            {
                OnDdsSaved(new DdsEventArgs(KindActivity, true, selestitem, KindDoc.CodetId));
            }
            else
            {
                OnDdsSaved(new DdsEventArgs(KindActivity, true, selestitem,""));
            }
        }
        protected override void Delete()
        {
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                if (!Context.DeleteDdsDnevnicModel(ddsDnevnikModel.Id))
                {
                    MessageBoxWrapper.Show("Грешка при триене");
                }
                else
                {
                    OnRefreshExecuted(new DdsEventArgs(this.KindActivity));
                }
                
            }
        }

        protected override void Report()
        {
            var rd = new ReportDialog(this);
            rd.ShowDialog();
        }
        #region IReportBuilder
        public List<List<string>> GetItems()
        {
            List<List<string>> result = Context.GetDnevItem(KindActivity, ddsDnevnikModel.FromDate, ddsDnevnikModel.ToDate);
            return result;
        }
       
        public DateTime FromDate
        {
            get
            {
                return ddsDnevnikModel.FromDate;
            }

            set
            {
                ddsDnevnikModel.FromDate = value;
            }
        }

        
        public DateTime ToDate
        {
            get
            {
                return ddsDnevnikModel.ToDate;
            }

            set
            {
                ddsDnevnikModel.ToDate = value;
            }
        }
        public List<string> GetTitles()
        {
            var ret = new List<string>();
           
            return ret;
        }
        public List<string> GetHeader()
        {
            var ret = new List<string>();
            if (ddsDnevnikModel.FromDate.Month != ddsDnevnikModel.ToDate.Month)
            {
                ret.Add(String.Format("За период           : от месец {0} до месец {1}", Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[ddsDnevnikModel.FromDate.Month-1],
                                                                             Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[ddsDnevnikModel.ToDate.Month-1]));
            }
            else
            {
                ret.Add(String.Format("За месец            : {0} година {1}",Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[ddsDnevnikModel.ToDate.Month - 1], ddsDnevnikModel.ToDate.Year));
            }
            ret.Add(String.Format("Дата на извлечението: {0}",DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}",ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}", Entrence.UserName));
            ret.Add(String.Format("ИД.Номер по ЗДДС    : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.DDSnum));
            ret.Add(String.Format("Данъчен период      : {0}/{1}",ddsDnevnikModel.Month, ddsDnevnikModel.Year));
            return ret;
            
        }
        public List<string> GetFuther()
        {
            var ret = new List<string>();
            ret.Add("Съставил (имена и подпис):                               ");
            ret.Add("Главен Счетоводител (имена и подпис):                    ");
            ret.Add("Управител (имена и подпис):                              ");
            return ret;
        }
        public string Filename
        {
            get {if (ddsDnevnikModel.KindActivity == 1) return "DdsDnevPok";
                return "DdsDnevProd"; }
        }
        private IEnumerable<ReportItem> _reportItems;
        private bool _canSaveDds;
        private SaldoItem _selectedItem;
        private bool _kor;
        private string _oldsum;

        public IEnumerable<ReportItem> ReportItems
        {
            get
            {
                if (_reportItems == null)
                {
                    List<ReportItem> items = new List<ReportItem>();
                    List<int> hs = new List<int> { 5, 2, 3, 12, 11,
                                            12, 15, 18, 4, 12,
                                            11, 11, 11, 12, 11,
                                            11, 11, 11, 11, 11,
                                            11, 11, 11, 11, 11,
                                            11,6, 3, 3 };
                    if (KindActivity == 1)
                    {
                        hs = new List<int> { 5, 2, 3, 12, 11,
                                            12, 15, 18, 4, 12,
                                            11, 11, 11, 11, 11,
                                            11,6, 3, 3 };
                    }
                    List<string> ret=new List<string>();
                    ret.Add("ПОРЕДЕН НОМЕР");//1
                    ret.Add("Клон");         //2
                    ret.Add("В И Д");//3
                    ret.Add("НОМЕР НА ДОКУМЕНТА");//4
                    ret.Add("ДАТА НА ДОКУМЕНТА");//5
                    ret.Add("ИДЕНТИФИКАЦ.");//6
                    ret.Add("ИМЕ НА КОНТРАГЕНТА");//7
                    ret.Add("ВИД НА СТОКА/УСЛУГА");//8
                    ret.Add("Дост чл 163а или внос чл.167а");//8а
                    if (KindActivity == 2)
                    {
                        ret.Add("ОБЩО ДО ЗА ОБЛАГАНЕ С ДДС");//9
                        ret.Add("ВСИЧКО ДДС");//10
                        ret.Add("ДО НА СДЕЛКИ В СТРАНАТА 20%");//11
                        ret.Add("НАЧИСЛЕН ДДС 20%");//12
                        ret.Add("ДО НА ВОП");//13
                        ret.Add("ДО НА ПОЛУЧ. ДОСТАВКИ ЧЛ82 АЛ.2-5 ЗДДС");//14
                        ret.Add("НАЧИСЛЕН ДДС20% ЗА ВОП И ДОСТ.ЧЛ82 АЛ. 2-5");//15
                        ret.Add("НАЧИСЛЕН ДДС(20%) ЛИЧНИ НУЖДИ");//16
                        ret.Add("ДО НА ОБЛАГАЕМИТЕ ДОСТАВКИ СЪС СТАВКА 9%");//17
                        ret.Add("НАЧИСЛЕН ДДС ЗА ДОСТАВКИТЕ КОЛ.17");//18
                        ret.Add("ДО НА СДЕЛКИ СЪС СТАВКА 0% ГЛАВА 3");//19
                        ret.Add("ДО НА СДЕЛКИ СЪС СТАВКА 0% ВОД");//20
                        ret.Add("ДО НА СДЕЛКИ СТАВКА 0% ПО ЧЛ.140,146,173");//21
                        ret.Add("ДО ДОСТ.,УСЛ. ПО ЧЛ.21(2) ЗДДС С ДРУГ. ДЪРЖ. ЧЛЕНКА");//22
                        ret.Add("ДО НА ДОСТ. ПО ЧЛ.69 ДИСТ. ПРОДАЖ. ДР.ДЪРЖ.");//23
                        ret.Add("ДО НА ОСВ.ДОСТАВКИ И ОСВ. ВОП");//24
                        ret.Add("ДО НА ДОСТ. КАТО ПОСРЕДНИК");//25
                    }
                    if (KindActivity == 1)
                    {
                        ret.Add("ДО и Данък на Получените Доставки, ВОП, Получените Доставки по чл.82, ал.2-6 от ЗДДС и Вносът без Право на Данъчен Кредит или без Данък");//9
                        ret.Add("ДО на Получените Доставки, ВОП, Получени Доставки по чл.82, ал.2-6 от ЗДДС, Вносът, Както и ДО на Получените Доставки, Използвани за Извършване на Доставки по чл.69, ал.2 от ЗДДС с Право на Пълен Данъчен Кредит");
                        ret.Add("ДДС с Право на Пълен Данъчен Кредит");//11
                        ret.Add("ДО на Получените Доставки, ВОП, Получени Доставки по чл.82, ал.2-6 от ЗДДС, Вносът, Както и ДО на Получените Доставки, Използвани за Извършване на Доставки по чл.69, ал.2 от ЗДДС с Право на Частичен Данъчен Кредит");
                        ret.Add("ДДС с Право на Частичен Данъчен Кредит");//13
                        ret.Add("Годишна Корекция по чл.73, ал.8 от ЗДДС (+/-)");//14
                        ret.Add("ДО при Придобиване на Стоки от Посредник в Тристранна Операция");//15
                    }
                    ret.Add("Конто номер");//26 or 16
                    ret.Add("Папка");//27 or 17
                    ret.Add("Обект");//28 or 18
                    int i = 0;
                    foreach (var item in ret)
                    {
                        if ((i > 8) && (i<ret.Count-3))
                        {
                            items.Add(new ReportItem {Name = item, Height = 10, IsShow = true, Width = hs[i],Sborno = true,IsSuma = true});
                        }
                        else
                        {
                            items.Add(new ReportItem { Name = item, Height = 10, IsShow = true, Width = hs[i]});
                        }
                        i++;
                        
                    }
                    if (KindActivity == 2)
                        {
                            items[14].IsShow = true;//Денкее Денкее гулема си
                            for(var k=17;k<26;k++) items[k].IsShow = false;

                        }
                    if (KindActivity ==1)
                    {
                        items[14].IsShow = false;
                    }
                    _reportItems = items;
                }
                return _reportItems;
            }
            set { _reportItems = value; }
        }
        public void GenerateReport()
        {
            Report();
        }
        public List<string> GetSubTitles()
        {
            List<string> result = new List<string>();
            result.Add("1");
            result.Add("2");
            result.Add("3");
            result.Add("4");
            result.Add("5");
            result.Add("6");
            result.Add("7");
            result.Add("8");
            result.Add("8a");
            result.Add("9");
            result.Add("10");
            if (KindActivity == 2)
            {
                result.Add("11");
                result.Add("12");
                result.Add("13");
                result.Add("14");
                result.Add("15");
                result.Add("16");
                result.Add("17");
                result.Add("18");
                result.Add("19");
                result.Add("20");
                result.Add("21");
                result.Add("22");
                result.Add("23");
                result.Add("24");
                result.Add("25");
                result.Add("26");
                
            }
            if (KindActivity == 1)
            {
                result.Add("11");
                result.Add("12");
                result.Add("13");
                result.Add("14");
                result.Add("15");
                result.Add("16");
                
            }
            
            //if (KindActivity != 0)
            //{
            //    var res=context.GetAllDnevItems(KindActivity);
            //    foreach (DdsItemModel ddsItemModel in res)
            //    {
            //        result.Add(ddsItemModel.Name);
            //    }
            //}
            return result;
        }
        public List<List<string>> GetTXTAntetka()
        {
            var ret = new List<List<string>>();
            bool razmina = true; 
            int k = 0;
            int step = 1;
            List<ReportItem> report=new List<ReportItem>(ReportItems); 
            while (razmina) 
            {
                razmina = false;
                List<string> row = new List<string>();
                int i = 0;
                foreach (var title in ReportItems)
                {
                    k = report[i].Width * step ;
                    if (title.Name.Length > k-report[i].Width)
                    {
                        if (k <= title.Name.Length)
                        {
                            row.Add(title.Name.Substring(k - report[i].Width, report[i].Width));
                            razmina = true;
                        }
                        else
                        {
                            row.Add(title.Name.Substring(k-report[i].Width,title.Name.Length-(k-report[i].Width)));
                        }
                    }
                    else
                    {
                        row.Add("");
                    }
                    i++;
                    
                }
                ret.Add(row);
                step++;
            }

            
            return ret;
        }
        #endregion

        public SaldoItem SelectedItem
        {
            get { return _selectedItem; }
            set { 
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("Bustad");
                OnPropertyChanged("DdsId");
                if (value != null && value.LiD>0) ddsDnevnikModel.LookupElementID = value.LiD;
                CanSave();
            }
        }

        internal void RaiseCancel()
        {
            OnDdsSaved(new DdsEventArgs(KindActivity,false));
        }

        public string Error
        {
            get { return null; }
        }

        public string ClName {
            get
            {
                return ddsDnevnikModel.NameKontr;
            }
            set
            {
                ddsDnevnikModel.NameKontr = value;
                OnPropertyChanged("ClName");
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "DocID":
                        if (!this.ValidString(this.DocID))
                            error = "Задължително поле";
                        break;
                    case "Lookup":
                        if (Lookup == null)
                            error = "Изберeте тип номенклатура";
                        break;
                    case "SelectedItem":
                        if (SelectedItem == null)
                            error = "Изберете елемент от номенклатурата";
                        break;
                    case "DdsId":
                        if (SelectedItem != null)
                        {
                            if (string.IsNullOrWhiteSpace(SelectedItem.Vat)) error = "Изберете елемент от номенклатурата";
                        }
                        else
                        {
                             error = "Изберете елемент от номенклатурата";
                        }
                    break;
                    
                
                }
                return error;

            }
        }

        internal void Refresh(FastLookupEventArgs e)
        {
            
            SelectedItem = e.SaldoItem;
            ClName = e.SaldoItem.Lookupval;
          
        }

        internal void SaveFromOutside()
        {
            Save();
        }

        internal void OnCancelDdsExecutedOut(DdsCancelEventArgs ddsEventArgs)
        {
            if (CancelSave != null)
            {
                CancelSave(this, ddsEventArgs);
            }
        }
    }

}