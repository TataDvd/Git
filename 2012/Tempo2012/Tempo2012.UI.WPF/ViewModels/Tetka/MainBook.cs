using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Tetka
{
    public class MainBook:BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        private string _accShortName;
        private List<AccountsModel> Allacc;
        private bool _foreachAll;
        
        public MainBook()
        {
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сметка", Width = 30 });
            reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Дебит", Width = 20,IsSuma = true});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит", Width = 20,IsSuma = true});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сметка", Width = 30 });
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
            if (_foreachAll)
            {
                return AllaccEval();
            }
            if (_foreachGroup)
            {
                return GroupedAcc();
            }
            return OneAcc();
        }

        private List<List<string>> GroupedAcc()
        {
            var res = new List<List<string>>();
            foreach (var item in Allacc.Where(e=>e.Short.StartsWith(_accShortName.Replace("*",""))))
            {

                CurrenAcc = item;
                OneAcc(res);
                List<string> row = new List<string>();
                row.Add("-------------------");
                row.Add("-------------------");
                row.Add("-------------------");
                row.Add("-------------------");
                res.Add(row);
            }
            return res;
        }

        private List<List<string>> AllaccEval()
        {
            var res = new List<List<string>>();
            foreach (var item in Allacc)
            {
                
                CurrenAcc = item;
                OneAcc(res);
                List<string> row = new List<string>();
                row.Add("-------------------");
                row.Add("-------------------");
                row.Add("-------------------");
                row.Add("-------------------");
                res.Add(row);
            }
            return res;
        }

        private List<List<string>> OneAcc(List<List<string>> res=null)
        {
            OboronaVed v = Context.GetOborotnaVedSaldo(FromDate, CurrenAcc.Id, CurrenAcc.FirmaId);
            decimal totalc = 0, totald = 0;
            if (res==null) res = new List<List<string>>();
            List<string> row = new List<string>();
            row.Add(CurrenAcc.Short);
            row.Add("");
            row.Add("");
            row.Add("");
            res.Add(row);
            row = new List<string>();
            row.Add("");
            row.Add(CurrenAcc.TypeAccount == 1 ? (CurrenAcc.BeginSaldoL+(v.NSD-v.NSK)).ToString(Vf.LevFormatUI) : "0.00");
            row.Add(CurrenAcc.TypeAccount == 2 ? (CurrenAcc.BeginSaldoL+(v.NSK-v.NSD)).ToString(Vf.LevFormatUI)  : "0.00");
            row.Add("Начално салдо");
            res.Add(row);
            var k = Context.GetCredit(FromDate, ToDate, CurrenAcc.Id, CurrenAcc.FirmaId);
            foreach (List<string> list in k)
            {
                row = new List<string>();
                row.Add("");
                row.Add("0.00");
                row.Add(list[1]);
                row.Add(list[0]);
                totalc += decimal.Parse(list[1]);
                res.Add(row);
            }
            var d = Context.GetDebit(FromDate, ToDate, CurrenAcc.Id, CurrenAcc.FirmaId);
            foreach (List<string> list in d)
            {
                row = new List<string>();
                row.Add("");
                row.Add(list[1]);
                row.Add("0.00");
                row.Add(list[0]);
                totald += decimal.Parse(list[1]);
                res.Add(row);
            }
            row = new List<string>();
            row.Add("");
            row.Add(totald.ToString(Vf.LevFormatUI));
            row.Add(totalc.ToString(Vf.LevFormatUI));
            row.Add("Общо обороти");
            res.Add(row);
            row = new List<string>();
            row.Add("");
            row.Add(CurrenAcc.TypeAccount == 1 ? (totald - totalc + (CurrenAcc.BeginSaldoL+(v.NSD - v.NSK))).ToString(Vf.LevFormatUI) : "0.00");
            row.Add(CurrenAcc.TypeAccount == 2 ? (totalc - totald + (CurrenAcc.BeginSaldoL+(v.NSK - v.NSD))).ToString(Vf.LevFormatUI) : "0.00");
            row.Add("Крайно салдо");
            res.Add(row);
            return res;
        }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            List<string> list = new List<string>();
            list.Add(string.Format("за сметка                 : {0}", CurrenAcc!=null?CurrenAcc.ShortName:_accShortName));
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
            get { return "mainbook"; }
        }

        public string Title
        {
            get;set; //{ return "Главна Книга"; }
        }
        public string SubTitle
        {
            get; set;
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
                if (value=="*")
                {
                    _foreachAll = true;
                }
                if (value.Contains("*"))
                {
                    _foreachGroup = true;
                }
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




        bool _foreachGroup; 
    }
}
