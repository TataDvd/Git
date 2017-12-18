using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CoffeeLibrary
{
    /// <summary>
    /// Represents a cup of coffee that can calculate its own price
    /// based on the selected options assigned to its properties.
    /// </summary>
    public class CupOfCoffee : INotifyPropertyChanged
    {
        #region Fields

        BeanType _beanType;
        Nullable<DrinkSize> _drinkSize;
        Flavorings _flavorings = Flavorings.None;
        decimal _price;
        Temperature _temperature = Temperature.Normal;

        #endregion // Fields

        #region Properties

        public BeanType BeanType
        {
            get { return _beanType; }
            set
            {
                if (value == _beanType)
                    return;

                _beanType = value;
                this.CalculatePrice();
            }
        }

        public Nullable<DrinkSize> DrinkSize
        {
            get { return _drinkSize; }
            set
            {
                if (value == _drinkSize)
                    return;

                _drinkSize = value;
                this.CalculatePrice();
            }
        }

        public Flavorings Flavorings
        {
            get { return _flavorings; }
            set
            {
                if (value == _flavorings)
                    return;

                _flavorings = value;
                this.CalculatePrice();
            }
        }

        public decimal Price
        {
            get { return _price; }
            private set
            {
                if (value == _price)
                    return;

                _price = value;
                this.OnPropertyChanged("Price");
            }
        }

        public Temperature Temperature
        {
            get { return _temperature; }
            set
            {
                if (value == _temperature)
                    return;

                _temperature = value;
                this.CalculatePrice();
            }
        }

        #endregion // Properties

        #region Private Helpers

        void CalculatePrice()
        {
            decimal price = 0;

            if (this.DrinkSize.HasValue)
            {
                switch (this.DrinkSize)
                {
                    case CoffeeLibrary.DrinkSize.Small:
                        price = 0.99m;
                        break;

                    case CoffeeLibrary.DrinkSize.Medium:
                        price = 1.49m;
                        break;

                    case CoffeeLibrary.DrinkSize.Large:
                        price = 1.99m;
                        break;

                    default:
                        Debug.Fail("Unrecognized DrinkSize value: " + this.DrinkSize);
                        break;
                }
            }

            if (this.Flavorings != Flavorings.None)
            {
                // Check each bit in the flag enum and add
                // fifty cents to the price for each bit 
                // that is set (i.e. each shot of flavoring
                // costs fifty cents).
                int flavorings = (int)this.Flavorings;
                int bit = 1;
                for (int i = 0; i < 30; ++i)
                {
                    if ((flavorings & bit) == bit)
                        price += 0.5m;

                    bit <<= 1;
                }
            }

            this.Price = price;
        }

        #endregion // Private Helpers

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}