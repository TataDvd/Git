using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Tetka
{
    public class TetkaViewModel : BaseViewModel
    {
        private AccountsModel _acc;

        public TetkaViewModel(AccountsModel accountsModel)
        {
            _acc = accountsModel;
            if (_acc.TypeAccount == 1)
            {
                _beginDt = _acc.BeginSaldoL.ToString();
                _sumDt = _acc.OborotDL.ToString();
                _endDt =_acc.EndSaldoL.ToString();
            }
            else
            {
                _beginKt = _acc.BeginSaldoL.ToString();
                _sumKt = _acc.OborotKL.ToString();
                _endKt=_acc.EndSaldoL.ToString();
            }
            OborotDt=new ObservableCollection<string>();
            OborotKt=new ObservableCollection<string>();
            Context.GetAccMovent(_acc, OborotDt, OborotKt);
        }

        public string Title
        {
            get { return _acc.ShortName; }
        }

        private string _beginKt;
        public string BeginKt
        {
            get { return _beginKt; }
            set { _beginKt = value; OnPropertyChanged("BeginKt"); }
        }

        private string _beginDt;
        public string BeginDt
        {
            get { return _beginDt; }
            set { _beginDt = value;OnPropertyChanged("BeginDt"); }
        }

        private string _endKt;
        public string EndKt
        {
            get { return _endKt; }
            set { _endKt = value;OnPropertyChanged("EndKt"); }
        }

        private string _endDt;
        public string EndDt
        {
            get { return _endDt; }
            set { _endDt = value;OnPropertyChanged("EndDt"); }
        }

        private string _sumKt;
        public string SumKt
        {
            get { return _sumKt; }
            set { _sumKt = value;OnPropertyChanged("SumKt"); }
        }

        private string _sumDt;
        public string SumDt
        {
            get { return _sumDt; }
            set { _sumDt = value;OnPropertyChanged("SumDt"); }
        }

        public ObservableCollection<string> OborotKt{ get; set;}
        public ObservableCollection<string> OborotDt{ get; set;}
    }
}
