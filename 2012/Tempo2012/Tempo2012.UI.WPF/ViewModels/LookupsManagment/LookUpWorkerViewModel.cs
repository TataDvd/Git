using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.ViewModels.LookupsManagment
{
    public class LookUpRow
    {
        public string Field1 { get; set;}
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
    }

    public class LookUpWorkerViewModel : BaseViewModel, IItemsProvider<LookUpRow>
    {
        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value;OnPropertyChanged("Count"); }
        }

        private string _tableName;
       

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; OnPropertyChanged("TableName"); }
        }
 
        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; OnPropertyChanged("Filter"); }
        }

        public int FetchCount()
        {
            return Count;
        }

        public IList<LookUpRow> FetchRange(int startIndex, int count)
        {
            var rows = Context.GetLookupDictionary(TableName, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Filter,
                                                   string.Format(" First {0} Skip {1} ", startIndex, count));
            List<LookUpRow> rez=new List<LookUpRow>();
            foreach (Dictionary<string, object> dictionary in rows)
            {
                rez.Add(new LookUpRow
                                 {
                                     Field1 = dictionary["Field1"].ToString(),
                                     Field2 = dictionary["Field2"].ToString(),
                                     Field3 = dictionary["Field3"].ToString(),
                                     Field4 = dictionary["Field4"].ToString(),
                                     Field5 = dictionary["Field5"].ToString()
                                 });
            }
            return rez;
        }
    }
}
