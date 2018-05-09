using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Tetka
{
    public class ReportDebit:BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        private string _accShortName;
        private List<AccountsModel> Allacc;
        
        public ReportDebit()
        {
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сметка", Width = 30 });
            reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Сума Оборот", Width = 20,Sborno = true});
            ReportItems = reportItems;
            Allacc = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AccountsModel CurrenAcc { get; set; }
        public List<List<string>> GetItems()
        {
            return IsCredit?Context.GetCredit(FromDate, ToDate, CurrenAcc.Id, CurrenAcc.FirmaId):Context.GetDebit(FromDate, ToDate, CurrenAcc.Id, CurrenAcc.FirmaId);
        }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            List<string> list = new List<string>();
            list.Add(string.Format("за сметка                 : {0}", CurrenAcc));
            list.Add(string.Format("за период                 : {0} до {1}", FromDate.ToShortDateString(), ToDate.ToShortDateString()));
            list.Add(string.Format("Дата на извлечението      : {0}", DateTime.Now.ToShortDateString()));
            list.Add(string.Format("за фирма                  : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            list.Add(string.Format("Счетоводител              : {0}", Entrence.UserName));
            return list;
        }

        public List<string> GetFuther()
        {

            List<string> list = new List<string>();
            //list.Add("---------------------------------------------------------------");
            //list.Add("| Параметри на сметката       |    дебит      |   кредит      |");
            //list.Add("---------------------------------------------------------------");
            //list.Add(string.Format("| Начални салда               |{0,15}|{1,15}|", CurrenAcc.TypeAccount == 1 ? CurrenAcc.BeginSaldoL.ToString(Vf.LevFormatUI) : "", CurrenAcc.TypeAccount == 2 ? CurrenAcc.BeginSaldoL.ToString(Vf.LevFormatUI) : ""));
            //list.Add(string.Format("| Oбороти                     |{0,15}|{1,15}|", OborotsDebit, OborotsCredit));
            //list.Add(string.Format("| Сборове                     |{0,15}|{1,15}|", TotalD, TotalC));
            //list.Add(string.Format("| Крайни салда                |{0,15}|{1,15}|", CurrenAcc.TypeAccount == 1 ? CurrenAcc.EndSaldoL.ToString(Vf.LevFormatUI) : "", CurrenAcc.TypeAccount == 2 ? CurrenAcc.EndSaldoL.ToString(Vf.LevFormatUI) : ""));
            //list.Add("---------------------------------------------------------------");
            return list;
        }

        public string Filename
        {
            get { return IsCredit?"debitrep":"creditrep"; }
        }

        public string Title
        {
            get { return IsCredit ? "Корeспонденция по сметка кредит" : "Корeспонденция по сметка дебит"; }
        }

        public IEnumerable<ReportItem> ReportItems { get; set;}

        public List<string> GetSubTitles()
        {
            return null;
        }

        public List<List<string>> GetTXTAntetka()
        {
            return null;
        }

        public string AccShortName
        {
            get { return _accShortName; }
            set
            {
                _accShortName = value;
                if (!value.Contains("/"))
                {
                    int num;
                    if (int.TryParse(value, out num))
                    {
                        CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    }

                }
                else
                {
                    int num, subnum;
                    var ac = value.Split('/');

                    if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
                    {
                        CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                    }
                }
            }
        }

        public bool IsCredit { get; set; }
        
    }
}
