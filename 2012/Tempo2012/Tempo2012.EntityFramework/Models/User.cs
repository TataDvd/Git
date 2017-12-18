using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class User
    {
        public string UserName { get; set;}
        public string PassWord { get; set;}
        public string Name{ get; set; }
        public int Id { get; set; }
        public uint Rights{ get; set;}

       
        public bool CanUpdateAcc
        {
            get { return CanDo(ConstantsBinary.UpdateAcc); }
            set {
                SetOrReset(value, ConstantsBinary.UpdateAcc);
            }
        }

        
        //
        public bool CanUpdateSaldo
        {
            get { return CanDo(ConstantsBinary.UpdateSaldo); }
            set { SetOrReset(value, ConstantsBinary.UpdateSaldo); }
        }
        //
        public bool CanDeleteConto
        {
            get { return CanDo(ConstantsBinary.DeleteConto); }
            set { SetOrReset(value, ConstantsBinary.DeleteConto); }
        }
        //
        public bool CanUpdateConto
        {
            get { return CanDo(ConstantsBinary.UpdateConto); }
            set { SetOrReset(value, ConstantsBinary.UpdateConto); }
        }
        //
        public bool CanOborotReport
        {
            get { return CanDo(ConstantsBinary.OborotReport); }
            set { SetOrReset(value, ConstantsBinary.OborotReport); }
        }
        public bool CanFinishMonth
        {
            get { return CanDo(ConstantsBinary.FinishMonth); }
            set { SetOrReset(value, ConstantsBinary.FinishMonth); }
        }
        public bool CanBalansReport
        {
            get { return CanDo(ConstantsBinary.BalansReport); }
            set { SetOrReset(value, ConstantsBinary.BalansReport); }
        }
        public bool CanReportPeriodi
        {
            get { return CanDo(ConstantsBinary.ReportPeriodi); }
            set { SetOrReset(value, ConstantsBinary.ReportPeriodi); }
        }
        public bool CanClasses
        {
            get { return CanDo(ConstantsBinary.Classes); }
            set { SetOrReset(value, ConstantsBinary.Classes); }
        }
        public bool CanNewCurrency
        {
            get { return CanDo(ConstantsBinary.NewCurrency); }
            set { SetOrReset(value, ConstantsBinary.NewCurrency); }
        }
        public bool CanAddCurrencyRates
        {
            get { return CanDo(ConstantsBinary.AddCurrencyRates); }
            set { SetOrReset(value, ConstantsBinary.AddCurrencyRates); }
        }
        public bool CanFinishYear
        {
            get { return CanDo(ConstantsBinary.FinishYear); }
            set { SetOrReset(value, ConstantsBinary.FinishYear); }
        }
        public bool CanAddStore
        {
            get { return CanDo(ConstantsBinary.AddStore); }
            set { SetOrReset(value, ConstantsBinary.AddStore); }
        }
        public bool CanStoreReports
        {
            get { return CanDo(ConstantsBinary.StoreReports); }
            set { SetOrReset(value, ConstantsBinary.StoreReports); }
        }
        public bool CanHronologicalReport
        {
            get { return CanDo(ConstantsBinary.HronologicalReport); }
            set { SetOrReset(value, ConstantsBinary.HronologicalReport); }
        }
        public bool CanAnaliticalReport
        {
            get { return CanDo(ConstantsBinary.AnaliticalReport); }
            set { SetOrReset(value, ConstantsBinary.AnaliticalReport); }
        }

        private bool CanDo(uint right)
        {
            var i = Rights & right;
            return i == right;
        }

        private void SetCanDo(uint rights)
        {
            Rights=Rights|rights;
        }
        private void ResetCanDo(uint rights)
        {
            Rights = Rights & rights;
        }
        private void SetOrReset(bool value,uint flag)
        {
            if (value)
            {
                SetCanDo(flag);
            }
            else
            {
                ResetCanDo(~flag);
            }
        }

        public string PassHash { get; set; }
        public User Clone()
        {
            return (User)this.MemberwiseClone();
        }
    }
}
