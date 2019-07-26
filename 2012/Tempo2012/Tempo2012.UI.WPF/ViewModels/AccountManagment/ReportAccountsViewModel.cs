using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.ViewModels.AccountManagment
{
    public class ReportAccountsViewModel : BaseViewModel, IReportBuilder
    {

        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public ReportAccountsViewModel(): base()
        {
           
            AllAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            var _reportItems = new List<ReportItem>();
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Име", Width = 50 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Тип", Width = 20 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Ниво", Width = 20 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Опции", Width = 20 });
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
            List<List<string>> items=new List<List<string>>();
            List<string> newitem;
            Dictionary<int, string> types = new Dictionary<int, string> { { 0, "Грешка" },{ 1, "Активна" }, { 2, "Пасивна" }, { 3, "Активно-Пасивна" }, { 4, "Транзитивна" } };
            Dictionary<int, string> kinds = new Dictionary<int, string> { { 0, "Грешка" },{ 1, "Синтетично" }, { 2, "Аналитично" } };
            Dictionary<int, string> props = new Dictionary<int, string> { { 0, "Грешка" }, { 1, "Нормална" }, { 2, "Бюджетна" }, { 3, "Собстевени средства" } };
            foreach (AccountsModel account in AllAccounts)
            {
                newitem = new List<string> {account.Short, account.NameMain, types[account.TypeAccount],kinds[account.LevelAccount],props[account.TypeAccountEx]};
                items.Add(newitem);
                
            }
            newitem = new List<string> { "----------", "-----------", "", "", "" };
            items.Add(newitem);
            newitem = new List<string> {"Общо", AllAccounts.Count.ToString(), "", "",""};
            items.Add(newitem);
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
            get { return "IndAcc";}
        }


        public string Title
        {
            get;set; 
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

        protected override void Report()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }
    }
}
