using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using ReportBuilder;
using Tempo2012.UI.WPF.Views.ReportManager;
using ReportItem = ReportBuilder.ReportItem;
using System.Threading;

namespace Tempo2012.UI.WPF.ViewModels.Dnevnici
{
    public class DdsSallesViewModel : BaseViewModel, IReportBuilder
    {
        public string Title
        {
            get { return ddsDnevnikModel.Title; }
        }
        public DdsSallesViewModel(DdsDnevnikModel ddsmodel)
        {
            ddsDnevnikModel = ddsmodel;
            KindDocLookup = new ObservableCollection<LookUpSpecific>(context.GetAllDocTypes());
            SelectedLookup = new ObservableCollection<ContragentInfo>();
            ActivityTypeLookup = new ObservableCollection<LookUpSpecific>
                                {
                                    new LookUpSpecific{CodetId = "01",Id=1,Name = "Покупки"},
                                    new LookUpSpecific{CodetId = "02",Id=2,Name = "Продажби"},
                                    new LookUpSpecific{CodetId = "03",Id=3,Name = "Други"},
                                };
            this.Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups(" where NAMEENG='k'"));

            AllFields = new ObservableCollection<DdsDnevnicItem>();
            foreach (var items in ddsDnevnikModel.DetailItems)
            {
                AllFields.Add(new DdsDnevnicItem(items));
            }
            if (ddsmodel.LookupID > 0)
            {
                Lookup = Lookups.FirstOrDefault(e => e.Id == ddsmodel.LookupID);
            }
            if (ddsmodel.LookupElementID > 0)
            {
                LookupElementInfo = SelectedLookup.FirstOrDefault(e => e.Id == ddsmodel.LookupElementID);
            }

        }
        public DdsSallesViewModel(DdsDnevnikModel ddsmodel, DdsDnevnicItem item)
        {
            ddsDnevnikModel = ddsmodel;
            KindDocLookup = new ObservableCollection<LookUpSpecific>(context.GetAllDocTypes());
            KindDoc = KindDocLookup.FirstOrDefault(e => e.CodetId == ddsDnevnikModel.CodeDoc);
            SelectedLookup = new ObservableCollection<ContragentInfo>();
            ActivityTypeLookup = new ObservableCollection<LookUpSpecific>
                                {
                                    new LookUpSpecific{CodetId = "01",Id=1,Name = "Покупки"},
                                    new LookUpSpecific{CodetId = "02",Id=2,Name = "Продажби"},
                                    new LookUpSpecific{CodetId = "03",Id=3,Name = "Други"},
                                };
            this.Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups(" where NAMEENG='k'"));

