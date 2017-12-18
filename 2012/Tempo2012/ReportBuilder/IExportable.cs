using System.Collections.Generic;

namespace ReportBuilder
{
    public interface IExportable
    {
        List<List<string>> GetAllItems();
        List<IDbFields> GetAllFields();
        string FileName { get; set;}
        string TableName { get; set;}
        string SourceFile { get; set; }
    }

   
}