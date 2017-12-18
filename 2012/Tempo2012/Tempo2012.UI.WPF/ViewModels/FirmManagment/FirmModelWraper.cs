using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.FirmManagment
{
    public class FirmModelWraper : BaseViewModel
    {
        private FirmModel _CurrentFirma;

        public FirmModelWraper(FirmModel cFirmModel)
        {
            _CurrentFirma = cFirmModel;
        }
        public string Name
        {
            get
            {
                if (_CurrentFirma != null)
                    return _CurrentFirma.Name;
                return "";
            }
            set
            {
                if (_CurrentFirma != null && _CurrentFirma.Name == value) return;
                _CurrentFirma.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Id
        {
            get
            {
                if (_CurrentFirma != null)
                    return _CurrentFirma.Id;
                return 0;
            }
            set
            {
                if (_CurrentFirma != null && _CurrentFirma.Id == value) return;
                _CurrentFirma.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Bulstad
        {
            get
            {
                if (_CurrentFirma != null) return _CurrentFirma.Bulstad;
                return "";
            }
            set
            {
                if (_CurrentFirma != null && _CurrentFirma.Bulstad == value) return;
                _CurrentFirma.Bulstad = value;
                OnPropertyChanged("Bulstad");
                

            }
        }


        public FirmModel CurrentFirma
        {
            get { return _CurrentFirma; }
            set { _CurrentFirma = value;
            OnPropertyChanged("CurrentFirma"); 
            OnPropertyChanged("DDSnum");
            OnPropertyChanged("Bulstad");
            OnPropertyChanged("Name");
            }
        }
        public string DDSnum
        {
            get
            {
                if (_CurrentFirma != null) return _CurrentFirma.DDSnum;
                return "";
            }
            set
            {
                if (_CurrentFirma.DDSnum == value) return;
                _CurrentFirma.DDSnum = value;
                OnPropertyChanged("DDSnum");

            }
        }
    }
}
