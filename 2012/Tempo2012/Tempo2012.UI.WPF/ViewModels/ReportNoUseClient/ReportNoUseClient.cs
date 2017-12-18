using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.ViewModels.AccountManagment
{
    public class ReportNoUseClient : BaseViewModel, IReportBuilder
    {


        public ReportNoUseClient()
            : base()
        {
           
             var _reportItems = new List<ReportItem>();
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Име", Width = 50 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Булстат ", Width = 15,IsSuma=true});
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "ЗДДС номер", Width = 15 ,IsSuma=true});
            ReportItems = _reportItems;
            IsShowAdd = false;
            IsShowAddNew = false;
            IsShowDelete = false;
            IsShowEdit = false;
            IsShowNavigation = false;
            IsShowSearch = false;
            IsShowView = false;
            IsShowReport = true; 
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }
        public ObservableCollection<AccountsModel> AllAccounts { get; set; }

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
            List<List<string>> items=Context.GetUnusableClients();
            return items;
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
            get { return "UnusableClients";}
        }

        public string Title
        {
            get { return "Справка за клиенти не участващи в осчетоводяване"; }
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

        protected override void Report()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }

        public void DeleteAll()
        {
            Context.GetUnusableClients(true);
        }
    }
}
