using System.ComponentModel;
namespace ReportBuilder
{
    [System.Serializable]
    public class ReportItem
    {
        private string _name;
        public virtual string Name
        {
            get { return _name; }
            set { _name = value;OnPropertyChanged("Name"); }
        }

        private bool _isVisible;
        public virtual bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value;OnPropertyChanged("IsVisible"); }
        }

        private string _dbField;
        public virtual string DbField
        {
            get { return _dbField; }
            set { _dbField = value;OnPropertyChanged("DbField"); }
        }

        private int _size;
        public virtual int Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged("Size"); }
        }

        private string _filter;
        public virtual string Filter
        {
            get { return _filter; }
            set { _filter = value;OnPropertyChanged("Filter"); }
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

        private int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }

        private bool _isShow;
        public bool IsShow
        {
            get { return _isShow; }
            set { _isShow = value;OnPropertyChanged("IsShow"); }
        }

        private int _height;
        private bool _isSuma;
        private bool _sborno;

        public int Height
        {
            get { return _height; }
            set { _height = value;OnPropertyChanged("Height"); }
        }

        public bool IsSuma
        {
            get { return _isSuma; }
            set { _isSuma = value; }
        }

        public bool Sborno
        {
            get { return _sborno; }
            set { _sborno = value; }
        }
    }
}