using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportBuilder
{
    public interface IReportBuilder
    {
        List<List<string>> GetItems();
        List<string> GetTitles();
        List<string> GetHeader();
        List<string> GetFuther();
        string Filename { get; }
        string Title { get; set;}
        string SubTitle { get; set;}
        IEnumerable<ReportItem> ReportItems { get; set;}
        List<string> GetSubTitles();
        List<List<string>> GetTXTAntetka();
        DateTime FromDate { get; set;}
        DateTime ToDate{ get; set;}
        void LoadSettings(string Path);
        void SaveSettings(string Path);

        Dictionary<int, List<string>> Rowfoother { get; set;}
    }
}
