using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Periods
{
    public class PeriodsViewModel:BaseViewModel
    {
        public ObservableCollection<PeriodModel> Periods { get; set;}
        private PeriodModel _period;

        public PeriodModel Period
        {
            get {
                if (_period == null)
                    _period = new PeriodModel
                    { To = DateTime.Now,
                        Fr = DateTime.Now,
                        Holding = ConfigTempoSinglenton.GetInstance().ActiveHolding,
                        Firma = ConfigTempoSinglenton.GetInstance().ActiveFirma
                    };
                return _period; }
            set { _period = value;
                OnPropertyChanged("To");
                OnPropertyChanged("Fr");
 }
        }

        public string Title { get; set;}
        public DateTime To {
            get
            {
                return Period.To;
            }
            set { Period.To = value; OnPropertyChanged("To"); }
        }
        public DateTime Fr
        {
            get
            {
                return Period.Fr;
            }
            set {
                Period.Fr = value;
                OnPropertyChanged("Fr"); }
        }
        public PeriodsViewModel()
        {
            Title = string.Format("Забранени периоди за фирма {0}", Entrence.CurrentFirma.Name); 
            Periods=new ObservableCollection<PeriodModel>(ConfigTempoSinglenton.GetInstance().Periods.Where(e=>e.Firma==ConfigTempoSinglenton.GetInstance().ActiveFirma && e.Holding == ConfigTempoSinglenton.GetInstance().ActiveHolding));
            if (Periods.Count > 0)
            {
                Period = Periods[0];
            }
            
        }
        protected override void Add()
        {
            Periods.Add(new PeriodModel { Fr = Period.Fr, To = Period.To,Firma=Period.Firma,Holding=Period.Holding});
            OnPropertyChanged("Periods");
        }
        protected override void Save()
        {
            ConfigTempoSinglenton.GetInstance().Periods = new System.Collections.Generic.List<PeriodModel>(Periods);
            ConfigTempoSinglenton.GetInstance().SaveConfiguration();

        }

        protected override void Update()
        {
            Periods.Remove(Period);
        }
    }
}