using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{
    public class HoldingsWindowViewModel:BaseViewModel
    {
        public HoldingsWindowViewModel()
        {
            //string[] lines=null;
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "holdings.txt");
            //if (File.Exists(path))
            //{
            //    lines = File.ReadAllLines(path);
            //}
            //else
            //{
            //    lines =new string[]{ "Tehko|localhost|t" };
            //}
            //int i = 1;
            Holdings = new ObservableCollection<HoldingViewModel>();
            var hol = ConfigTempoSinglenton.GetInstance().Holdings;
            foreach (HoldingModel holdingModel in hol)
            {
                Holdings.Add(new HoldingViewModel(holdingModel));
            }
            //foreach (string line in lines)
            //{
            //    var splitline = line.Split('|');
            //    if (splitline.Length < 2) continue;
            //    HoldingViewModel item = new HoldingViewModel();
            //    item.Nom = i;
            //    item.Name = splitline[0].Trim();
            //    item.IpServer= splitline[1].Trim();
            //    item.Template=splitline[2].Trim();
            //    Holdings.Add(item);
            //    i++;
            //}
        }
        private HoldingViewModel _holding;
       
        public ObservableCollection<HoldingViewModel> Holdings { get; set; }

        public HoldingViewModel Holding
        {
            get { return _holding; }
            set
            {
                _holding = value;
                OnPropertyChanged("Holding");
                OnPropertyChanged("Name");
                OnPropertyChanged("IpServer");
                OnPropertyChanged("Template");
                OnPropertyChanged("ConnectionString");
            }
        }

        public string Name
        {
            get { if (_holding != null) return _holding.Name; return null;}
            set
            {
                if (_holding != null) _holding.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string IpServer
        {
            get { if (_holding != null) return _holding.IpServer;
                return null;
            }
            set
            {
                if (_holding != null) _holding.IpServer = value;
                OnPropertyChanged("IpServer");
            }
        }
        public string Template
        {
            get { if (_holding != null) return _holding.Template;
                return null;
            }
            set
            {
                if (_holding != null) _holding.Template = value;
                OnPropertyChanged("Template");
            }
        }
        public string ConnectionString
        {
            get
            {
                if (_holding != null) return _holding.ConnectionString;
                return null;
            }
            set
            {
                if (_holding != null) _holding.ConnectionString = value;
                OnPropertyChanged("ConnectionString");
            }
        }
        protected  override void Add()
        {
            HoldingViewModel holding =
                new HoldingViewModel(new HoldingModel {Name = "New", IpServer = "localhost", Nom = Holdings.Count + 1});
            var conf = ConfigTempoSinglenton.GetInstance();
            holding.ConnectionString = string.Format(Entrence.ConectionStringTemplate, holding.IpServer,conf.BaseDbPath, "H" + holding.Nom);
            Holdings.Add(holding);
            if (
                MessageBoxWrapper.Show("Копиране на празна база от темплейтите?", "Предупреждение",
                    MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                IoHelper.DirectoryCopy(ConfigTempoSinglenton.GetInstance().BaseTemplatePath,
                    Path.Combine(ConfigTempoSinglenton.GetInstance().BaseDbPath, "H" + holding.Nom), true);
            }
            foreach (var item in conf.ConfigNames)
            {
                var spliter = item.Split('|');
                FirmSettingModel newsett = new FirmSettingModel();
                newsett.Key = spliter[0];
                newsett.Name = spliter[1];
                newsett.Value = spliter[2];
                newsett.FirmaId = 1;
                newsett.HoldingId = holding.Nom;
                conf.FirmSettings.Add(newsett);
            }
            conf.SaveConfiguration();
        }

        protected override void Delete()
        {
            Holdings.Remove(Holding);
            Holding=Holdings.Last();
        }

        protected override void Update()
        {
            List<HoldingModel> hm=new List<HoldingModel>();
            foreach (HoldingViewModel holdingViewModel in Holdings)
            {
                hm.Add(holdingViewModel.HoldingModel);
            }
            var conf = ConfigTempoSinglenton.GetInstance();
            conf.Holdings = hm;
            conf.SaveConfiguration();
        }

    
    }
}
