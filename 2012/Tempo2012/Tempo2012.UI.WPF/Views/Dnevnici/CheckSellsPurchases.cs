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
    public class CheckSellsPurchases:BaseViewModel , IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public CheckSellsPurchases(DateTime fromDate,DateTime todate,int kinddds) 
        {
            ToDate = todate;
            FromDate =fromDate;
            KindDDS = kinddds;
            ReportItems = new List<ReportItem>{
                    new ReportItem{IsShow=true, Width=20, Name="Номер Фактура",Height=20},
                    new ReportItem{IsShow=true, Width=50, Name="Контрагент",Height=20},
                    new ReportItem{IsShow=true, Width=15, Name="Брой Повторения",Height=20},
                    
                };
            //}
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
               
            }
        }

        public int KindDDS { get; private set; }
       
        public List<List<string>> GetItems()
        {
            return Context.CheckSellsPurchases(FromDate, ToDate, KindDDS);
               
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
            get {return "oborot";}
        }

        public string Title
        {
            get {if (KindDDS==1) return string.Format("Проверка дублирани фактури дневник покупки за фирма {0} от {1} до {2}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name,FromDate.ToShortDateString(),ToDate.ToShortDateString()); 
                return string.Format("Проверка дублирани фактури дневник продажби за фирма {0} от {1} до {2}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name,FromDate.ToShortDateString(),ToDate.ToShortDateString()); }
        }

       
        public IEnumerable<ReportItem> ReportItems
        {
            get;

            set;

        }

        public List<string> GetSubTitles()
        {
            return new List<string>();
        }

        public List<List<string>> GetTXTAntetka()
        {
            
            return null;
        }
        

    }
}
