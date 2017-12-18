using System.ComponentModel;
namespace Tempo2012.UI.WPF.Models
{
    public class SaldoItemDebitCredit : INotifyPropertyChanged
    {
        public bool IsValid { get; set;}
        public bool IsLookUp { get; set;}
        public int Length { get; set; }
        public int Relookup { get; set;}
        public string Name { get; set; }
        private string _valueDebit;
        public string ValueDebit
        {
            get { return _valueDebit; }
            set
            {
                switch (Type)
                {

                    case SaldoItemTypes.Currency:
                        {
                            decimal valuedecimal;
                            IsValid = decimal.TryParse(value, out valuedecimal);
                            if (IsValid) ValuedecimalDebit = valuedecimal;
                            break;
                        }
                    case SaldoItemTypes.Integer:
                        {
                            int valueint = 0;
                            IsValid = int.TryParse(value, out valueint);
                            if (IsValid) ValueIntDebit = valueint;
                            break;
                        }
                    case SaldoItemTypes.Date:
                        {
                            break;
                        }

                }
                _valueDebit = value;
                OnPropertyChanged("ValueDebit");
            }
        }

        private string _valueCredit;
        public string ValueCredit
        {
            get { return _valueCredit; }
            set
            {
                switch (Type)
                {
                    case SaldoItemTypes.Currency:
                        {
                            decimal valuedecimal;
                            IsValid = decimal.TryParse(value, out valuedecimal);
                            if (IsValid) ValuedecimalCredit = valuedecimal;
                            break;
                        }
                    case SaldoItemTypes.Integer:
                        {
                            int valueint = 0;
                            IsValid = int.TryParse(value, out valueint);
                            if (IsValid) ValueIntCredit = valueint;
                            break;
                        }
                    case SaldoItemTypes.Date:
                        {
                            break;
                        }

                }
                _valueCredit = value;
                OnPropertyChanged("ValueCredit");
            }
        }

        public SaldoItemTypes Type { get; set; }
        public int ValueIntCredit { get; set; }

        public decimal ValuedecimalCredit { get; set; }
        public int ValueIntDebit { get; set; }
        public decimal ValuedecimalDebit { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}