using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Tempo2012.UI.WPF.ViewModels.Deklar
{
    public class DeclarConfigViewModel:BaseViewModel
    {
        public DeclarConfigViewModel()
        {
            model = ConfigTempoSinglenton.GetInstance().DeclarConfig;
            _year = DateTime.Now.Year;
            _month = DateTime.Now.Month;
        }
        private DeclarConfigModel model;
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year");}
        }
        private int _month;
        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged("Month");}
        }
        public Decimal Kl33 
        {
            get { return model.Kl33; }
            set { model.Kl33 = value; OnPropertyChanged("Kl33"); }
        }
        public Decimal Kl70
        {
            get { return model.Kl70; }
            set { model.Kl70 = value; OnPropertyChanged("Kl70");  }
        }
        public Decimal Kl71
        {
            get { return model.Kl71; }
            set { model.Kl71 = value; OnPropertyChanged("Kl71"); }
        }
        public Decimal Kl80
        {
            get { return model.Kl80; }
            set { model.Kl80 = value; OnPropertyChanged("Kl80"); }
        }
        public Decimal Kl81
        {
            get { return model.Kl81; }
            set { model.Kl81 = value; OnPropertyChanged("Kl81"); }
        }
        public Decimal Kl82
        {
            get { return model.Kl82; }
            set { model.Kl82 = value; OnPropertyChanged("Kl82"); }
        }
        protected override void Add()
        {
            Dictionary<string, string> declar = new Dictionary<string, string>();
            Declar d = new Declar(model,
                                context.GetPokupki(Month,Year),
                                context.GetProdazbi(Month,Year));

            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            declar.Add("data", DateTime.Now.ToShortDateString());
            declar.Add("firma", firma.Name);
            declar.Add("adress", firma.Address);
            declar.Add("tel", firma.Tel);
            declar.Add("dds", firma.DDSnum);
            declar.Add("iddds", firma.Bulstad);
            declar.Add("kl01", d.Kl01.ToString(Vf.LevFormatUI));
            declar.Add("kl11", d.Kl11.ToString(Vf.LevFormatUI));
            declar.Add("kl12", d.Kl12.ToString(Vf.LevFormatUI));
            declar.Add("kl13", d.Kl13.ToString(Vf.LevFormatUI));
            declar.Add("kl14", d.Kl14.ToString(Vf.LevFormatUI));
            declar.Add("kl15", d.Kl15.ToString(Vf.LevFormatUI));
            declar.Add("kl16", d.Kl16.ToString(Vf.LevFormatUI));
            declar.Add("kl17", d.Kl17.ToString(Vf.LevFormatUI));
            declar.Add("kl18", d.Kl18.ToString(Vf.LevFormatUI));
            declar.Add("kl19", d.Kl19.ToString(Vf.LevFormatUI));

            declar.Add("kl20", d.Kl20.ToString(Vf.LevFormatUI));
            declar.Add("kl21", d.Kl12.ToString(Vf.LevFormatUI));
            declar.Add("kl22", d.Kl22.ToString(Vf.LevFormatUI));
            declar.Add("kl23", d.Kl23.ToString(Vf.LevFormatUI));
            declar.Add("kl24", d.Kl24.ToString(Vf.LevFormatUI));

            declar.Add("kl30", d.Kl30.ToString(Vf.LevFormatUI));
            declar.Add("kl31", d.Kl31.ToString(Vf.LevFormatUI));
            declar.Add("kl32", d.Kl32.ToString(Vf.LevFormatUI));
            declar.Add("kl33", d.Kl33.ToString(Vf.LevFormatUI));

            declar.Add("kl40", d.Kl40.ToString(Vf.LevFormatUI));
            declar.Add("kl41", d.Kl41.ToString(Vf.LevFormatUI));
            declar.Add("kl42", d.Kl40.ToString(Vf.LevFormatUI));
            declar.Add("kl43", d.Kl41.ToString(Vf.LevFormatUI));

            declar.Add("kl50", d.Kl50.ToString(Vf.LevFormatUI));
            declar.Add("kl60", d.Kl60.ToString(Vf.LevFormatUI));
            declar.Add("kl70", d.Kl70.ToString(Vf.LevFormatUI));
            declar.Add("kl71", d.Kl71.ToString(Vf.LevFormatUI));

            declar.Add("kl80", d.Kl80.ToString(Vf.LevFormatUI));
            declar.Add("kl81", d.Kl81.ToString(Vf.LevFormatUI));
            declar.Add("kl82", d.Kl82.ToString(Vf.LevFormatUI));

            declar.Add("DateTimeNow", DateTime.Now.ToShortDateString());
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = DeklarviewModel.ReturnDeklar(declar);
            var path = AppDomain.CurrentDomain.BaseDirectory + "Deklar1.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(actual);
            }
            Process.Start(path);
        }
        protected override void Save()
        {
            ConfigTempoSinglenton.GetInstance().SaveConfiguration();
        }
        protected override void AddNew()
        {
            Declar d = new Declar(model,
                                context.GetPokupki(Month,Year),
                                context.GetProdazbi(Month,Year));

            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,-15}", firma.DDSnum);                   //1
            sb.AppendFormat("{0,-50}", firma.NameBoss);                 //2
            sb.AppendFormat("{0,-4}",Year);     //3
            sb.AppendFormat("{0,-2}",Month);   //3
            sb.AppendFormat("{0,-50}",firma.Names);                     //4
            sb.AppendFormat("{0,-15}", 1);                              //5 ?
            sb.AppendFormat("{0,-15}", 1);                              //6 ?
            sb.AppendFormat("{0,-15}", d.Kl01);                         //7
            sb.AppendFormat("{0,-15}", d.Kl20);                         //8
            sb.AppendFormat("{0,-15}", d.Kl11);                         //9
            sb.AppendFormat("{0,-15}", d.Kl21);                         //10
            sb.AppendFormat("{0,-15}", d.Kl12);                         //11
            sb.AppendFormat("{0,-15}", d.Kl22);                         //12
            sb.AppendFormat("{0,-15}", d.Kl23);                         //13
            sb.AppendFormat("{0,-15}", d.Kl13);                         //14
            sb.AppendFormat("{0,-15}", d.Kl24);                         //15
            sb.AppendFormat("{0,-15}", d.Kl14);                         //16
            sb.AppendFormat("{0,-15}", d.Kl15);                         //17
            sb.AppendFormat("{0,-15}", d.Kl16);                         //18
            sb.AppendFormat("{0,-15}", d.Kl17);                         //19
            sb.AppendFormat("{0,-15}", d.Kl18);                         //20
            sb.AppendFormat("{0,-15}", d.Kl19);                         //21
            //pokupki
            sb.AppendFormat("{0,-15}", d.Kl30);                         //22
            sb.AppendFormat("{0,-15}", d.Kl31);                         //23
            sb.AppendFormat("{0,-15}", d.Kl41);                         //24
            sb.AppendFormat("{0,-15}", d.Kl32);                         //25
            sb.AppendFormat("{0,-15}", d.Kl42);                         //26
            sb.AppendFormat("{0,-15}", d.Kl43);                         //27
            //resultat
            sb.AppendFormat("{0,-15}", d.Kl33);                         //28
            sb.AppendFormat("{0,-15}", d.Kl40);                         //29
            sb.AppendFormat("{0,-15}", d.Kl50);                         //30
            sb.AppendFormat("{0,-15}", d.Kl60);                         //31
            sb.AppendFormat("{0,-15}", d.Kl70);                         //32
            sb.AppendFormat("{0,-15}", d.Kl71);                         //33
            sb.AppendFormat("{0,-15}", d.Kl80);                         //34
            sb.AppendFormat("{0,-15}", d.Kl81);                         //35
            sb.AppendFormat("{0,-15}", d.Kl81);                         //36

            var path = AppDomain.CurrentDomain.BaseDirectory + "Deklar.txt";
            Encoding srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte [] originalByteString = srcEncodingFormat.GetBytes(sb.ToString());
            byte [] convertedByteString = Encoding.Convert(srcEncodingFormat,dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(finalString);
            }
            Process.Start(path);
        }
    }
}
