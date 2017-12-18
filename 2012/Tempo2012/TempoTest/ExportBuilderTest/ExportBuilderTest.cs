using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ReportBuilder;
using Tempo2012.EntityFramework;

namespace TempoTest.ExportBuilderTest
{
    [TestFixture]
    public class ExportBuilderTest
    {
        [Test]
        public void ExportBuilderTest_DostSqlScript()
        {
            ExportBuilderInput exportBuilderInput = new ExportBuilderInput("dost.sql", "nom_12", @"E:\inp\DOSTAVCHICI.TXT");
            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exportBuilderInput.FileName));
        }
        [Test]
        public void ExportBuilderTest_ClientSqlScript()
        {
            ExportBuilderInput exportBuilderInput = new ExportBuilderInput("dost.sql", "nom_12", @"E:\inp\KLIENTI.txt");
            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory,exportBuilderInput.FileName));
        }

        [Test]
        public void ExportBuilderTest_CitiesBgSqlScript()
        {
            CitiesInput exportBuilderInput = new CitiesInput();
            exportBuilderInput.FileName = "safiqZip.sql";
            exportBuilderInput.TableName = "cities";
            exportBuilderInput.SourceFile = @"E:\inp\safiqZip.txt";
            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exportBuilderInput.FileName));
        }

        [Test]
        public void ExportBuilderTest_SqlScriptNA1()
        {
            var exportBuilderInput = new InporterNa("NA1.sql", "na1", @"C:\Users\atakov.CLEANCODE\AppData\Roaming\Skype\My Skype Received Files\SMETKOPLAN_500.txt");
            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory,exportBuilderInput.FileName));
        }

        [Test]
        public void ExportBuilderTest_SqlScriptNA2()
        {
            var exportBuilderInput = new InporterNa("NA2.sql", "na2", @"C:\Users\atakov.CLEANCODE\AppData\Roaming\Skype\My Skype Received Files\SMETKI_BUDJET.txt");


            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exportBuilderInput.FileName));
        }
        [Test]
        public void ReadContosFromFile()
        {
            var year=ConfigTempoSinglenton.GetInstance().WorkDate.Year;
            ContoReader exportBuilderInput = new ContoReader();
            var res = ExportBuilder.SqlScriptString(exportBuilderInput);
            //SaveTofile(res, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exportBuilderInput.FileName));
        }

        private void SaveTofile(string finalString,string path)
        {
            
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(finalString);
            }
            //Process.Start(path);
        }

        [Test]
        public void ExportBuilderTest_GenerateCsv()
        {
            ExportBuilderInput exportBuilderInput = new ExportBuilderInput("clients.sql", "nom_17", @"E:\inp\KLIENTI.txt");
            

            var res = ExportBuilder.GenerateExportCSV(exportBuilderInput);
            
            Assert.AreEqual(res,true);
        }

        
    }
}
