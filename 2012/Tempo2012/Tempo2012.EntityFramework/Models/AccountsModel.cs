using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    [Serializable]
    public class AccountsModel : BaseModel
    {
        public virtual int Id { get; set;}
        public virtual int Num { get; set;}
        public virtual int SubNum { get; set; }
        public virtual long AnaliticalNum { get; set;}
        public virtual int PartidNum { get; set; }
        public virtual string NameMain { get; set;}
        public virtual string NameMainEng { get; set;}
        public virtual string NameSub { get; set; }
        public virtual string NameSubEng { get; set; }
        public virtual int TypeAccountEx { get; set; }
        public virtual int TypeAccount { get; set; }
        public virtual int LevelAccount { get; set; }
        public virtual int TypeSaldo { get; set; }
        public virtual int FirmaId { get; set; }
        public virtual string LookUpName { get; set;}
       
        
        public virtual decimal SaldoKL { get; set;}
        public virtual decimal SaldoDL { get; set;}
        
        public virtual decimal SaldoDK { get; set;}
        public virtual decimal SaldoKK { get; set;}
        
        public virtual decimal SaldoDV { get; set;}
        public virtual decimal SaldoKV { get; set;}

        public virtual decimal OborotKL { get; set; }
        public virtual decimal OborotDL { get; set; }

        public virtual decimal OborotDK { get; set; }
        public virtual decimal OborotKK { get; set; }

        public virtual decimal OborotDV { get; set; }
        public virtual decimal OborotKV { get; set; }
        
        public virtual decimal SubSaldoDL { get; set;}
        public virtual decimal SubSaldoKL { get; set;}

        public virtual decimal SubSaldoDV { get; set;}
        public virtual decimal SubSaldoKV { get; set;}

        public virtual decimal SubSaldoDK { get; set;}
        public virtual decimal SubSaldoKK { get; set;}

        public virtual decimal EndSaldoL
        {
            get
            {
                decimal rez = 0;
                if (TypeAccount == 1)
                {
                    rez =(SaldoDL +OborotDL)-(OborotKL+SaldoKL);
                }
                else
                {
                    rez =(SaldoKL + OborotKL) - (OborotDL+SaldoDL);
                }
                return rez;
            }
        }

        public virtual decimal EndSaldoV
        {
            get
            {
                decimal rez = 0;
                if (TypeAccount == 1)
                {
                    rez = (SaldoDV + OborotDV) - (OborotKV + SaldoKV);
                }
                else
                {
                    rez = (SaldoKV + OborotKV) - (OborotDV+SaldoDV);
                }
                return rez;
            }
        }

        public virtual decimal EndSaldoK
        {
            get
            {
                decimal rez = 0;
                if (TypeAccount == 1)
                {
                    rez = (SaldoDK + OborotDK) - (OborotKK+SaldoKK);
                }
                else
                {
                    rez = (SaldoKK + OborotKK) - (OborotDK+SaldoDK);
                }
                return rez;
            }
        }
        public virtual decimal BeginSaldoL
        {
            get
            {
                 return TypeAccount == 1 ? SaldoDL-SaldoKL : SaldoKL-SaldoDL;
            }
        }

        public virtual decimal BeginSaldoV
        {
            get
            {
                return  TypeAccount == 1 ? SaldoDV-SaldoKV : SaldoKV-SaldoDV;
            }
        }

        public virtual decimal BeginSaldoK
        {
            get
            {
                return TypeAccount == 1 ? SaldoDK-SaldoKK : SaldoKK-SaldoDK;
            }
        }
        public virtual decimal TotalSaldoDL
        {
            get { return SaldoDL + SubSaldoDL;}
        }
        public virtual decimal TotalSaldoKL
        {
            get { return SaldoKL + SubSaldoKL;}
        }
        public virtual decimal TotalSaldoDV
        {
            get { return SaldoDV + SubSaldoDV;}
        }
        public virtual decimal TotalSaldoKV
        {
            get { return SaldoKV + SubSaldoKV;}
        }
        public virtual decimal TotalSaldoDK
        {
            get { return SaldoDK + SubSaldoDK;}
        }
        public virtual decimal TotalSaldoKK
        {
            get { return SaldoKK + SubSaldoKK;}
        }

        public virtual long TypeAnaliticalKey { get; set;}
        public AccountsModel Clone()
        {
            return (AccountsModel)this.MemberwiseClone();
        }
        public override string ToString()
        {
            int num=0;
            if (Num >= 0) num = Num;
            if (SubNum > 0) num = SubNum;
            string res="";
            if (num>0)
            {
                res = SubNum > 0 ? string.Format("{0}/{1} {2}", Num, SubNum, NameMain) : string.Format("{0} {1}",num, NameMain);
            }
            else
            {
               res = Num==0 ? string.Format("000 {0}", NameMain) : NameMain;
            }
            //res+= (SubNum != 0) ?"/"+SubNum.ToString() : (AnaliticalNum != 0) ?"/":" ";
            //res += (AnaliticalNum != 0) ? "/" + AnaliticalNum.ToString() : " ";
            ////res += (!string.IsNullOrEmpty(NameMain) && string.IsNullOrEmpty(NameSub)) ? " " + NameMain : "";
            ////res += !string.IsNullOrEmpty(NameSub) ? " " + NameSub : ""
            //res += " " + NameMain;
            //res += (PartidNum != 0) ? " # " + PartidNum.ToString() + " " + LookUpName:"";
            return res;
        }
        public override string GetTableName()
        {
            return "accounts";
        }
        public string ShortName
        {
            get { return this.ToString(); }
        }
        public string Short
        {
            get
            {
                int num = 0;
                if (Num > 0) num = Num;
                if (SubNum > 0) num = SubNum;
                string res = "";
                if (num > 0)
                {
                    res = SubNum > 0 ? string.Format("{0}/{1}", Num, SubNum) : string.Format("{0}", num);
                }
                else
                {
                    if (num==0) res = "000";
                }
                //res+= (SubNum != 0) ?"/"+SubNum.ToString() : (AnaliticalNum != 0) ?"/":" ";
                //res += (AnaliticalNum != 0) ? "/" + AnaliticalNum.ToString() : " ";
                ////res += (!string.IsNullOrEmpty(NameMain) && string.IsNullOrEmpty(NameSub)) ? " " + NameMain : "";
                ////res += !string.IsNullOrEmpty(NameSub) ? " " + NameSub : ""
                //res += " " + NameMain;
                //res += (PartidNum != 0) ? " # " + PartidNum.ToString() + " " + LookUpName:"";
                return res;
            }
        }

        public virtual string Search { get; set;
        }
        public DateTime DataInvoise { get; set; }
    }
}
