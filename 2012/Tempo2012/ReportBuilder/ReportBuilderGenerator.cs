using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Tempo2012.EntityFramework;

namespace ReportBuilder
{
    public class ReportBuilderGenerator
    {
        public static void CreateWorkbook(IReportBuilder iReportBuilder)
        {
            //int rownumber = 1, columnNumber = 1;

            //var path = Path.Combine(Entrence.CurrentFirmaPathReport, iReportBuilder.Filename+DateTime.Now.ToString("ddMMyyyy")+".xls");
            //Microsoft.Office.Interop.Excel.Application xl = null;
            //Microsoft.Office.Interop.Excel._Workbook wb = null;
            //Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            ////VBIDE.VBComponent module = null;
            //bool SaveChanges = false;
            //try
            //{

            //    if (File.Exists(path)) { File.Delete(path); }

            //    GC.Collect();

            //    // Create a new instance of Excel from scratch

            //    xl = new Microsoft.Office.Interop.Excel.Application();
            //    xl.Visible = true;


            //    // Add one workbook to the instance of Excel

            //    wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
            //    wb.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);




            //    // Get a reference to the one and only worksheet in our workbook

            //    sheet = (Microsoft.Office.Interop.Excel._Worksheet)wb.ActiveSheet;
            //    //sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[0]);
            //    if (iReportBuilder.Title != null)
            //    {
            //        sheet.Cells[rownumber, columnNumber] = iReportBuilder.Title;
            //        rownumber = 5;
            //    }
            //    // Fill spreadsheet with sample data
            //    var hedar = iReportBuilder.GetHeader();
            //    GenFilter(iReportBuilder, hedar);
            //    foreach (var item in hedar)
            //    {
            //        sheet.Cells[rownumber, columnNumber] = item;
            //        rownumber++;
            //    }

            //    rownumber++;
            //    int i = 0;
            //    foreach (var footer in iReportBuilder.GetTitles())
            //    {
                    
            //        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
            //        {
            //            sheet.Cells[rownumber, columnNumber] = footer;
            //            columnNumber++;
            //        }
            //        i++;
            //    }

            //    foreach (List<string> dList in iReportBuilder.GetItems())
            //    {
            //        rownumber++;
            //        columnNumber = 1;
            //        i = 0;
            //        foreach (string s in dList)
            //        {
            //            if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
            //            {
            //                sheet.Cells[rownumber, columnNumber] = s;
            //                columnNumber++;
            //            }
            //            i++;
            //        }
                    
            //    }

               

            //    columnNumber = 1;
            //    rownumber++;
            //    foreach (var footer in iReportBuilder.GetItems())
            //    {
            //        foreach (var f in footer)
            //        {
            //            sheet.Cells[rownumber, columnNumber] = f;
            //            columnNumber++;
            //        }
                    
            //    }

            //    columnNumber = 1;
            //    rownumber++;
            //    var futher = iReportBuilder.GetFuther();
            //    if (futher != null)
            //    {
            //        foreach (var footer in futher)
            //        {
            //            sheet.Cells[rownumber, columnNumber] = footer;
            //            columnNumber++;
            //        }
            //    }
            //    // set come column heading names




            //    //// Let loose control of the Excel instance

            //    //xl.Visible = false;
            //    //xl.UserControl = false;

            //    // Set a flag saying that all is well and it is ok to save our changes to a file.

            //    SaveChanges = true;

            //    //  Save the file to disk

            //    wb.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
            //              null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
            //              false, false, null, null, null);


            //}
            //catch (Exception err)
            //{
            //    String msg;
            //    msg = "Error: ";
            //    msg = String.Concat(msg, err.Message);
            //    msg = String.Concat(msg, " Line: ");
            //    msg = String.Concat(msg, err.Source);
                
            //}
            //finally
            //{

            //    //try
            //    //{
            //    //    // Repeat xl.Visible and xl.UserControl releases just to be sure
            //    //    // we didn't error out ahead of time.

            //    //    xl.Visible = true;
            //    //    xl.UserControl = false;
            //    //    // Close the document and avoid user prompts to save if our method failed.
            //    //    //wb.Close(SaveChanges, null, null);
            //    //    //xl.Workbooks.Close();
            //    //}
            //    //catch { }

            //    //// Gracefully exit out and destroy all COM objects to avoid hanging instances
            //    //// of Excel.exe whether our method failed or not.

            //    ////xl.Quit();

            //    ////if (module != null) { Marshal.ReleaseComObject(module); }
            //    //if (sheet != null) { Marshal.ReleaseComObject(sheet); }
            //    //if (wb != null) { Marshal.ReleaseComObject(wb); }
            //    //if (xl != null) { Marshal.ReleaseComObject(xl); }

            //    ////module = null;
            //    //sheet = null;
            //    //wb = null;
            //    //xl = null;
            //    //GC.Collect();
            //}
        }
        public static void CreateWorkbookTxt(IReportBuilder iReportBuilder)
        {

            var path = Path.Combine(Entrence.CurrentFirmaPathReport, iReportBuilder.Filename + DateTime.Now.ToString("ddMMyyyy") + ".txt"); 
            StringBuilder sb=new StringBuilder();
           try
            {

                if (File.Exists(path)) { File.Delete(path); }

               
                if (iReportBuilder.Title != null)
                {
                    sb.AppendLine(iReportBuilder.Title);
                    //sb.AppendFormat("{0:300}", "-");
                    Line(iReportBuilder, sb);
                    
                }
                // Fill spreadsheet with sample data

                //int i = 0;
                bool showline = false;
                var hedar = iReportBuilder.GetHeader();
                GenFilter(iReportBuilder, hedar);
                if (hedar != null)
                {
                    foreach (var footer in hedar)
                    {
                        sb.AppendLine(footer);
                        showline = true;
                    }
                    if (showline) Line(iReportBuilder, sb);
                }
                foreach (var item in iReportBuilder.ReportItems)
                {
                    if (string.IsNullOrWhiteSpace(item.Filter))
                    {

                    }
                }
                //    if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                //    {
                //        string ter = footer.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                //                         ? footer.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                //                         : footer;
                //        sb.Append("|");
                //        sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                //    }
                //    i++;
                //}
                //sb.Append("|");
                int i = 0;
                var items = iReportBuilder.GetTXTAntetka();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        i = 0;
                        if (item != null)
                            foreach (var el in item)
                            {
                                if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                                {
                                    string ter = el.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                                                     ? el.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                                                     : el;
                                    sb.Append("|");
                                    sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                                }
                                i++;
                            }
                        sb.Append("|");
                        sb.AppendLine();
                    }
                    Line(iReportBuilder, sb);
                }
                i = 0;
                var titles = iReportBuilder.GetTitles();
                if (titles != null && titles.Count > 0)
                {
                    foreach (var footer in iReportBuilder.GetTitles())
                    {

                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {
                            string ter = footer.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                                             ? footer.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                                             : footer;
                            sb.Append("|");
                            sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                        }
                        i++;
                    }
                    sb.Append("|");
                    Line(iReportBuilder, sb);
                }
                i = 0;
                var sub = iReportBuilder.GetSubTitles();
                if (sub != null && sub.Count > 0)
                {
                    foreach (var footer in iReportBuilder.GetSubTitles())
                    {

                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {
                            string ter = footer.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                                             ? footer.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                                             : footer;
                            sb.Append("|");
                            sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                        }
                        i++;
                    }
                    sb.Append("|");
                    Line(iReportBuilder, sb);
                }
                //sborno
                bool sborno = false;
                List<decimal> hs = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
                var it = iReportBuilder.GetItems();
                if (it != null && it.Count > 0)
                {
                    var currentrow = 0;
                    foreach (List<string> dList in it)
                    {
                        i = 0;
                        if (machfilters(dList, iReportBuilder))
                        {
                            foreach (string s in dList)
                            {
                                if (iReportBuilder.ReportItems.Count() > i &&
                                    iReportBuilder.ReportItems.ElementAt(i).IsShow)
                                {
                                    string ter=" ";
                                    if (s != null)
                                    {
                                        var s1 = s.Replace("\n", "|");
                                        ter = s1.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                                            ? s1.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                                            : s1;
                                    }
                                    
                                    sb.Append("|");
                                    //sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                                    if (iReportBuilder.ReportItems.ElementAt(i).Sborno)
                                    {   
                                        sb.Append(ter.PadLeft(iReportBuilder.ReportItems.ElementAt(i).Width));
                                        hs[i] += decimal.Parse(s);
                                        sborno = true;
                                    }
                                    else
                                    {
                                        sb.Append(iReportBuilder.ReportItems.ElementAt(i).IsSuma
                                            ? ter.PadLeft(iReportBuilder.ReportItems.ElementAt(i).Width)
                                            : ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                                    }
                                }
                                i++;
                            }
                            sb.Append("|");
                            sb.AppendLine();
                            if (iReportBuilder.Rowfoother != null && iReportBuilder.Rowfoother.ContainsKey(currentrow))
                            {
                                //Line(iReportBuilder, sb);
                                foreach (var foother in iReportBuilder.Rowfoother[currentrow])
                                {
                                    sb.Append(foother);
                                    sb.AppendLine();
                                }
                              
                            }
                           
                            
                        }
                        currentrow++;
                    }

                    if (iReportBuilder.Rowfoother == null) Line(iReportBuilder, sb);
                    if (sborno)
                    {
                        i = 0;
                        foreach (var rep in iReportBuilder.ReportItems)
                        {

                            if (rep.IsShow)
                            {
                                sb.Append("|");
                                if (rep.Sborno)
                                {
                                    sb.Append(hs[i].ToString(Vf.LevFormatUI).PadLeft(rep.Width));
                                }
                                else
                                {
                                    sb.Append(" ".PadRight(rep.Width));
                                }
                            }
                            i++;
                        }
                        sb.Append("|");
                        sb.AppendLine();
                        Line(iReportBuilder, sb);
                    }
                }
                var futher = iReportBuilder.GetFuther();
                if (futher != null && futher.Count>0)
                {
                    foreach (var footer in iReportBuilder.GetFuther())
                    {
                        sb.AppendLine(footer);
                    }
                    sb.AppendLine(); 
                }
               


            }
            catch (Exception err)
            {
                Logger.Instance().WriteLogError(err.Message, "public static void CreateWorkbookTxt(IReportBuilder iReportBuilder)");
            }
            finally
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(sb.ToString());
                }
                Process.Start(path);
            }
        }
        public static void CreateWorkbookHtml(IReportBuilder iReportBuilder)
        {

            var path = Path.Combine(Entrence.CurrentFirmaPathReport, iReportBuilder.Filename + DateTime.Now.ToString("ddMMyyyy") + ".html");
            StringBuilder sb = new StringBuilder();
            try
            {

                if (File.Exists(path)) { File.Delete(path); }


                sb.Append("<!DOCTYPE html>");
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"table.css\" />");
                sb.Append("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<div class=\"CSSTableGenerator\">");

                if (iReportBuilder.Title != null)
                {
                    sb.AppendFormat("<H1>{0}</H1>",iReportBuilder.Title);
                    
                }
               

                //foreach (var hedar in iReportBuilder.GetHeader())
                //{

                //    sb.AppendFormat("<H1>{0}</H1>", hedar);


                //}
                
                
                
                var header = iReportBuilder.GetHeader();
                GenFilter(iReportBuilder, header);
                if (header != null)
                {
                   
                    foreach (var footer in header)
                    {
                       sb.AppendFormat("<H4>{0}</H4>",footer);
                    }
                    
                }
                sb.Append("<table>");
                int i = 0;
                var titles = iReportBuilder.GetTitles();
                if (titles != null)
                {
                    sb.Append("<tr>");
                    foreach (var footer in titles)
                    {
                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                            sb.AppendFormat("<td style=\"width:{0}px\">{1}</td>",
                                iReportBuilder.ReportItems.ElementAt(i).Width*10, footer);
                        i++;
                    }
                    sb.Append("</tr>");
                }
                bool sborno = false;
                List<decimal> hs = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                var items = iReportBuilder.GetItems();
                if (items != null)
                {
                    foreach (List<string> dList in items)
                    {
                        sb.Append("<tr>");
                        i = 0;
                        foreach (string s in dList)
                        {
                            if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                                sb.AppendFormat("<td style=\"width:{0}px;\">{1}</td>",
                                    iReportBuilder.ReportItems.ElementAt(i).Width*10, s);
                            if (iReportBuilder.ReportItems.ElementAt(i).Sborno)
                            {
                                hs[i] += decimal.Parse(s);
                                sborno = true;
                            }
                            i++;
                        }
                        sb.Append("</tr>");
                    }
                }
                //sborno
                 if (sborno)
                    {
                        sb.Append("<tr>");
                        i = 0;
                        foreach (var rep in iReportBuilder.ReportItems)
                        {

                            if (rep.IsShow)
                            {
                                sb.AppendFormat("<td style=\"width:{0}px;\">{1}</td>",iReportBuilder.ReportItems.ElementAt(i).Width * 10,rep.Sborno?hs[i].ToString(Vf.LevFormatUI):" ");
                            }
                            i++;
                        }
                        sb.Append("</tr>");
                    }
                
                sb.Append("</table>");
                var futher = iReportBuilder.GetFuther();
                if (futher != null)
                {
                    foreach (var footer in iReportBuilder.GetFuther())
                    {
                        sb.AppendFormat("<p>{0}</p>", footer);
                    }
                }
                sb.AppendLine("</div></body></html>");
                

            }
            catch (Exception err)
            {

            }
            finally
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(sb.ToString(),Encoding.UTF8);
                }
                Process.Start(path);
            }
        }
        public static void CreateWorkbookCsv(IReportBuilder iReportBuilder)
        {

            var path = Path.Combine(Entrence.CurrentFirmaPathReport, iReportBuilder.Filename + DateTime.Now.ToString("ddMMyyyy") + ".csv");
            StringBuilder sb = new StringBuilder();
            try
            {

                if (File.Exists(path)) { File.Delete(path); }
                
                var items = iReportBuilder.GetTXTAntetka();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        int i = 0;
                        if (item != null)
                            foreach (var el in item)
                            {
                                if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                                {

                                    sb.Append(el);
                                    sb.Append(";");
                                }
                                i++;
                            }
                        sb.AppendLine();
                    }
                    Line(iReportBuilder, sb);
                }
                var titles = iReportBuilder.GetTitles();
                if (titles != null && titles.Count > 0)
                {
                    int i = 0;
                    foreach (var footer in iReportBuilder.GetTitles())
                    {
                        if (iReportBuilder.ReportItems.Count() > i && iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {
                            sb.Append(footer);
                            sb.Append(";");
                        }
                        i++;
                    }
                    sb.AppendLine();
                }
                var sub = iReportBuilder.GetSubTitles();
                if (sub != null && sub.Count > 0)
                {
                    int i = 0;
                    foreach (var footer in sub)
                    {

                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {

                            sb.Append(footer);
                            sb.Append(";");
                        }
                        i++;
                    }
                    sb.AppendLine();
                }
                var it = iReportBuilder.GetItems();
                if (it != null && it.Count > 0)
                {
                    foreach (List<string> dList in it)
                    {
                        int i = 0;
                        if (machfilters(dList, iReportBuilder))
                        {
                            foreach (string s in dList)
                            {
                                if (iReportBuilder.ReportItems.Count() > i &&
                                    iReportBuilder.ReportItems.ElementAt(i).IsShow)
                                {
                                    if (s != null)
                                    {
                                        sb.Append(s.Replace("\n", " "));
                                    }
                                    else
                                    {
                                        sb.Append("");
                                    }
                                    sb.Append(";");
                                }
                                i++;
                            }
                            sb.AppendLine();
                        }
                    }

                }

            }
            catch (Exception err)
            {
                Logger.Instance().WriteLogError(err.Message, "public static void CreateWorkbookCsv(IReportBuilder iReportBuilder)");
            }
            finally
            {
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
        }
        private static void GenFilter(IReportBuilder iReportBuilder, List<string> header)
        {
            foreach (var item in iReportBuilder.ReportItems)
            {
                string s = "Филтър ";
                bool isfilter = false;
                if (!string.IsNullOrWhiteSpace(item.Filter))
                {
                    s = string.Format("{0} {1}={2} ", s, item.Name, item.Filter);
                    isfilter = true;
                }
                if (isfilter)
                {
                    if (header != null)
                    {
                        header.Add(s);
                    }
                    else
                    {
                        header = new List<string> {s};
                    }
                }
            }
        } 
        private static bool machfilters(List<string> dList, IReportBuilder iReportBuilder)
        {
            var i = 0;
            foreach (string s in dList)
            {
                var filter = iReportBuilder.ReportItems.ToList()[i].Filter;
                if (s != null && (filter != null && !Aplly(s.ToUpper(),filter.ToUpper())))
                {
                    return false;
                }
                i++;
            }
            return true;
        }
        public static bool Aplly(string s, string filter)
        {
            bool neg = false;
            if (filter == "*" || string.IsNullOrWhiteSpace(filter))return true;
            if (filter.Contains("&"))
            {
                var split = filter.Split('&');
                return split.Aggregate(true, (current, filt) => current && Aplly(s, filt));
            }
            if (filter.Contains("|"))
            {
                var split = filter.Split('|');
                return split.Aggregate(false, (current, filt) => current || Aplly(s, filt));
            }
            if (filter.StartsWith("!"))
            {
                neg = true;
                filter=filter.Remove(0, 1);
            }
            if (filter.StartsWith(">="))
            {
                filter = filter.Remove(0, 2);
                float a, b;
                if (float.TryParse(s, out a) && (float.TryParse(filter, out b)))
                {
                    return a >= b;
                }
                return false;
            }
            if (filter.StartsWith("<="))
            {
                filter = filter.Remove(0, 2);
                float a, b;
                if (float.TryParse(s, out a) && (float.TryParse(filter, out b)))
                {
                    return a <= b;
                }
                return false;
            }
            if (filter.StartsWith(">"))
            {
                filter = filter.Remove(0, 1);
                float a, b;
                if (float.TryParse(s,out a) && (float.TryParse(filter,out b)))
                {
                    return a > b;
                }
                return false;
            }
            if (filter.StartsWith("<"))
            {
                filter = filter.Remove(0, 1);
                float a, b;
                if (float.TryParse(s, out a) && (float.TryParse(filter, out b)))
                {
                    return a < b;
                }
                return false;
            }
           
            if (filter.EndsWith("*") && filter.StartsWith("*"))
            {
                return neg?!s.Contains(filter.Replace("*", "")):s.Contains(filter.Replace("*", ""));
            }
            if (filter.EndsWith("*"))
            {
                return neg?!s.StartsWith(filter.Replace("*", "")):s.StartsWith(filter.Replace("*", ""));
            }
            if (filter.StartsWith("*"))
            {
                return neg?!s.EndsWith(filter.Replace("*", "")):s.EndsWith(filter.Replace("*", ""));
            }
            return neg ? !s.Equals(filter) : s.Equals(filter);
        }
        private static void Line(IReportBuilder iReportBuilder, StringBuilder sb)
        {
            sb.AppendLine();
            foreach (var c in iReportBuilder.ReportItems)
            {
                if (c.IsShow)
                {
                    for (int i = 0;i < c.Width + 1;i++)
                    {
                        sb.Append("-");
                    }
                }
            }
            sb.Append("-");
            sb.AppendLine();
        }
    }
}
