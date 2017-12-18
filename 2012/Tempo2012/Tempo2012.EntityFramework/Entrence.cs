using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    public class Entrence
    {
        private static ConfigFirmaToLookupToField _configFirmaToLookup;
        private static List<string> _providerList;
        public static string ConnectionString { get; set; }
      
        public static CSearchAcc Mask { get; set;}

        public static double FontSize { get; set; }

        public static ConfigFirmaToLookupToField ConfigFirmaToLookup
        {
            get
            {
                if (_configFirmaToLookup == null)
                {
                    _configFirmaToLookup=new ConfigFirmaToLookupToField();
                    _configFirmaToLookup.LoadFromDb();
                }
                return _configFirmaToLookup;
            }
            set { _configFirmaToLookup = value; }
        }

        public static FirmModel CurrentFirma { get; set; }

        public static string ConectionStringTemplate
        {
            get { return "User ID=sysdba;Password=masterkey;Database={0}:{1}\\{2}\\TEMPO2012.FDB;DataSource={0};Charset=UTF8";}
        }

        public static string DdsSmetkaD { get; set;}
        public static string DdsSmetkaK { get; set; }

        public static string UserName { get; set; }
        public static int  UserId { get; set; }


        public static bool IsShowPopUp { get; set; }
        public static bool IsShowLogin { get; set; }
        public static int InfoCount { get; set; }
        public static List<string> ProviderList
        {
            get
            {
                if (_providerList == null)
                {
                    if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bulstats.txt")))
                    {
                        try
                        {
                            var list = File.ReadLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bulstats.txt"));
                            if (list!=null) return _providerList = new List<string>(list);
                        }
                        catch
                        {

                        }
                    }
                    _providerList = new List<string> { "BG836231725", "BG200011778", "BG101154956" };
                }
                return _providerList;
            }
            set { _providerList = value; }
        }
        public static string CurrentFirmaPath { get; set;}
        public static string CurrentFirmaPathTemplate { get; set; }
        public static string CurrentFirmaPathReport { get; set; }
        public static bool UseIntelliSense { get; set; }

        public static string GetItemFromDetails(string ddetails,string cdetails,string fildname,string oldvalue)
        {
            if (!string.IsNullOrWhiteSpace(oldvalue))
            {
                return oldvalue;
            }
            string rez = string.Empty;
            if (ddetails.Contains(fildname))
                {
                    var str = ddetails.Split(new string[] { "\n" }, StringSplitOptions.None);
                    foreach (var a in str)
                    {
                        if (a.Contains(fildname))
                        {
                            var str1=a.Split('-');
                            if (str1.Length > 0)
                            {
                                rez = str1[1];
                            }
                        }

                    }
                    
                }
             if (cdetails.Contains(fildname))
                {
                    var str = cdetails.Split(new string[] { "\n" }, StringSplitOptions.None);
                    foreach (var a in str)
                    {
                        if (a.Contains(fildname))
                        {
                            var str1=a.Split('-');
                            if (str1.Length > 0)
                            {
                                rez = str1[1];
                            }
                        }

                    }
                    
                }
             return rez;
        }
    }
}
