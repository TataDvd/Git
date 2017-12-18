using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    [Serializable]
    public class WraperConto : BaseViewModel
    {
        private Conto _conto;
        public WraperConto(Conto conto)
        {
            _conto = conto;
            OnPropertyChanged("Oborot");
            OnPropertyChanged("DocId");
            OnPropertyChanged("Note");
            OnPropertyChanged("Data");
            OnPropertyChanged("Folder");
            OnPropertyChanged("Reason");
            OnPropertyChanged("DataInvoise");
            OnPropertyChanged("NumberObject");
            OnPropertyChanged("Index");
            OnPropertyChanged("DDetails");
            OnPropertyChanged("CDetails");
            OnPropertyChanged("IsDdsPurchasesIncluded");
            OnPropertyChanged("IsDdsSalesIncluded");
            OnPropertyChanged("IsDdsPurchases");
            OnPropertyChanged("IsDdsSales");
            OnPropertyChanged("IsPurchases");
            OnPropertyChanged("IsSales");
            OnPropertyChanged("VopPurchases");
            OnPropertyChanged("VopSales");
            OnPropertyChanged("Total");
            OnPropertyChanged("DName");
            OnPropertyChanged("CName");
            OnPropertyChanged("Pr1");
            OnPropertyChanged("Pr2");
            OnPropertyChanged("Kd");
            OnPropertyChanged("UserId");
            OnPropertyChanged("Id");
        }


       
        public int Total
        {
            get { return _conto.CartotecaCredit; }
            set { _conto.CartotecaCredit = value; OnPropertyChanged("Total"); }
        }

        public bool IsDdsPurchasesIncluded
        {
            get { return _conto.IsDdsPurchasesIncluded==1; }
            set
            {
                _conto.IsDdsPurchasesIncluded = value?1:0;
                OnPropertyChanged("IsDdsPurchasesIncluded");
            }
        }

        
        public bool IsDdsSalesIncluded
        {
            get { return _conto.IsDdsSalesIncluded == 1;}
            set
            {
                _conto.IsDdsSalesIncluded = value?1:0;
                OnPropertyChanged("IsDdsSalesIncluded");
            }
        }

        public bool IsDdsPurchases
        {
            get { return _conto.IsDdsPurchases == 1; }
            set
            {
                _conto.IsDdsPurchases = value ? 1 : 0;
                OnPropertyChanged("IsDdsPurchases");
            }
        }
        
        public bool IsPurchases
        {
            get { return _conto.IsPurchases == 1; }
            set
            {
                _conto.IsPurchases = value ? 1 : 0;
                OnPropertyChanged("IsPurchases");
                OnPropertyChanged("Dnevnik");
            }
        }
        public bool IsDdsSales
        {
            get { return _conto.IsDdsSales==1; }
            set
            {
                _conto.IsDdsSales = value ? 1 : 0;
                OnPropertyChanged("IsDdsSales");
                OnPropertyChanged("KolK");
            }
        }
        
        public bool IsSales
        {
            get { return _conto.IsSales == 1; }
            set
            {
                _conto.IsSales = value ? 1 : 0;
                OnPropertyChanged("IsSales");
                OnPropertyChanged("Dnevnik");
            }
        }
        
        public string VopPurchases
        {
            get { return _conto.VopPurchases; }
            set { _conto.VopPurchases = value;
                OnPropertyChanged("VopPurchases");
            }
        }

        
        public string VopSales
        {
            get { return _conto.VopSales; }
            set
            {
                _conto.VopSales = value;
                OnPropertyChanged("VopSales");
            }
        }

        public string CName
        {
            get { return _conto.CName; }
            set
            {
                _conto.CName = value;
                OnPropertyChanged("CName");
            }
        }

        public string DDetails
        {
            get { return _conto.DDetails; }
            set
            {
                _conto.DDetails = value;
                OnPropertyChanged("DDetails");
            }
        }
        public string CDetails
        {
            get { return _conto.CDetails; }
            set
            {
                _conto.CDetails = value;
                OnPropertyChanged("CDetails");
            }
        }

        public string DName
        {
            get { return _conto.DName; }
            set
            {
                _conto.DName = value;
                OnPropertyChanged("DName");
            }
        }

        public string Data
        {
            get { return _conto.Data.ToShortDateString(); }
            set
            {
                DateTime data;
                if (DateTime.TryParse(value, out data))
                {
                    _conto.Data = data;
                }
                OnPropertyChanged("Data");
            }
        }
        
        public string DataInvoise
        {
            get { return _conto.DataInvoise.ToShortDateString(); }
            set
            {
                DateTime data;
                if (DateTime.TryParse(value, out data))
                {
                    _conto.DataInvoise = data;
                }
                OnPropertyChanged("DataInvoise");
            }
        }

        public decimal Oborot
        {
            get { return _conto.Oborot; }
            set
            {
                _conto.Oborot = value;
                OnPropertyChanged("Oborot");
            }
        }

        public decimal OborotValuta
        {
            get { return _conto.OborotValutaD; }
            set
            {
                _conto.OborotValutaD = value;
                OnPropertyChanged("OborotValuta");
            }
        }

        public decimal OborotKol
        {
            get { return _conto.OborotKolD; }
            set
            {
                _conto.OborotKolD = value;
                OnPropertyChanged("OborotKol");
            }
        }

        public decimal OborotValutaK
        {
            get { return _conto.OborotValutaK; }
            set
            {
                _conto.OborotValutaK = value;
                OnPropertyChanged("OborotValutaK");
            }
        }

        public decimal OborotKolK
        {
            get { return _conto.OborotKolK; }
            set
            {
                _conto.OborotKolK = value;
                OnPropertyChanged("OborotKolK");
            }
        }

        public string Reason
        {
            get { return _conto.Reason; }
            set
            {
                _conto.Reason = value;
                OnPropertyChanged("Reason");
            }
        }

        public string Folder
        {
            get { return _conto.Folder; }
            set
            {
                _conto.Folder = value;
                OnPropertyChanged("Folder");
            }
        }

        public string Note
        {
            get { return _conto.Note; }
            set
            {
                _conto.Note = value;
                OnPropertyChanged("Note");
            }
        }

       

        public int Id
        {
            get { return _conto.Id; }
            set
            {
                _conto.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public long NomId
        {
            get { return _conto.Nd; }
            set
            {
                _conto.Nd = value;
                OnPropertyChanged("NomId");
            }
        }

        public string DocId
        {
            get { return _conto.DocNum; }
            set
            {
                _conto.DocNum = value;
                OnPropertyChanged("DocId");
            }
        }

        public string Pr1
        {
            get { return _conto.Pr1; }
            set
            {
                _conto.Pr1 = value;
                OnPropertyChanged("Pr1");
            }
        }

        public string Pr2
        {
            get { return _conto.Pr2; }
            set
            {
                _conto.Pr2 = value;
                OnPropertyChanged("Pr2");
            }
        }
        public Conto CurrentConto
        {
            get { return _conto; }
            set
            {
                _conto = value; 
                OnPropertyChanged("Oborot");
                OnPropertyChanged("DocId");
                OnPropertyChanged("Note");
                OnPropertyChanged("Data");
                OnPropertyChanged("Folder");
                OnPropertyChanged("Reason");
                OnPropertyChanged("DataInvoise");
                OnPropertyChanged("NumberObject");
                OnPropertyChanged("IsDdsPurchasesIncluded");
                OnPropertyChanged("IsDdsSalesIncluded");
                OnPropertyChanged("IsDdsPurchases");
                OnPropertyChanged("IsDdsSales");
                OnPropertyChanged("IsPurchases");
                OnPropertyChanged("IsSales");
                OnPropertyChanged("VopPurchases");
                OnPropertyChanged("VopSales");
                OnPropertyChanged("Total");
                OnPropertyChanged("DDetails");
                OnPropertyChanged("CDetails");
                OnPropertyChanged("DName");
                OnPropertyChanged("CName");
                OnPropertyChanged("Pr1");
                OnPropertyChanged("Pr2");
            }
        }

        public decimal Kol
        {
            get { return _conto.OborotKolD; }
            set
            {
                _conto.OborotKolD = value;
                OnPropertyChanged("Kol");
            }
        }

        public decimal Val
        {
            get { return _conto.OborotValutaD; }
            set
            {
                _conto.OborotValutaD = value;
                OnPropertyChanged("Val");
            }
        }

        public decimal KolK
        {
            get { return _conto.OborotKolK; }
            set
            {
                _conto.OborotKolK = value;
                OnPropertyChanged("KolK");
            }
        }

        public decimal ValK
        {
            get { return _conto.OborotValutaK; }
            set
            {
                _conto.OborotValutaK = value;
                OnPropertyChanged("ValK");
            }
        }

        //public string KindActivity
        //{
        //    get { return _conto.KindActivity; }
        //    set { _conto.KindActivity = value;OnPropertyChanged("KindActivity");}
        //}
        
        public string Dnevnik
        {
            get
            {
                if (IsSales && IsPurchases)
                {
                    return "Пр./Пок.";
                }
                if (IsSales)
                {
                    return "Прод.";
                }
                if (IsPurchases)
                {
                    return "Пок.";
                }
                return "";
            }
            
        }

        public long Page { get { return _conto.Page; } }

        public int TotalCount
        {
            get { return _conto.TotalCound;}
        }

        public int Year
        {
            get { if (_conto != null) return _conto.Data.Year;
                return 0;
            }
        }

        public int Month
        {
        
            get { if (_conto != null) return _conto.Data.Month;
                return 0;
            }
        }

        
        public int NumberObject
        {
            get { return _conto.NumberObject;}
            set { _conto.NumberObject = value; OnPropertyChanged("NumberObject"); }
        }

        public string Kd
        {
            get { return _conto.KD; }
            set { _conto.KD = value; OnPropertyChanged("Kd"); }
        }

        public int UserId
        {
            get { return _conto.UserId; }
            set { _conto.UserId = value; OnPropertyChanged("UserId"); }
        }
    }
}