            AllFields = new ObservableCollection<DdsDnevnicItem>();
            foreach (var litem in ddsDnevnikModel.DetailItems)
            {
                if (litem.Name == item.Name)
                {
                    litem.DdsSuma = item.DdsSuma;
                }
                AllFields.Add(new DdsDnevnicItem(litem));
            }
            if (ddsmodel.LookupID > 0)
            {
                Lookup = Lookups.FirstOrDefault(e => e.Id == ddsmodel.LookupID);
            }
            if (ddsmodel.LookupElementID > 0)
            {
                LookupElementInfo = SelectedLookup.FirstOrDefault(e => e.Id == ddsmodel.LookupElementID);
            }

        }
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; OnPropertyChanged("Info"); }
        }

        private DdsDnevnikModel ddsDnevnikModel;
        public ObservableCollection<LookUpSpecific> KindDocLookup { get; set; }
        private LookUpSpecific _currentKindDoc;
        public LookUpSpecific CurrentKindDoc
        {
            get { return _currentKindDoc; }
            set { _currentKindDoc = value; OnPropertyChanged("CurrentKindDoc"); }
        }

        private LookUpSpecific _currentActivity;
        public LookUpSpecific CurrentActivity
        {
            get { return _currentActivity; }
            set
            {
                _currentActivity = value;
                OnPropertyChanged("LookUpSpecific");
            }
        }

        private LookUpMetaData _lookUpMetaData;
        public LookUpMetaData Lookup
        {
            get { return _lookUpMetaData; }
            set
            {
                _lookUpMetaData = value;
                SelectedLookup = new ObservableCollection<ContragentInfo>();
                var list = context.GetLookup(_lookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                foreach (List<string> li in list)
                {
                    if (li != null && li.Count > 5)
                    {
                        SelectedLookup.Add(new ContragentInfo { Id = int.Parse(li[1]), Name = li[2], Bulstad = li[4], Nzdds = li[5] });

                    }
                }
                ddsDnevnikModel.LookupID = _lookUpMetaData.Id;
                OnPropertyChanged("Lookup");
                OnPropertyChanged("SelectedLookup");

            }
        }

        private ContragentInfo _lookupElementInfo;
        public ContragentInfo LookupElementInfo
        {
            get
            {
                return _lookupElementInfo;
            }
            set
            {
                if (value == null) return;
                _lookupElementInfo = value;
                _info = string.Format("БУЛСТАТ {0},NЗДДС {1}", value.Bulstad, value.Nzdds);
                ddsDnevnikModel.LookupElementID = value.Id;
                ddsDnevnikModel.Bulstat = value.Bulstad;
                ddsDnevnikModel.Nzdds = value.Nzdds;
                ddsDnevnikModel.NameKontr = value.Name;
                OnPropertyChanged("LookupElementInfo");
                OnPropertyChanged("Info");
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
        public int A8
        {
            get { return ddsDnevnikModel.A8; }
            set
            {
                ddsDnevnikModel.A8 = value;
                OnPropertyChanged("A8");
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


        public ObservableCollection<ContragentInfo> SelectedLookup { get; set; }

        protected override void Save()
        {
            context.SaveDdsDnevnicModel(ddsDnevnikModel);
        }
        protected override void Delete()
        {
            if (MessageBox.Show("Сигурен ли си ,че искаш да изтриеш този запис?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (context.DeleteDdsDnevnicModel(ddsDnevnikModel.Id))
                {
                    MessageBox.Show("Записа е изтрит");
                }
                else
                {
                    MessageBox.Show("Грешка при триене");
                }
            }
        }

        protected override void Report()
        {
            var rd = new ReportDialog(this);
            rd.ShowDialog();
        }

        public List<List<string>> GetItems()
        {
            List<List<string>> result = context.GetDnevItem(KindActivity, ddsDnevnikModel.Month, ddsDnevnikModel.Year);
            return result;
        }

        public List<string> GetTitles()
        {
            var ret = new List<string>();
            ret.Add("ПОРЕДЕН НОМЕР");//1
            ret.Add("Клон");         //2
            ret.Add("В И Д");//3
            ret.Add("НОМЕР НА ДОКУМЕНТА");//4
            ret.Add("ДАТА НА ДОКУМЕНТА");//5
            ret.Add("ИДЕНТИФИКАЦ.");//6
            ret.Add("ИМЕ НА КОНТРАГЕНТА");//7
            ret.Add("ВИД НА СТОКА/УСЛУГА");//8
            ret.Add("Дост чл 163а");//8а
            if (KindActivity == 2)
            {
                ret.Add("ОБЩО ДО ЗА ОБЛАГАНЕ С ДДС");//9
                ret.Add("ВСИЧКО ДДС");//10
                ret.Add("ДО НА СДЕЛКИ В СТРАНАТА 20%");//11
                ret.Add("НАЧИСЛЕН ДДС 20%");//12
                ret.Add("ДО НА ВОП");//13
                ret.Add("ДО НА ПОЛУЧ. ДОСТАВКИ ЧЛ82 АЛ.2-5 ЗДДС");//14
                ret.Add("НАЧИСЛЕН ДДС20% ЗА ВОП И ДОСТ.ЧЛ82 АЛ. 2-5");//15
                ret.Add("НАЧИСЛЕН ДДС(20%) ДРУГИ СЛУЧАИ");//16
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
                ret.Add("ДО и Данък на Получените Доставки, ВОП, Получените Доставки по чл.82, ал.2-5 от ЗДДС и Вносът без Право на Данъчен Кредит или без Данък");//9
                ret.Add("ДО на Получените Доставки, ВОП, Получени Доставки по чл.82, ал.2-5 от ЗДДС, Вносът, Както и ДО на Получените Доставки, Използвани за Извършване на Доставки по чл.69, ал.2 от ЗДДС с Право на Пълен Данъчен Кредит");
                ret.Add("ДДС с Право на Пълен Данъчен Кредит");//11
                ret.Add("ДО на Получените Доставки, ВОП, Получени Доставки по чл.82, ал.2-5 от ЗДДС, Вносът, Както и ДО на Получените Доставки, Използвани за Извършване на Доставки по чл.69, ал.2 от ЗДДС с Право на Частичен Данъчен Кредит");
                ret.Add("ДДС с Право на Частичен Данъчен Кредит");//13
                ret.Add("Годишна Корекция по чл.73, ал.8 от ЗДДС (+/-)");//14
                ret.Add("ДО при Придобиване на Стоки от Посредник в Тристранна Операция");//15
            }
            return ret;
        }

        public List<string> GetHeader()
        {
            var ret = new List<string>();
            ret.Add(String.Format("За месец            : {0} година {1}",Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[ddsDnevnikModel.Month-1], ddsDnevnikModel.Year));
            ret.Add(String.Format("Дата на извлечението: {0}",DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}",ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}",ConfigTempoSinglenton.GetInstance().CurrentFirma.Names));
            ret.Add(String.Format("ИД.Номер по ЗДДС    : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.DDSnum));
            ret.Add(String.Format("Даначеn период      : {0}/{1}",ddsDnevnikModel.Month, ddsDnevnikModel.Year));
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
            get { return "DDSDNEV"; }
        }

        private IEnumerable<ReportItem> _reportItems;
        public IEnumerable<ReportItem> ReportItems
        {
            get
            {
                if (_reportItems == null)
                {
                    List<ReportItem> items = new List<ReportItem>();
                    List<int> hs = new List<int> { 16, 4, 2, 20, 11, 16, 50, 30, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };
                    int i=0;
                    foreach (var item in GetTitles())
                    {
                        items.Add(new ReportItem { Name = item, Height = 10, IsShow = true, Width = hs[i]});
                        i++;
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
            }
            if (KindActivity == 1)
            {
                result.Add("11");
                result.Add("12");
                result.Add("13");
                result.Add("14");
                result.Add("15");
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
            int maxlen = 0;
            bool razmina = true; 
            int k = 0;
            int step = 1;
            List<ReportItem> report=new List<ReportItem>(ReportItems); 
            while (razmina) 
            {
                razmina = false;
                List<string> row = new List<string>();
                int i = 0;
                foreach (var title in GetTitles())
                {
                    k = report[i].Width * step ;
                    if (title.Length > k-report[i].Width)
                    {
                        if (k <= title.Length)
                        {
                            row.Add(title.Substring(k - report[i].Width, report[i].Width));
                            razmina = true;
                        }
                        else
                        {
                            row.Add(title.Substring(k-report[i].Width,title.Length-(k-report[i].Width)));
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
    }

}