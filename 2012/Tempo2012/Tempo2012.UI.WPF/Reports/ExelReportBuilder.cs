using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Tempo2012.UI.WPF.Reports
{
    public static class ExelReportBuilder
    {
        public static void CreateWorkbook(string fileName,List<string> headers,List<List<string>> data,List<string> footers,string title=null,string footNote=null)
        {
            int rownumber = 1, columnNumber = 1;
            
            var path = AppDomain.CurrentDomain.BaseDirectory + fileName;
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
                if (title != null)
                {
                    sheet.Cells[rownumber, columnNumber] = title;
                    rownumber = 10;
                }
                // Fill spreadsheet with sample data
                
                foreach (var hedar in headers)
                {
                    sheet.Cells[rownumber, columnNumber] = hedar;
                    columnNumber++;
                }

                foreach (List<string> dList in data)
                {
                    rownumber++;
                    columnNumber = 1;
                    foreach (string s in dList)
                    {
                        sheet.Cells[rownumber, columnNumber] = s;
                        columnNumber++;
                    }
                }

                columnNumber = 1;
                rownumber++;
                foreach (var footer in footers)
                {
                    sheet.Cells[rownumber, columnNumber] = footer;
                    columnNumber++;
                }

                columnNumber = 1;
                rownumber++;
                if (footNote != null)
                {
                    sheet.Cells[rownumber, columnNumber] = footNote;
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
                MessageBox.Show(msg);
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
        
    }
}
 