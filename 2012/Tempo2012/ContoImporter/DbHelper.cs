using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;

namespace ContoImporter
{
    public class DbHelper
    {
        public static IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields, int typecpnto)
        {
            List<SaldoItem> saldoItems = new List<SaldoItem>();
            int offset = 16;
            if (typecpnto == 2) offset = 60;
            int current = 0;
            foreach (SaldoAnaliticModel analiticalFields in fields)
            {
                current++;
                //Titles.Add(analiticalFields.Name);
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.DBField == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;

                }
                if (analiticalFields.DBField.Contains("DECIMAL"))
                {
                    saldotype = SaldoItemTypes.Currency;

                }
                if (analiticalFields.DBField == "Date")
                {
                    saldotype = SaldoItemTypes.Date;

                }

                SaldoItem saldoItem = new SaldoItem();
                saldoItem.Type = saldotype;
                saldoItem.Name = analiticalFields.Name;
                saldoItem.Value = analiticalFields.VAL;
                saldoItem.Fieldkey = analiticalFields.ACCFIELDKEY;
                saldoItem.IsK = typecpnto == 0;
                saldoItem.IsD = typecpnto == 1;
                saldoItem.Id = analiticalFields.ID;
                saldoItem.KursDif = analiticalFields.KURSD;
                saldoItem.ValueKurs = analiticalFields.KURS;
                saldoItem.MainKurs = analiticalFields.KURSM;
                saldoItem.ValueVal = analiticalFields.VALVAL;
                saldoItem.ValueCredit = analiticalFields.VALUEMONEY;
                saldoItem.Lookupval = analiticalFields.LOOKUPVAL;
                saldoItem.TabIndex = offset + current;
                if (analiticalFields.ACCFIELDKEY == 29 || analiticalFields.ACCFIELDKEY == 30 ||
                    analiticalFields.ACCFIELDKEY == 31)
                {
                    saldoItem.IsDK = true;
                    if (analiticalFields.ACCFIELDKEY == 30)
                    {
                        //saldoItem.InfoTitle = "Валутен курс";
                        saldoItem.IsVal = true;
                        
                    }
                    if (analiticalFields.ACCFIELDKEY == 31)
                    {
                        //    saldoItem.InfoTitle = "Единичнa цена";
                        saldoItem.IsKol = true;
                        saldoItem.ValueKol = analiticalFields.VALVAL;
                        saldoItem.OnePrice = analiticalFields.KURS;
                    }
                  
                }
                if (analiticalFields.LOOKUPID != 0)
                {
                    saldoItem.LiD = analiticalFields.LOOKUPFIELDKEY;

                    saldoItem.Relookup = analiticalFields.LOOKUPID;
                    saldoItem.IsLookUp = true;
                    
                }
                saldoItems.Add(saldoItem);
            }
            return saldoItems;
        }
    }
}
