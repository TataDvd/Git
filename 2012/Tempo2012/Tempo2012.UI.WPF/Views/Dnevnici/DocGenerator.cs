using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.Deklar;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using Tempo2012.UI.WPF.Views.Declar;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    public class DocGenerator
    {
        public static void GenrateDeclar(IDataBaseContext context, int month, int year, DeclarConfigModel model)
        {
            Dictionary<string, string> declar = new Dictionary<string, string>();
            EntityFramework.Models.Declar d = new EntityFramework.Models.Declar(model,
                                context.GetPurchases(month, year),
                                context.GetSales(month, year));

            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            declar.Add("data", string.Format("{0:D2}/{1}", month, year));
            declar.Add("firma", firma.Name);
            declar.Add("adress", firma.Address);
            declar.Add("tel", firma.Telefon);
            declar.Add("dds", firma.DDSnum);
            declar.Add("iddds", firma.Bulstad);
            declar.Add("kl01", d.Kl01.ToString(Vf.LevFormatUI));
            declar.Add("kl11", d.Kl11.ToString(Vf.LevFormatUI));
            declar.Add("kl12", d.Kl12.ToString(Vf.LevFormatUI));
            declar.Add("kl13", d.Kl13.ToString(Vf.LevFormatUI));
            declar.Add("kl14", d.Kl14.ToString(Vf.LevFormatUI));
            declar.Add("kl15", d.Kl15.ToString(Vf.LevFormatUI));
            declar.Add("kl16", d.Kl16.ToString(Vf.LevFormatUI));
            declar.Add("kl17", d.Kl17.ToString(Vf.LevFormatUI));
            declar.Add("kl18", d.Kl18.ToString(Vf.LevFormatUI));
            declar.Add("kl19", d.Kl19.ToString(Vf.LevFormatUI));

            declar.Add("kl20", d.Kl20.ToString(Vf.LevFormatUI));
            declar.Add("kl21", d.Kl21.ToString(Vf.LevFormatUI));
            declar.Add("kl22", d.Kl22.ToString(Vf.LevFormatUI));
            declar.Add("kl23", d.Kl23.ToString(Vf.LevFormatUI));
            declar.Add("kl24", d.Kl24.ToString(Vf.LevFormatUI));

            declar.Add("kl30", d.Kl30.ToString(Vf.LevFormatUI));
            declar.Add("kl31", d.Kl31.ToString(Vf.LevFormatUI));
            declar.Add("kl32", d.Kl32.ToString(Vf.LevFormatUI));
            declar.Add("kl33", d.Kl33.ToString(Vf.LevFormatUI));

            declar.Add("kl40", d.Kl40.ToString(Vf.LevFormatUI));
            declar.Add("kl41", d.Kl41.ToString(Vf.LevFormatUI));
            declar.Add("kl42", d.Kl42.ToString(Vf.LevFormatUI));
            declar.Add("kl43", d.Kl43.ToString(Vf.LevFormatUI));

            declar.Add("kl50", d.Kl50.ToString(Vf.LevFormatUI));
            declar.Add("kl60", d.Kl60.ToString(Vf.LevFormatUI));
            declar.Add("kl70", d.Kl70.ToString(Vf.LevFormatUI));
            declar.Add("kl71", d.Kl71.ToString(Vf.LevFormatUI));

            declar.Add("kl80", d.Kl80.ToString(Vf.LevFormatUI));
            declar.Add("kl81", d.Kl81.ToString(Vf.LevFormatUI));
            declar.Add("kl82", d.Kl82.ToString(Vf.LevFormatUI));

            declar.Add("DateTimeNow", DateTime.Now.ToShortDateString());
            declar.Add("pr", firma.PresentorYN == 1 ? "X" : " ");
            declar.Add("kl", d.Kl70 > 0 || d.Kl80 > 0 || d.Kl81 > 0 || d.Kl82 > 0 ? "X" : " ");
            declar.Add("mol", firma.NameBoss);
            declar.Add("dl", firma.Tel);
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = DeklarviewModel.ReturnDeklar(declar);
            var path = AppDomain.CurrentDomain.BaseDirectory + "Deklar1.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(actual);
            }
            Process.Start(path);
        }

        public static void GenrateDeclarF(IDataBaseContext context, int month, int year,DeclarConfigModel model)
        {
            EntityFramework.Models.Declar d = new EntityFramework.Models.Declar(model,
                                context.GetPurchases(month, year),
                                context.GetSales(month, year));

            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,-15}", firma.DDSnum);                   //1
            sb.AppendFormat("{0,-50}", firma.Name);                 //2
            sb.AppendFormat("{0:D4}", year);     //3
            sb.AppendFormat("{0:D2}", month);   //
            sb.AppendFormat("{0,-50}", firma.NameBoss);                     //4
            sb.AppendFormat("{0,15}", d.CountSells);                              //6 ?
            sb.AppendFormat("{0,15}", d.CountPurchases);                              //5 ?
            sb.AppendFormat("{0,15}", d.Kl01.ToString(Vf.LevFormatUI));                         //7
            sb.AppendFormat("{0,15}", d.Kl20.ToString(Vf.LevFormatUI));                         //8
            sb.AppendFormat("{0,15}", d.Kl11.ToString(Vf.LevFormatUI));                         //9
            sb.AppendFormat("{0,15}", d.Kl21.ToString(Vf.LevFormatUI));                         //10
            sb.AppendFormat("{0,15}", d.Kl12.ToString(Vf.LevFormatUI));                         //11
            sb.AppendFormat("{0,15}", d.Kl22.ToString(Vf.LevFormatUI));                         //12
            sb.AppendFormat("{0,15}", d.Kl23.ToString(Vf.LevFormatUI));                         //13
            sb.AppendFormat("{0,15}", d.Kl13.ToString(Vf.LevFormatUI));                         //14
            sb.AppendFormat("{0,15}", d.Kl24.ToString(Vf.LevFormatUI));                         //15
            sb.AppendFormat("{0,15}", d.Kl14.ToString(Vf.LevFormatUI));                         //16
            sb.AppendFormat("{0,15}", d.Kl15.ToString(Vf.LevFormatUI));                         //17
            sb.AppendFormat("{0,15}", d.Kl16.ToString(Vf.LevFormatUI));                         //18
            sb.AppendFormat("{0,15}", d.Kl17.ToString(Vf.LevFormatUI));                         //19
            sb.AppendFormat("{0,15}", d.Kl18.ToString(Vf.LevFormatUI));                         //20
            sb.AppendFormat("{0,15}", d.Kl19.ToString(Vf.LevFormatUI));                         //21
            //pokupki
            sb.AppendFormat("{0,15}", d.Kl30.ToString(Vf.LevFormatUI));                         //22
            sb.AppendFormat("{0,15}", d.Kl31.ToString(Vf.LevFormatUI));                         //23
            sb.AppendFormat("{0,15}", d.Kl41.ToString(Vf.LevFormatUI));                         //24
            sb.AppendFormat("{0,15}", d.Kl32.ToString(Vf.LevFormatUI));                         //25
            sb.AppendFormat("{0,15}", d.Kl42.ToString(Vf.LevFormatUI));                         //26
            sb.AppendFormat("{0,15}", d.Kl43.ToString(Vf.LevFormatUI));                         //27
            //resultat
            sb.AppendFormat("{0,4}", d.Kl33.ToString(Vf.LevFormatUI));                         //28
            sb.AppendFormat("{0,15}", d.Kl40.ToString(Vf.LevFormatUI));                         //29
            sb.AppendFormat("{0,15}", d.Kl50.ToString(Vf.LevFormatUI));                         //30
            sb.AppendFormat("{0,15}", d.Kl60.ToString(Vf.LevFormatUI));                         //31
            sb.AppendFormat("{0,15}", d.Kl70.ToString(Vf.LevFormatUI));                         //32
            sb.AppendFormat("{0,15}", d.Kl71.ToString(Vf.LevFormatUI));                         //33
            sb.AppendFormat("{0,15}", d.Kl80.ToString(Vf.LevFormatUI));                         //34
            sb.AppendFormat("{0,15}", d.Kl81.ToString(Vf.LevFormatUI));                         //35
            sb.AppendFormat("{0,15}", d.Kl82.ToString(Vf.LevFormatUI));                         //36
            sb.AppendLine();
            var path = AppDomain.CurrentDomain.BaseDirectory + "Deklar.txt";
            Encoding srcEncodingFormat = Encoding.Unicode;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(sb.ToString());
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat, dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            using (StreamWriter sw = new StreamWriter(path, false, dstEncodingFormat))
            {
                sw.Write(finalString);
            }
            //Process.Start(path);
        }

        public static void GenerateDdsPurchasesF(IDataBaseContext context, int month, int year)
        {
            //var rez = context.GetPokupki(Month, Year);
            var rows = context.GetDnevItem(1, month, year);
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            var path = AppDomain.CurrentDomain.BaseDirectory + "POKUPKI.TXT";
            StringBuilder sb = new StringBuilder();
            //int i = 1;
            foreach (var item in rows)
            {
                //if (i > rows.Count - 2) break;
                sb.AppendFormat("{0,-15}", firma.DDSnum);                   //1 Идентификационен номер по ДДС на лицето
                sb.AppendFormat("{0:D4}", year);                            //2
                sb.AppendFormat("{0:D2}", month);                           //2
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
                                      //18  8а
                int a8;
                if (int.TryParse(item[8], out a8))
                {
                    if (a8 == 0)
                    {
                        sb.Append("  ");
                    }
                    else
                    {
                        sb.AppendFormat("{0}", item[8].Length == 1 ? "0" + item[8] : item[8]);
                    }
                }    
                    //{
                    //    switch (a8)
                    //    {
                    //        case 0:
                    //            
                    //            break;
                    //        case 1:
                    //            sb.Append("01");
                    //            break;
                    //        case 2:
                    //            sb.Append("02");
                    //            break;
                    //        default:
                    //            sb.Append("  ");
                    //            break;
                    //    }

                    //}
                    //else
                    //{
                    //    sb.Append("  ");
                    //}                       //8а
                    sb.AppendLine();
                //i++;
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
            //Process.Start(path);
        }

        public static void GenerateDdsPurchases(int month,int year,bool withdialog)
        {
            if (withdialog)
            {
                ReportManagerViewModel report =
                    new ReportManagerViewModel(
                        new DdsViewModel(new DdsDnevnikModel { KindActivity = 1, Title = "Дневник покупки", Month = month, Year = year,FromDate=new DateTime(year,month,1),ToDate=new DateTime(year,month, GetEndDate(month,year))}));
                report.AddNewCommand.Execute(null);
            }
            else
            {

                ReportDialog rd =
                    new ReportDialog(new DdsViewModel(new DdsDnevnikModel{KindActivity = 1, Title = "Дневник покупки", Month = month, Year = year,FromDate=new DateTime(year,month,1),ToDate=new DateTime(year,month, GetEndDate(month,year))}));
                rd.ShowDialog();

            }
        }

        private static int GetEndDate(int toMonth, int currentYear)
        {
            int rez = 30;
            switch (toMonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    {
                        rez = 31;
                    }
                    break;
                case 2:
                    rez = IsYearBig(currentYear) ? 29 : 28;
                    break;
            }
            return rez;
        }

        private static bool IsYearBig(int currentYear)
        {
            return currentYear % 4 == 0;
        }

        public static void GenerateDdsSalesF(IDataBaseContext context, int month, int year)
        {
            var rows = context.GetDnevItem(2, month, year);
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            var path = AppDomain.CurrentDomain.BaseDirectory + "PRODAGBI.TXT";
            StringBuilder sb = new StringBuilder();
            //int i = 1;
            foreach (var item in rows)
            {
                //if (i > rows.Count - 2) break;
                sb.AppendFormat("{0,-15}", firma.DDSnum);                   //1
                sb.AppendFormat("{0:D4}", year);                            //2
                sb.AppendFormat("{0:D2}", month);                           //2
                //3

                int num;
                if (int.TryParse(item[1], out num))
                {
                    sb.AppendFormat("{0,4}", num);
                }                                                 //4
                else
                {
                    sb.AppendFormat("{0,4}", 0);
                }
                sb.AppendFormat("{0,-15}", item[0]);
                sb.AppendFormat("{0,2}", item[2]);                            //5
                sb.AppendFormat("{0,-20}", item[3]);                             //6
                sb.AppendFormat("{0,-10}", item[4]);                             //7
                sb.AppendFormat("{0,-15}", item[5]);                             //8
                sb.AppendFormat("{0,-50}", item[6]);                             //9
                sb.AppendFormat("{0,-30}", item[7]);                             //10
                sb.AppendFormat("{0,15}", item[9]);                             //11
                sb.AppendFormat("{0,15}", item[10]);                         //12
                sb.AppendFormat("{0,15}", item[11]);                         //13
                sb.AppendFormat("{0,15}", item[12]);                         //14
                sb.AppendFormat("{0,15}", item[13]);                         //15
                sb.AppendFormat("{0,15}", item[14]);                         //16
                sb.AppendFormat("{0,15}", item[15]);                         //17
                sb.AppendFormat("{0,15}", item[16]);                         //18
                sb.AppendFormat("{0,15}", item[17]);                         //19
                sb.AppendFormat("{0,15}", item[18]);                         //20
                sb.AppendFormat("{0,15}", item[19]);                         //21
                sb.AppendFormat("{0,15}", item[20]);                         //22
                sb.AppendFormat("{0,15}", item[21]);                         //23
                sb.AppendFormat("{0,15}", item[22]);                         //24
                sb.AppendFormat("{0,15}", item[23]);                         //25
                sb.AppendFormat("{0,15}", item[24]);                         //26
                sb.AppendFormat("{0,15}", item[25]);                         //27
                int a8;
                if (int.TryParse(item[8], out a8))
                {
                    if (a8 == 0)
                    {
                        sb.Append("  ");
                    }
                    else
                    {
                        sb.AppendFormat("{0}", item[8].Length == 1 ? "0" + item[8] : item[8]);
                    }
                }
                //int a8;
                //if (int.TryParse(item[8], out a8))
                //{
                //    switch (a8)
                //    {
                //        case 0:
                //            sb.Append("  ");
                //            break;
                //        case 1:
                //            sb.Append("01");
                //            break;
                //        case 2:
                //            sb.Append("02");
                //            break;
                //        default:
                //            sb.Append("  ");
                //            break;
                //    }

                //}
                //else
                //{
                //    sb.Append("  ");
                //}                       //8а
                sb.AppendLine();
                //i++;
            }
            Encoding srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(sb.ToString());
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat, dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            using (StreamWriter sw = new StreamWriter(path, false, dstEncodingFormat))
            {
                sw.Write(finalString);
            }
            //Process.Start(path);
        }

        public static void GenerateDdsSales(int month, int year,bool withdialog)
        {
            if (withdialog)
            {
                ReportManagerViewModel report = new ReportManagerViewModel(new DdsViewModel(new DdsDnevnikModel { KindActivity = 2, Title = "Дневник продажби", Month = month, Year = year, FromDate = new DateTime(year, month, 1), ToDate = new DateTime(year, month, GetEndDate(month, year))}));
                report.AddNewCommand.Execute(null);
            }
            else
            {
                ReportDialog rd = new ReportDialog(new DdsViewModel(new DdsDnevnikModel { KindActivity = 2, Title = "Дневник продажби", Month = month, Year = year, FromDate = new DateTime(year, month, 1), ToDate = new DateTime(year, month, GetEndDate(month, year))}));
                rd.ShowDialog();
            }
           

        }

        public static void GenerateVies(IDataBaseContext context, int month, int year, DeclarConfigModel model)
        {
            Dictionary<string, string> declar = new Dictionary<string, string>();
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            declar.Add("data", string.Format("{0:D2}/{1:D4}", month, year));
            declar.Add("firma", firma.Name);
            declar.Add("address", firma.Address);
            declar.Add("tel", firma.Telefon);
            declar.Add("dds", firma.DDSnum);
            declar.Add("iddds", firma.Bulstad);
            declar.Add("city", firma.CityName);
            declar.Add("p", firma.PresentorYN == 1 ? "X" : " ");
            declar.Add("pr", firma.PresentorYN == 0 ? "X" : " ");
            declar.Add("zip", firma.Zip);
            declar.Add("egn", firma.EGN);
            declar.Add("address1", firma.Address2);
            if (firma.PresentorYN == 0)
            {
                declar.Add("mol", firma.NameBoss);
            }
            else
            {
                declar.Add("mol", firma.Names);
            }
            string actual;
            actual = DeklarviewModel.ReturnViesDeclar(declar, context.GetVies(month, year, declar),context.GetViesG(month, year, declar));
            var path = AppDomain.CurrentDomain.BaseDirectory + "Vies1.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(actual);
            }
            Process.Start(path);
        }

        public static void GenerateViesF(IDataBaseContext context, int month, int year, DeclarConfigModel model)
        {
            Dictionary<string, string> declar = new Dictionary<string, string>();
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            declar.Add("data", string.Format("{0:D2}/{1}", month, year));
            declar.Add("firma", firma.Name);
            declar.Add("address", firma.Address);
            declar.Add("tel", firma.Telefon);
            declar.Add("dds", firma.DDSnum);
            declar.Add("iddds", firma.Bulstad);
            declar.Add("city", firma.CityName);
            declar.Add("p", firma.PresentorYN == 1 ? "X" : " ");
            declar.Add("pr", firma.PresentorYN == 0 ? "X" : " ");
            declar.Add("zip", firma.Zip);
            declar.Add("egn", firma.EGN);
            declar.Add("address1", firma.Address2);
            if (firma.PresentorYN == 0)
            {
                declar.Add("mol", firma.NameBoss);
                declar.Add("presentor","A");
            }
            else
            {
                declar.Add("mol", firma.Names);
                declar.Add("presentor", "R");
            }
            
            var path = AppDomain.CurrentDomain.BaseDirectory + "VIES.TXT";
            var list = context.GetVies(month, year, declar);
            var listG = context.GetViesG(month, year, declar);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,3}", "VHR");              //1
            sb.AppendFormat("{0,7}", declar["data"]);     //2
            sb.AppendFormat("{0,5}", declar["count"]);     //3
            sb.AppendLine();
            sb.AppendFormat("{0,3}", "VDR");
            sb.AppendFormat("{0,-15}", declar["egn"]);
            sb.AppendFormat("{0,-150}", declar["mol"]);
            sb.AppendFormat("{0,-50}", declar["city"]);
            sb.AppendFormat("{0,4}", declar["zip"]);
            sb.AppendFormat("{0,-150}", declar["address1"]);
            sb.AppendFormat("{0,1}", declar["presentor"]);
            sb.AppendLine();
            sb.AppendFormat("{0,3}", "VTR");
            sb.AppendFormat("{0,-15}", declar["dds"]);
            sb.AppendFormat("{0,-150}", declar["mol"]);
            sb.AppendFormat("{0,-200}", declar["address"]);
            sb.AppendLine();
            sb.AppendFormat("{0,3}", "TTR");
            sb.AppendFormat("{0,12}", declar["sumak1k2k3"]);
            sb.AppendFormat("{0,12}", declar["sumak3"]);
            sb.AppendLine();
            foreach (var l in list)
            {
                sb.AppendFormat("VIR{0,5}{1,-15}{2,12}{3,12}{4,12}{5,7}", l.PorNom, l.Name, l.K3, l.K4, l.K5, " ");
                sb.AppendLine();
            }
            if (listG.Count > 0)
            {
                sb.AppendFormat("{0,3}", "CHR");
                sb.AppendFormat("{0,5}", listG.Count);
                sb.AppendLine();
                foreach (var l in listG)
                {
                    sb.AppendFormat("COS{0,5}{1,-15}{2,1}{3,-15}{4,7}", l.NomRow, l.VIN, l.KOD, l.VINDest, l.PeriodOP);
                    sb.AppendLine();
                }
            }
            Encoding srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(sb.ToString());
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat, dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            using (StreamWriter sw = new StreamWriter(path, false, dstEncodingFormat))
            {
                sw.Write(finalString);
            }
            //Process.Start(path);
        }

        public static void GenerateDdsPurchases(DateTime from, DateTime to,bool withdialog)
        {
            if (withdialog)
            {
                ReportManagerViewModel report = new ReportManagerViewModel(new DdsViewModel(new DdsDnevnikModel { KindActivity = 1, Title = "Дневник покупки", Month = from.Month, Year = from.Year,FromDate = from,ToDate = to}));
                report.StartTxt(null,null);
            }
            else
            {
                ReportDialog rd = new ReportDialog(new DdsViewModel(new DdsDnevnikModel { KindActivity = 1, Title = "Дневник покупки", Month = from.Month, Year = from.Year, FromDate = from, ToDate = to }));
                rd.ShowDialog();
            }
        }
        public static void GenerateDdsSales(DateTime from, DateTime to, bool withdialog)
        {
            if (withdialog)
            {
                ReportManagerViewModel report = new ReportManagerViewModel(new DdsViewModel(new DdsDnevnikModel { KindActivity = 2, Title = "Дневник продажби", Month = from.Month, Year = from.Year, FromDate = from, ToDate = to }));
                report.StartTxt(null, null);
            }
            else
            {
                ReportDialog rd = new ReportDialog(new DdsViewModel(new DdsDnevnikModel { KindActivity = 2, Title = "Дневник продажби", Month = from.Month, Year = from.Year, FromDate = from, ToDate = to }));
                rd.ShowDialog();
            }
        }

        public static void FirmaData()
        {
            var firma = ConfigTempoSinglenton.GetInstance().CurrentFirma;
            FirmaData(firma);
        }

        public static void FirmaData(FirmModel firma)
        {
            StringBuilder sb = new StringBuilder();
            var path = AppDomain.CurrentDomain.BaseDirectory + "FIRMADATA.TXT";
            sb.AppendLine(string.Format("Данни за Фирма {0}",firma.Name));
            sb.AppendLine();//1
            sb.AppendFormat("Име               {0}", firma.Name);
            sb.AppendLine();
            sb.AppendFormat("Булстат           {0}", firma.Bulstad);
            sb.AppendLine();
            sb.AppendFormat("ЗДДС              {0}", firma.DDSnum);
            sb.AppendLine();
            sb.AppendFormat("Телефон           {0}", firma.Telefon);
            sb.AppendLine();
            sb.AppendFormat("Град              {0}  Пощенски код:{1}", firma.CityName, firma.Zip);
            sb.AppendLine();
            sb.AppendFormat("Държава           {0}", firma.ContryName);
            sb.AppendLine();
            sb.AppendFormat("Адрес             {0}", firma.Address);
            sb.AppendLine();
            sb.AppendFormat("Собственик        {0}", firma.NameBoss);
            sb.AppendLine();
            sb.AppendLine("Данни за преставляващия");
            sb.AppendFormat("   Преставляващ      {0}", firma.Presentor);
            sb.AppendLine();
            sb.AppendFormat("   Име:              {0,-20} Презиме: {1,-20} Фамилия: {2,-20}", firma.FirstName,firma.SurName,firma.LastName);
            sb.AppendLine();
            sb.AppendFormat("   Телефон           {0}", firma.Tel);
            sb.AppendLine();
            sb.AppendFormat("   Адрес             {0}", firma.Address2);
            sb.AppendLine();
            sb.AppendFormat("   Град              {0}  Пощенски код:{1}", firma.CityName2, firma.Zip2);
            sb.AppendLine();
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(sb.ToString());
            }
            Process.Start(path);
        }
    }
}