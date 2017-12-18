using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Views.ReportManager;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Dnevnici
{
    public class DnPurchasesViewModel:BaseViewModel
    {
        public DnPurchasesViewModel()
        {
            _year = DateTime.Now.Year;
            _month = DateTime.Now.Month;
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year"); }
        }
        private int _month;
        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged("Month"); }
        }
        protected override void AddNew()
        {
            //var rez = context.GetPokupki(Month, Year);
            var rows = context.GetDnevItem(1, Month, Year);
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            var path = AppDomain.CurrentDomain.BaseDirectory + "POKUPKI.TXT";
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var item in rows)
            {
                if (i > rows.Count - 2) break;
                sb.AppendFormat("{0,-15}", firma.DDSnum);                   //1 Идентификационен номер по ДДС на лицето
                sb.AppendFormat("{0:D4}", Year);                            //2
                sb.AppendFormat("{0:D2}", Month);                           //2
                int num;
                if (int.TryParse(item[1], out num))
                {
                    sb.AppendFormat("{0,4}", num);
                }                                                           //4 Клон
                else
                {
                    sb.AppendFormat("{0,4}", 0);
                }
                sb.AppendFormat("{0,-15}", item[0]);                        //3 Пореден номер
                sb.AppendFormat("{0,2}", item[2]);                         //5 Вид Документа
                sb.AppendFormat("{0,-20}", item[3]);                        //6 Номер Документ
                sb.AppendFormat("{0,-10}", item[4]);                        //7 Дата на документа
                sb.AppendFormat("{0,-15}", item[5]);                        //8 Идентификационен номер на контрагента
                                                          
                sb.AppendFormat("{0,-50}", item[6]);                         //9 Име на контрагента
                sb.AppendFormat("{0,-30}", item[7]);                         //10 Вид на стоката
                sb.AppendFormat("{0,15}", item[9]);                         //11 ДО на ВОП
                sb.AppendFormat("{0,15}", item[10]);                         //12 ДО
                sb.AppendFormat("{0,15}", item[11]);                         //13 ДДС
                sb.AppendFormat("{0,15}", item[12]);                         //14 ДО
                sb.AppendFormat("{0,15}", item[13]);                         //15 ДДС с пълен ДК
                sb.AppendFormat("{0,15}", item[14]);                         //16 ПОСР
                sb.AppendFormat("{0,15}", item[15]);                         //17 163а
                int a8;
                if (int.TryParse(item[8], out a8))
                {
                    switch (a8)
                    {
                        case 0:
                            sb.Append("  ");
                            break;
                        case 1:
                            sb.Append("01");
                            break;
                        case 2:
                            sb.Append("02");
                            break;
                        default:
                            sb.Append("  ");
                            break;
                    }

                }
                else
                {
                    sb.Append("  ");
                }                       //8а
                sb.AppendLine();
                i++;
            }
            Encoding srcEncodingFormat = Encoding.Unicode;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(sb.ToString());
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat, dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            using (StreamWriter sw = new StreamWriter(path, false, dstEncodingFormat))
            {
                sw.Write(finalString);
            }
            Process.Start(path);
        }
        protected override void Add()
        {
            ReportDialog report = new ReportDialog(new DdsViewModel(new DdsDnevnikModel { KindActivity = 1, Title = "Дневник покупки",Month=Month,Year=Year }));
            report.ShowDialog();
        }
    }
}
