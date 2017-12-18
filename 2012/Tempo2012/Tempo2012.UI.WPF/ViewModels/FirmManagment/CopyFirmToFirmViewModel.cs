using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using MessageBox = System.Windows.MessageBox;

namespace Tempo2012.UI.WPF.ViewModels
{
    
    public class CopyAccsFromYeatToYearViewModel:BaseViewModel
    {

        public CopyAccsFromYeatToYearViewModel()
            : base()
        {
            CopyCommand = new DelegateCommand((o) => this.CopyAccFromYtoY(), (o) => this.CanCopyAccFromYtoY());
        }

        private bool CanCopyAccFromYtoY()
        {
            return (ToYear ==FromYear+1 );
        }

        private void CopyAccFromYtoY()
        {
            context.CopyAccFromYtoY(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, FromYear, ToYear);
        }

        public ICommand CopyCommand { get; private set;}

        private int _toYear;
        public int ToYear
        {
            get { return _toYear; }
            set { _toYear = value;OnPropertyChanged("ToYear"); }
        }

        private int _fromYear;
        public int FromYear
        {
            get { return _fromYear; }
            set { _fromYear = value;OnPropertyChanged("FromYear"); }
        }
    }
}
