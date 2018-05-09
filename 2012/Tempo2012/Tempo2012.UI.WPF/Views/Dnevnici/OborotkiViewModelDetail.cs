using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;
using ReportBuilder;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    public class OborotkiViewModelDetail:BaseViewModel,IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public OborotkiViewModelDetail() 
        {
            ToDate = DateTime.Now;
            FromDate = DateTime.Now.AddMonths(-1);
            ReportItems= new List<ReportItem>{
                    new ReportItem{IsShow=true, Width=20, Name="Номер на Сметка",Height=20},
                    new ReportItem{IsShow=true, Width=50, Name="Име на Сметка",Height=20},
                    new ReportItem{IsShow=true, Width=15, Name="Номер",Height=20},
                    new ReportItem{IsShow=true, Width=15, Name="Булстат",Height=20},
                    new ReportItem{IsShow=true, Width=50, Name="Контрагент",Height=20},
                    new ReportItem{IsShow=true, Width=15, Name="Начално салдо дебит",Height=20,Sborno = true},
                    new ReportItem{IsShow=true, Width=15, Name="Начално салдо кредит",Height=20,Sborno = true},
                    new ReportItem{IsShow=true, Width=15, Name="Оборот дебит",Height=20,Sborno = true},
                    new ReportItem{IsShow=true, Width=15, Name="Оборот кредит",Height=20,Sborno = true},
                    new ReportItem{IsShow=true, Width=15, Name="Крайно салдо дебит",Height=20,Sborno = true},
                    new ReportItem{IsShow=true, Width=15, Name="Крайно салдо кредит",Height=20,Sborno = true}
                };
            
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }

        private DateTime toDate;
        public DateTime ToDate { 
            get 
            { 
                return toDate; 
            } 
            set 
            { 
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        
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
                OnPropertyChanged("FromDate");
            }
        }

        public List<List<string>> GetItems()
        {
            if (AccString == "*")
            {
                return Context.GetOborotnaVedDetail(FromDate, ToDate);
            }
            else
            {
                var allacc = Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().ActiveFirma);
                var curacc = allacc.FirstOrDefault(e => e.Short == AccString);
                if (curacc != null)
                {
                    return Context.GetOborotnaVedDetail(FromDate, ToDate,curacc.Id);
                }
            }
            return null;
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
            return new List<string>();
        }

        public string Filename
        {
            get {return "oborotdetail";}
        }

        public string Title
        {
            get { return string.Format("Оборотна ведомост на фирма {0} от {1} до {2} по контрагенти", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name,FromDate.ToShortDateString(),ToDate.ToShortDateString()); }
        }

       
        public IEnumerable<ReportItem> ReportItems
        {
            get;

            set;

        }
        public string AccString { get; internal set; }

        public List<string> GetSubTitles()
        {
            return new List<string>();
        }

        public List<List<string>> GetTXTAntetka()
        {
            //var a = new List<List<string>>();
            //a.Add(new List<string>{ "Сметка                  ", "Начално салдо", "Начално салдо", "Оборот дебит", "Оборoт кредит", "Крайно салдо", "Крайно салдо"});
            //a.Add(new List<string>{ "                        ", "    дебит    ", "     кредит  ", "            ", "             ", "     дебит   ", "   кредит  " });
            return null;
        }
        protected override void Add()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }

    }
}
