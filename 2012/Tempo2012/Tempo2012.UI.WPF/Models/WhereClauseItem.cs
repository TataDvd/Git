using System.ComponentModel;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class WhereClauseItem:INotifyPropertyChanged
    {
        public virtual string GlobalOperator { get; set;}
        public virtual string DbField { get; set;}
        public virtual string SqlOperator { get; set;}
        public virtual string Value { get; set;}
        public override string  ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append(DbField);
            sb.Append(SqlOperator);
            sb.Append(Value);
            return sb.ToString();
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