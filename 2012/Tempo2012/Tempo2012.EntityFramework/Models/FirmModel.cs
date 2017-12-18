using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;


namespace Tempo2012.EntityFramework.Models
{
    [Serializable]
    public enum EditMode
    {
        View,
        Edit,
        Add
    }
    [Serializable]
    public class FirmModel:BaseModel,IDataErrorInfo
    {
        public virtual int Id{get;set;}
        public virtual string Name { get; set; }
        public virtual string Bulstad { get; set; }
        public virtual string DDSnum { get; set; }
        public virtual string Address { get; set; }
        public virtual string Telefon { get; set; }
        public virtual string Presentor { get; set; }
        public virtual string NameBoss { get; set; }
        public virtual string EGN { get; set; }
        public virtual int PresentorYN { get; set; }
        public virtual string Names { get; set; }
        public virtual string Tel { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Address2 { get; set; }
        public virtual int City { get; set; }
        public virtual int City1 { get; set; }
        public virtual int Country { get; set; }
        public virtual bool RegisterDds { get; set;}
        public virtual int AccType { get; set;}
        public FirmModel Clone()
        {
            return (FirmModel)this.MemberwiseClone();
        }

        public virtual string CityName { get; set;}
        public virtual string CityName2 { get; set;}
        public virtual string Zip { get; set; }
        public virtual string ContryName { get; set;}
        public virtual string Zip2 { get; set; }
        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        static readonly string[] ValidatedProperties = 
        { 
            "EGN", 
            "Name", 
            "DDSnum",
            "Bulstad"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "EGN":
                    error = MainValidators.MainValidators.ValidateEGN(this.EGN);
                    break;

                case "Name":
                    error = this.ValidateFirstName();
                    break;

                case "DDSnum":
                    error = this.ValidateDDSnum(this.DDSnum,9,15,false,"ЗДДС Номер ");
                    break;
                case "Bulstad":
                    error = this.ValidateDDSnum(this.Bulstad, 9, 13, true,"БУЛСТАТ ");
                    break;
                 
                default:
                    Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }

        

        string ValidateFirstName()
        {
            if (MainValidators.MainValidators.IsStringMissing(this.Name))
            {
                return "Задължително поле: Име на фирма";
            }
           
            return null;
        }

        string ValidateDDSnum(string num,int min,int max,bool checkdigits,string title)
        {
            if (MainValidators.MainValidators.IsStringMissing(num))
            {
                return string.Format("Задължително поле: ДДС Номер",title);
            }
            else if (checkdigits && !MainValidators.MainValidators.IsValidRegex(num, "[0-9]$"))
            {
                return string.Format("Невалиден {0} номер.Трябва да има само цифри",title);
            }
            else
                if (!MainValidators.MainValidators.IsLengthInRange(num, min, max))
                {
                    return string.Format("{0} номер трабва да е между {1} и {2} цифри",title,min,max);
                }
            return null;
        }

       

        #endregion // Validation


        public string Error
        {
            get { return null; } 
        }

        public string this[string columnName]
        {
            get { return this.GetValidationError(columnName); }
        }
        public override string GetTableName()
        {
            return "Firma";
        }

        
    }
}
