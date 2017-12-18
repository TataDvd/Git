namespace Tempo2012.EntityFramework.Models
{
    public class Sells
    {
        public decimal Kol9
        {
            get { return Kol11 + Kol13 + Kol14 + Kol17 + Kol19 + Kol20 + Kol21; }
        }
        public decimal Kol10
        {
            get { return Kol12 + Kol15 + Kol16 + Kol18; }
        }
        public decimal Kol11{ get; set; }
        public decimal Kol12{ get; set; }
        public decimal Kol13{ get; set; }
        public decimal Kol14{ get; set; }
        public decimal Kol15{ get; set; }
        public decimal Kol16 { get; set; }
        public decimal Kol17 { get; set; }
        public decimal Kol18 { get; set; }
        public decimal Kol19 { get; set; }
        public decimal Kol20 { get; set; }
        public decimal Kol21 { get; set; }
        public decimal Kol22 { get; set; }
        public decimal Kol23 { get; set; }
        public decimal Kol24 { get; set; }
        public decimal Kol25 { get; set; }
        public int Count { get; set; }
    }
}