using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Extentions
{
    public static class mydecimal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal Parse(string s)
        {
            decimal d;
            if (!decimal.TryParse(s,out d))
            {
                var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands|NumberStyles.AllowLeadingSign;
                var provider = new CultureInfo("en-US");
                var news = s.Replace(".", provider.NumberFormat.NumberDecimalSeparator);
                d = decimal.Parse(news,style,provider);
            }
            return d;
        }
    }
}
