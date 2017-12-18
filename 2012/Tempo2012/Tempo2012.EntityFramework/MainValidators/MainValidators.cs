using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tempo2012.EntityFramework.MainValidators
{
    public static class MainValidators
    {
        public static bool IsValidRegex(string egn, string pattern)
        {
            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            //string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            if (egn == null) return false;
            return Regex.IsMatch(egn, pattern, RegexOptions.IgnoreCase);
        }
        public static string ValidateEGN(string egn)
        {
            if (!EntityFramework.MainValidators.MainValidators.IsValidRegex(egn, "[0-9]{10}$"))
            {
                return "Невалидно ЕГН.Трябва да бъде точно 10 цифри";
            }
            return null;
        }
        public static bool IsLengthInRange(string teststr, int min, int max)
        {
            if (teststr.Length >= min && teststr.Length <= max)
            {
                return true;
            }
            return false;

        }
        public static bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }

        public static bool IsValidEmailAddress(string email)
        {
            if (IsStringMissing(email))
                return false;

            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
