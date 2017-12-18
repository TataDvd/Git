using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace Tempo2012.EntityFramework.Interface
{
    [Serializable]
    public class CSearchAcc:ISearchAcc
    {
        public CSearchAcc()
        {
            CreditItems=new List<INameValuePair>();
            DebitItems=new List<INameValuePair>();
            ToDate = DateTime.Now;
            FromDate = DateTime.Now;
        }

        public DateTime ToDate { get; set; }

        public DateTime FromDate { get; set; }

        public string NumDoc { get; set; }

        public string Reason { get; set; }

        public string Note { get; set; }

        public string CDetails { get; set; }

        public string DDetails { get; set; }

        public string Ob { get; set;}

        public string Folder { get; set;}

        public string Foleder { get; set; }

        public byte TypeDate { get; set; }

        public int Month { get; set;}

        public IList<INameValuePair> CreditItems { get; set; }

        public IList<INameValuePair> DebitItems { get; set; }

        public IAccNum CreditAcc { get; set; }

        public IAccNum DebitAcc { get; set; }
        public string Pr1 { get; set; }
        public string Pr2 { get; set; }

        public string Id { get; set; }

        public string UserId { get; set; }
        public string PorNom
        {
            get; set;
        }
        

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            bool isused = false;
            sb.Append("Маска :");
            if (!string.IsNullOrWhiteSpace(Ob))
            {
                sb.AppendFormat(" Обект={0}", Ob);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Folder))
            {
                sb.AppendFormat(" Папка={0}", Folder);
                isused = true;
            }
            if (DebitAcc != null)
            {
                sb.AppendFormat(" Дебит сметка={0}", DebitAcc);
                isused = true;
            }
            if (CreditAcc != null)
            {
                sb.AppendFormat(" Кредит сметка={0}", CreditAcc);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Reason))
            {
                sb.AppendFormat(" Основание=*{0}*", Reason);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Note))
            {
                sb.AppendFormat(" Забележка=*{0}*", Note);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Pr1))
            {
                sb.AppendFormat(" Признак 1=*{0}*",Pr1);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Pr2))
            {
                sb.AppendFormat(" Признак 2=*{0}*", Pr2);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(PorNom))
            {
                sb.AppendFormat(" Пореден Номер={0}", PorNom);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(Id))
            {
                sb.AppendFormat(" Уникален номер={0}", Id);
                isused = true;
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                sb.AppendFormat(" Потребител={0}",UserId);
                isused = true;
            }
            if (CreditItems != null)
            {
                foreach (var item in CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                {
                    sb.AppendFormat(" {0} = {1} ", item.Name, item.Value);
                }
                isused = true;
            }
            if (DebitItems != null)
            {
                foreach (var item in DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                {
                    sb.AppendFormat(" {0} = {1} ", item.Name, item.Value);
                }
            }
            if (!isused)
            {
                sb.Append("*");
            }
            return sb.ToString();
        }
    }
}