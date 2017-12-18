using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public class Vf
    {
        public static string LevFormat { get; set;}
        public static string KursFormat { get; set;}
        public static string ValFormat { get; set;}
        public static string KolFormat { get; set;}
        private static string _levFormatUi;
        public static string LevFormatUI
        {
            get { return _levFormatUi; }
            set { _levFormatUi = value; LevFormat = "{0:" + value + "}"; }
        }

        private static string _kursFormatUi;
        public static string KursFormatUI
        {
            get { return _kursFormatUi; }
            set { _kursFormatUi = value; KursFormat = "{0:" + value + "}"; }
        }

        private static string _valFormatUi;
        public static string ValFormatUI
        {
            get { return _valFormatUi; }
            set
            {
                _valFormatUi = value;
                ValFormat = "{0:" + value + "}";
            }
        }

        private static string _kolFormatUi;
        public static string KolFormatUI
        {
            get { return _kolFormatUi; }
            set { _kolFormatUi = value;
                KolFormat = "{0:" +value+ "}";
            }
        }

        public static void SetFormaters(string lvf, string kursf, string valf, string kolf)
        {
            LevFormatUI = lvf;
            KursFormatUI = kursf;
            ValFormatUI = valf;
            KolFormatUI = kolf;
        }
    }
}
