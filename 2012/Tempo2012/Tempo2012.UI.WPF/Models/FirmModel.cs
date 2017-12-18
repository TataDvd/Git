using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class FirmModel
    {
        private static readonly string[] yesNo = new string[] { "Да", "Не"}; 
        public virtual int Id{get;set;}
        public virtual string Name { get; set; }
        public virtual string Bulstad { get; set; }
        public virtual string DDSnum { get; set; }
        public virtual string Sity { get; set; }
        public virtual string Address { get; set; }
        public virtual string Telefon { get; set; }
        public virtual string Presentor { get; set; }
        public virtual string NameBoss { get; set; }
        public virtual string EGN { get; set; }
        public virtual string PresentorYN { get; set; }
        public virtual string Names { get; set; }
        public virtual string Tel { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Sity2 { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string[] YesNo
        {
            get { return yesNo;}
        }
   }
}
