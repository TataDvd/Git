using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.Views
{
    public class NumberFormatsViewModel:BaseViewModel
    {
        public NumberFormatsViewModel()
        {
            lv=Vf.LevFormatUI;
            kol=Vf.KolFormatUI;
            val=Vf.ValFormatUI;
            kurs=Vf.KursFormatUI;
        }
        private string lv;
        public string Lv {
            get { return lv; }
            set { lv = value;  OnPropertyChanged("Lv"); }
        }
        private string val;
        public string Val
        {
            get { return val; }
            set { val = value; Vf.ValFormatUI = value; OnPropertyChanged("Val"); }
        }
        private string kol;
        public string Kol
        {
            get { return kol; }
            set { kol = value; Vf.KolFormatUI = value; OnPropertyChanged("Kol"); }
        }
        private string kurs;
        public string Kurs
        {
            get { return kurs; }
            set { kurs = value; Vf.KursFormatUI = value; OnPropertyChanged("Kurs"); }
        }
        protected override void Add()
        {
            Vf.KolFormatUI = Kol;
            Vf.KursFormatUI = Kurs;
            Vf.ValFormatUI = Val;
            Vf.LevFormatUI = Lv;
            Context.SaveNuberFormats();
        }
    }
}
