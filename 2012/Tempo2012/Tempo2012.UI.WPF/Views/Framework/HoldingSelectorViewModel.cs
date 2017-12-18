using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{
    public class HoldingSelectorViewModel:BaseViewModel
    {
        private HoldingViewModel _holding;

        public HoldingSelectorViewModel()
        {
            //string[] lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"holdings.txt"));
            //int i = 1;
            Holdings=new ObservableCollection<HoldingViewModel>();
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
            //    Holdings.Add(item);
            //    i++;
            //}
        }
        public ObservableCollection<HoldingViewModel> Holdings { get; set;}

        public HoldingViewModel Holding
        {
            get { return _holding; }
            set { _holding = value; OnPropertyChanged("Holding"); }
        }
    }
}
