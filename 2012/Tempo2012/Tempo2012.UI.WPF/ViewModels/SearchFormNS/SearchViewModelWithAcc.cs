using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;

namespace Tempo2012.UI.WPF.ViewModels.SearchFormNS
{
    public class SearchViewModelWithAcc : BaseViewModel, IReportBuilder
    {
        private int vid;

        public SearchViewModelWithAcc()
        {
            AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
           //AllConto =
            //new ObservableCollection<Conto>(context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, 0, 30));
            //RefreshConto(AllConto);
            DataTypeForm = DateType.IsDateIn;
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Запис", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Номер", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Обект", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Папка", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата осчет.", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит c-ka", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит c-ka", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот", Width = 12, Sborno = true });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Кт", Width = 12, IsSuma = true });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Кт", Width = 12, IsSuma = true });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Дт", Width = 12, IsSuma = true });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Кт", Width = 12, IsSuma = true });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Дт", Width = 12, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Основание", Width = 30 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сделка", Width = 10 });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит Детайли", Width = 100 });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Забележка", Width = 20 });
            //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата на документ", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Контрагент", Width = 40 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "ЗДДС НОМЕР", Width = 15 });
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "ВИД ДОКУМЕНТ", Width = 7});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "НОМЕР ФАКТУРА", Width = 15});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "ДАТА ФАКТУРА", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "ДДС", Width = 15,Sborno = true});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сума Сделка", Width = 15,Sborno = true});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Общо", Width = 15, Sborno = true });
            reportItems.Add(new ReportItem { Height = 20, IsShow = true, Name = "Признак 1", Width = 10 });
            reportItems.Add(new ReportItem { Height = 20, IsShow = true, Name = "Признак 2", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит Детайли", Width = 100 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит Детайли", Width = 100 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Потребител", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Транзакция", Width = 12 });
            ReportItems = reportItems;
            
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }
        public SearchViewModelWithAcc(int p):this()
        {
            // TODO: Complete member initialization
            vid = p;
        }
        public DateType DataTypeForm { get; set; }

        public ObservableCollection<AccountsModel> AllAccountsK { get; set; }

        public DateTime FromDate
        {
            get
            {
                return Entrence.Mask.FromDate;
            }

            set
            {
                Entrence.Mask.FromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get
            {
                return Entrence.Mask.ToDate;
            }

            set
            {
                Entrence.Mask.ToDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public List<List<string>> GetItems()
        {
            List<string> newitem = null;
            List<List<string>> result = new List<List<string>>();
            var allconto = Context.GetAllContoWithDds(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask,Tipdnev);
            foreach (var conto in allconto.OrderBy(e=>e.Data))
            {
                newitem = new List<string>();
                newitem.Add(conto.Nd.ToString());
                newitem.Add(conto.DocNum);
                newitem.Add(conto.NumberObject.ToString());
                newitem.Add(conto.Folder);
                newitem.Add(conto.Data.ToShortDateString());
                var firstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.DebitAccount);
                newitem.Add(firstOrDefault != null ? firstOrDefault.Short.Trim(' ') : "");
                var accountsModel = AllAccountsK.FirstOrDefault(e => e.Id == conto.CreditAccount);
                newitem.Add(accountsModel != null ? accountsModel.Short.Trim(' ') : "");

                newitem.Add(conto.Oborot.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Reason);
                string pok = "";
                if (conto.IsPurchases == 1)
                {
                    pok = conto.VopPurchases;
                }
                if (conto.IsSales == 1)
                {
                    pok = conto.VopSales;
                }
                if (conto.IsPurchases == 1 && conto.IsSales == 1)
                {
                    pok = string.Format("{0}/{1}", conto.VopPurchases, conto.VopSales);
                }
                newitem.Add(pok); 
                //newitem.Add(conto.Note);
                //newitem.Add(conto.DataInvoise.ToShortDateString());
                newitem.Add(conto.Contragent);
                newitem.Add(conto.Vat);
                newitem.Add(conto.KindDoc);
                newitem.Add(conto.NomInvoise);
                newitem.Add(conto.DataInvoiseDnev);
                newitem.Add(conto.SumDds.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Sum.ToString(Vf.LevFormatUI));
                newitem.Add((conto.Sum+conto.SumDds).ToString(Vf.LevFormatUI));
                newitem.Add(conto.Pr1);
                newitem.Add(conto.Pr2);
                newitem.Add(conto.DDetails ?? "");
                newitem.Add(conto.CDetails ?? "");
                newitem.Add(conto.UserId.ToString());
                newitem.Add(conto.Id.ToString());
                result.Add(newitem);
                
            }
            return result;
        }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            var ret = new List<string>();
            ret.Add(String.Format("За период           : от {0} до {1}", Entrence.Mask.FromDate.ToShortDateString(), Entrence.Mask.ToDate.ToShortDateString()));
            ret.Add(String.Format("Дата на извлечението: {0}", DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}", Entrence.UserName));
            ret.Add(Entrence.Mask.ToString());
            return ret;
        }

        public List<string> GetFuther()
        {
            List<string> result = new List<string>();
            return result;
        }

        public string Filename
        {
            get { return "hronodds";}
        }

        public string Title
        {
            get { return Tipdnev==1?"Хронологичен р-р с дневниk покупки": "Хронологичен р-р с дневниk продажби"; }
        }

        public IEnumerable<ReportItem> ReportItems { get; set;}

        public List<string> GetSubTitles()
        {
            return new List<string>();
        }

        public List<List<string>> GetTXTAntetka()
        {
            return new List<List<string>>();
        }



        public int Tipdnev { get; set; }
    }
}
