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
            int rownumber = 1, columnNumber = 1;

            var path = String.Format("{0}{1}.xls", AppDomain.CurrentDomain.BaseDirectory, iReportBuilder.Filename);
            Microsoft.Office.Interop.Excel.Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            //VBIDE.VBComponent module = null;
            bool SaveChanges = false;
            try
            {

                if (File.Exists(path)) { File.Delete(path); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = true;


                // Add one workbook to the instance of Excel

                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                wb.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);




                // Get a reference to the one and only worksheet in our workbook

                sheet = (Microsoft.Office.Interop.Excel._Worksheet)wb.ActiveSheet;
                //sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[0]);
                if (iReportBuilder.Title != null)
                {
                    sheet.Cells[rownumber, columnNumber] = iReportBuilder.Title;
                    rownumber = 5;
                }
                // Fill spreadsheet with sample data
                int i = 0;
                foreach (var hedar in iReportBuilder.GetHeader())
                {

                    if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                    {
                        sheet.Cells[rownumber, columnNumber] = hedar;
                        columnNumber++;
                    }
                    i++;
                }

                columnNumber = 1;
                rownumber++;
                i = 0;
                foreach (var footer in iReportBuilder.GetTitles())
                {
                    
                    if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                    {
                        sheet.Cells[rownumber, columnNumber] = footer;
                        columnNumber++;
                    }
                    i++;
                }

                foreach (List<string> dList in iReportBuilder.GetItems())
                {
                    rownumber++;
                    columnNumber = 1;
                    i = 0;
                    foreach (string s in dList)
                    {
                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {
                            sheet.Cells[rownumber, columnNumber] = s;
                            columnNumber++;
                        }
                        i++;
                    }
                    
                }

               

                columnNumber = 1;
                rownumber++;
                foreach (var footer in iReportBuilder.GetItems())
                {
                    sheet.Cells[rownumber, columnNumber] = footer;
                    columnNumber++;
                }

                columnNumber = 1;
                rownumber++;
                foreach (var footer in iReportBuilder.GetFuther())
                {
                    sheet.Cells[rownumber, columnNumber] = footer;
                    columnNumber++;
                }
                // set come column heading names




                //// Let loose control of the Excel instance

                //xl.Visible = false;
                //xl.UserControl = false;

                // Set a flag saying that all is well and it is ok to save our changes to a file.

                SaveChanges = true;

                //  Save the file to disk

                wb.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                          null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                          false, false, null, null, null);


            }
            catch (Exception err)
            {
                String msg;
                msg = "Error: ";
                msg = String.Concat(msg, err.Message);
                msg = String.Concat(msg, " Line: ");
                msg = String.Concat(msg, err.Source);
                
            }
            finally
            {

                //try
                //{
                //    // Repeat xl.Visible and xl.UserControl releases just to be sure
                //    // we didn't error out ahead of time.

                //    xl.Visible = true;
                //    xl.UserControl = false;
                //    // Close the document and avoid user prompts to save if our method failed.
                //    //wb.Close(SaveChanges, null, null);
                //    //xl.Workbooks.Close();
                //}
                //catch { }

                //// Gracefully exit out and destroy all COM objects to avoid hanging instances
                //// of Excel.exe whether our method failed or not.

                ////xl.Quit();

                ////if (module != null) { Marshal.ReleaseComObject(module); }
                //if (sheet != null) { Marshal.ReleaseComObject(sheet); }
                //if (wb != null) { Marshal.ReleaseComObject(wb); }
                //if (xl != null) { Marshal.ReleaseComObject(xl); }

                ////module = null;
                //sheet = null;
                //wb = null;
                //xl = null;
                //GC.Collect();
            }
        }
        public static void CreateWorkbookTxt(IReportBuilder iReportBuilder)
        {
            
            var path = AppDomain.CurrentDomain.BaseDirectory + iReportBuilder.Filename+".txt";
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
                foreach (var footer in iReportBuilder.GetHeader())
                {
                    sb.AppendLine(footer);
                    showline=true;
                }
                if (showline) Line(iReportBuilder, sb);
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
                foreach (var item in iReportBuilder.GetTXTAntetka())
                {
                    i = 0;
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

                
                //foreach (var footer in iReportBuilder.GetTitles())
                //{
                    
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
                //Line(iReportBuilder, sb); 
                i = 0;
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
                sb.AppendLine();
                Line(iReportBuilder, sb); 
                
                
                foreach (List<string> dList in iReportBuilder.GetItems())
                {
                    i = 0;
                    
                    foreach (string s in dList)
                    {
                        if (iReportBuilder.ReportItems.Count()>i && iReportBuilder.ReportItems.ElementAt(i).IsShow)
                        {
                            string ter = s.Length > iReportBuilder.ReportItems.ElementAt(i).Width
                                             ? s.Substring(0, iReportBuilder.ReportItems.ElementAt(i).Width)
                                             : s;
                            sb.Append("|");
                            sb.Append(ter.PadRight(iReportBuilder.ReportItems.ElementAt(i).Width));
                        }
                        i++;
                    }
                    sb.Append("|");
                    sb.AppendLine();
                }
                
                Line(iReportBuilder, sb);
                //foreach (var footer in iReportBuilder.GetFuther())
                //{
                //    sb.AppendFormat("{0,-30}", footer);
                //}
                sb.AppendLine(); 


            }
            catch (Exception err)
            {
                Logger.Instance().WriteLogError(err.Message);
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

        private static void Line(IReportBuilder iReportBuilder, StringBuilder sb)
        {
            //sb.AppendLine();
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

        public static void CreateWorkbookHtml(IReportBuilder iReportBuilder)
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + iReportBuilder.Filename + ".html";
            StringBuilder sb = new StringBuilder();
            try
            {

                if (File.Exists(path)) { File.Delete(path); }


                sb.Append("<!DOCTYPE html>");
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"table.css\">");
                sb.Append("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<div class=\"CSSTableGenerator\">");

                if (iReportBuilder.Title != null)
                {
                    sb.AppendFormat("<H1>{0}</H1>",iReportBuilder.Title);
                    sb.AppendLine("<BR>");
                }
               

                //foreach (var hedar in iReportBuilder.GetHeader())
                //{

                //    sb.AppendFormat("<H1>{0}</H1>", hedar);


                //}

                sb.Append("<table><tr>");
                int i = 0;
                foreach (var footer in iReportBuilder.GetHeader())
                {
                    if (iReportBuilder.ReportItems.ElementAt(i).IsShow) sb.AppendFormat("<td style=\"width:{0}px;\">{1}</td>", iReportBuilder.ReportItems.ElementAt(i).Width*10, footer);
                    i++;
                }
                sb.Append("</tr><tr>");
                i = 0;
                foreach (var footer in iReportBuilder.GetTitles())
                {
                    if (iReportBuilder.ReportItems.ElementAt(i).IsShow) sb.AppendFormat("<td style=\"width:{0}px\">{1}</td>", iReportBuilder.ReportItems.ElementAt(i).Width * 10, footer);
                    i++;
                }
                sb.Append("</tr>");
                foreach (List<string> dList in iReportBuilder.GetItems())
                {
                    sb.Append("<tr>");
                    i = 0;
                    foreach (string s in dList)
                    {
                        if (iReportBuilder.ReportItems.ElementAt(i).IsShow) sb.AppendFormat("<td style=\"width:{0}px;\">{1}</td>", iReportBuilder.ReportItems.ElementAt(i).Width * 10, s);
                        i++;
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");

                foreach (var footer in iReportBuilder.GetFuther())
                {
                    sb.AppendFormat("<p>{0}</p>", footer);
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
                    sw.Write(sb.ToString());
                }
                Process.Start(path);
            }
        }
    }
}
