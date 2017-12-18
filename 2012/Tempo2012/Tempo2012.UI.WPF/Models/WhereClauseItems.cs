using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ReportBuilder;

namespace Tempo2012.UI.WPF.Models
{
    public class WhereClauseItems : INotifyPropertyChanged
    {
        public IEnumerable<WhereClauseItem> Items { get; set;}
        public IEnumerable<ReportItem> ShowItems { get; set;}
        public string View { get; set;}

        public override string  ToString()
        {
            return string.Format("{0} {1} {2}",ShowTtemsToString(),View,ItemsToString());
        }

        private string ItemsToString()
        {
            if (Items != null && Items.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" where ");
                string globalop = "";
                int br = 0;
                foreach (WhereClauseItem whereClauseItem in Items)
                {
                    if (br == 0)
                    {
                        globalop = "";
                    }
                    else
                    {
                        globalop = whereClauseItem.GlobalOperator;
                    }
                    sb.AppendFormat(" {0} {1}", globalop, whereClauseItem);
                    br++;
                }
                return sb.ToString();
            }
            return "";
        }

        private string ShowTtemsToString()
        {
            if (ShowItems != null && ShowItems.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Select ");
                string coma = "";
                int br = 0;
                foreach (var showitem in ShowItems)
                {
                    coma = br==ShowItems.Count()-1 ? "" : ",";
                    if (showitem.IsVisible)
                        sb.AppendFormat(" {0} {1}",showitem.DbField,coma);
                    br++;
                }
                sb.Append(" from ");
                return sb.ToString();
            }
            return "select * from";
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}