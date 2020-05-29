using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class DdsDnevnikModel
    {
        private string _stoke;

        public DdsDnevnikModel()
        {
            DetailItems=new List<DdsItemModel>();
        }
        public DdsDnevnikModel(List<DdsItemModel> items)
        {
            DetailItems = items;
        }

        public virtual int Id { get; set;}
        public virtual string DocId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime DataF { get; set; }
        public virtual int KindActivity { get; set; }
        public virtual int KindDoc { get; set; }
        public virtual int LookupID { get; set; }
        public virtual int LookupElementID { get; set; }

        public virtual string Stoke
        {
            get { return _stoke; }
            set
            {
                if (value != null)
                {
                    _stoke = value.Length > 30 ? value.Substring(0, 30) : value;
                }
                
            }
        }

        public List<DdsItemModel> DetailItems { get; set;}
        public virtual string Branch { get; set; }
        public virtual string Bulstat { get; set; }
        public virtual string Nzdds { get; set; }
        public virtual string Title { get; set;}
        public virtual int Num { get; set;}
        public virtual string CodeDoc { get; set;}
        public virtual string NameKontr { get; set; }
        public virtual decimal Suma { get; set; }
        public virtual decimal SumaDDS { get; set; }
        public virtual int Year { get; set;}
        public virtual int Month { get; set; }
        public virtual string A8 { get; set; }
        public virtual DateTime FromDate { get; set;}
        public virtual DateTime ToDate { get; set;}
        public virtual string ClNum { get; set; }
        public virtual bool IsLinked { get; set;}
        public string DdsIncluded { get; set; }
        public string Total { get; set; }
        public int IsSuma { get; set; }
    }
}
