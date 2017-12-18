namespace Tempo2012.EntityFramework.Models
{
    public class Declar
    {
        private DeclarConfigModel Config;
        private Purchases Purchases;
        private Sells Sells;
        public Declar(DeclarConfigModel config, Purchases pocupki, Sells prodazbi)
        {
            Config = config;
            Purchases = pocupki;
            Sells = prodazbi;
        }

        public decimal Kl01
        {
            get { return Sells.Kol11 + Sells.Kol13 + Sells.Kol14 + Sells.Kol17+Sells.Kol19+Sells.Kol20+Sells.Kol21;}
        }

        public decimal Kl11
        {
            get { return Sells.Kol11; }
        }

        public decimal Kl12
        {
            get { return Sells.Kol13+Sells.Kol14;}
        }

        public decimal Kl13
        {
            get { return Sells.Kol17;}
        }

        public decimal Kl14
        {
            get { return Sells.Kol19; }
        }

        public decimal Kl15
        {
            get { return Sells.Kol20; }
        }

        public decimal Kl16
        {
            get { return Sells.Kol21; }
        }
        public decimal Kl17
        {
            get { return Sells.Kol22; }
        }

        public decimal Kl18
        {
            get { return Sells.Kol23 + Sells.Kol25;}
        }

        public decimal Kl19
        {
            get { return Sells.Kol24; }
        }
       
        public decimal Kl20
        {
            get {
                return Sells.Kol12 + Sells.Kol15 + Sells.Kol16 + Sells.Kol18;
            }
        }

        public decimal Kl21
        {
            get { return Sells.Kol12; }
        }

        public decimal Kl22
        {
            get { return Sells.Kol15; }
        }
        public decimal Kl23
        {
            get { return Sells.Kol16; }
        }
        public decimal Kl24
        {
            get { return Sells.Kol18; }
        }
        public decimal Kl25
        {
            get { return Sells.Kol18; }
        }
       
        
        
        public decimal Kl30
        {
            get { return Purchases.Kol9 + Purchases.Kol15; }
        }
        public decimal Kl31
        {
            get { return Purchases.Kol10; }
        }
        public decimal Kl32
        {
            get { return Purchases.Kol12; }
        }
        public decimal Kl33
        {
            get { return Config.Kl33;}
        }

        public decimal Kl40
        {
            get { return Kl41 + Kl42*Kl33+ Kl43;}
        }
        public decimal Kl41
        {
            get { return Purchases.Kol11; }
        }
        public decimal Kl42
        {
            get { return Purchases.Kol13; }
        }
        public decimal Kl43
        {
            get { return Purchases.Kol14; }
        }

        public decimal Kl50
        {
            get { return Kl20 - Kl40 >= 0 ? Kl20 - Kl40 : 0;}
        }

        public decimal Kl60
        {
            get { return Kl40 - Kl20 > 0 ? Kl40 - Kl20 : 0;}
        }

        public decimal Kl70
        {
            get { return Config.Kl70; }
        }

        public decimal Kl71
        {
            get { return Config.Kl71; }
        }

        public decimal Kl80
        {
            get { return Config.Kl80; }
        }

        public decimal Kl81
        {
            get { return Config.Kl81; }
        }

        public decimal Kl82
        {
            get { return Config.Kl82; }
        }

        public int CountSells
        {
            get
            {
                return Sells.Count;
            }
        }

        public int CountPurchases
        {
            get
            {
                return Purchases.Count;
            }
        }
    }
}