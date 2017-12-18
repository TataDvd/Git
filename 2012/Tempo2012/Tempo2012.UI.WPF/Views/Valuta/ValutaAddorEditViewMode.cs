using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Valuta
{
    public class ValutaAddorEditViewMode:BaseViewModel
    {
        public ValutaAddorEditViewMode()
        {
            Codes = new ObservableCollection<string>(Context.GetLookupByName("vk", "VIDVALUTA"));
            _todata = DateTime.Now.AddMonths(1);
            _fromdata = DateTime.Now.AddMonths(-1);
            _code = Codes[0];
            Caption = "Добави";
            Refresh();
            
        }

        private void Refresh()
        {
            Valuts = new ObservableCollection<ValutaEntityWraper>();
            foreach (var item in Context.GetCurRates(_code, _fromdata, _todata))
            {
                var litem = new ValutaEntityWraper(item);
                litem.Codes=new ObservableCollection<string>(Codes);
                Valuts.Add(litem);
            }
        }

        public ValutaAddorEditViewMode(DateTime dateTime,Decimal kurs):this()
        {
            _fromdata = dateTime;
            _kurs = kurs;
        }
        private decimal _kurs;
        public Decimal Kurs
        {
            get { return _kurs; }
            set { _kurs = value; OnPropertyChanged("Kurs");}
        }

        private DateTime _fromdata;
        public DateTime FromData
        {
            get { return _fromdata; }
            set { _fromdata = value;
                Refresh();
                OnPropertyChanged("FromData");OnPropertyChanged("Valuts");}
        }
        private DateTime _todata;
        public DateTime ToData
        {
            get { return _todata; }
            set { _todata = value; Refresh(); OnPropertyChanged("ToData"); OnPropertyChanged("Valuts"); }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; 
                Refresh();
                OnPropertyChanged("Code");OnPropertyChanged("Valuts");}
        }
        protected override void Add()
        {
            
            var valutaEntityWraper = Valuts.FirstOrDefault(e => Valuts != null && e.Date == Enumerable.Max(Valuts, i => i.Date).Date);
            if (valutaEntityWraper != null)
                Valuts.Add(new ValutaEntityWraper(new ValutaEntity { CodeVal = _code, Date = Valuts.Max(e => e.Date).Date.AddDays(1), Value = valutaEntityWraper.Value, State = 1 }));
            else
            {
                Valuts.Add(
                    new ValutaEntityWraper(new ValutaEntity {CodeVal = _code, Date = DateTime.Now, Value = 0, State = 1}));
            }
            OnPropertyChanged("Valuts");
        }

        protected override void AddNew()
        {
            if (Valuts != null)
                foreach (var valutaEntityWraper in Valuts)
                {
                    if (valutaEntityWraper.State == 1)
                    {
                        Context.SaveKurs(valutaEntityWraper.Date, valutaEntityWraper.CodeVal, valutaEntityWraper.Value);
                    }
                }
            Refresh();
            OnPropertyChanged("Valuts");
        }

        public ObservableCollection<string> Codes { get; set;}
        public ObservableCollection<ValutaEntityWraper> Valuts { get; set; }
        public ValutaEntity SelectedItem { get; set; }

        internal void SaveKursFromOutSide(ValutaEntityWraper emp)
        {
            Context.SaveKurs(emp.Date,emp.CodeVal,emp.Value);
            if (Valuts != null)
                foreach (var valutaEntityWraper in Valuts)
                {
                    if (valutaEntityWraper.State == 1)
                    {
                        Context.SaveKurs(valutaEntityWraper.Date, valutaEntityWraper.CodeVal, valutaEntityWraper.Value);
                    }
                }
            Refresh();
            OnPropertyChanged("Valuts");
        }

        internal void DeleteContextFromOutside(List<ValutaEntity> itemsfordelete)
        {
            Context.DeleteKurs(itemsfordelete); 
            Refresh();
            OnPropertyChanged("Valuts");
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set { _caption = value;OnPropertyChanged("Caption"); }
        }
    }
}
