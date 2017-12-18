using System;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{ 
    
    public class HoldingViewModel:BaseViewModel
    {
        public HoldingViewModel(HoldingModel holding)
        {
            HoldingModel = holding;
        }

        public HoldingModel HoldingModel
        {
            get
            {
                if (_holdingModel == null)
                {
                    _holdingModel=new HoldingModel();
                }
                return _holdingModel;
            }
            set
            {
                _holdingModel = value;
            }
        }

        private string _ipServer;
        private string _name;
        private int _nom;
        private string _template;
        private HoldingModel _holdingModel;

        public int Nom
        {
            get { return HoldingModel.Nom; }
            set { HoldingModel.Nom = value;OnPropertyChanged("Nom"); }
        }

        public string Name
        {
            get { return HoldingModel.Name; }
            set { HoldingModel.Name = value;OnPropertyChanged("Name"); }
        }

        public string IpServer
        {
            get { return HoldingModel.IpServer; }
            set { HoldingModel.IpServer = value; OnPropertyChanged("IpServer"); }
        }
        
        public string Template
        {
            get { return HoldingModel.Template; }
            set { HoldingModel.Template = value; OnPropertyChanged("Template"); }
        }
        public string ConnectionString
        {
            get { return HoldingModel.ConectionString; }
            set { HoldingModel.ConectionString = value; OnPropertyChanged("ConnectionString"); }
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}",Name,IpServer);
        }

        
    }
}