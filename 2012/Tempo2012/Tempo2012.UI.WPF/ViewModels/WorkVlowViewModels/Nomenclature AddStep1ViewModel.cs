using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.WorkFlowViews;

namespace Tempo2012.UI.WPF.ViewModels.WorkVlowViewModels
{
    public class NomenclatureAddStep1ViewModel:ViewModelBase
    {
        public  NomenclatureHedar Workitem = new NomenclatureHedar{Description="",Name="Номенклатура1",Id=1};
        public string Name
        {
            get { return Workitem.Name; }
            set
            {
                Workitem.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return Workitem.Description; }
            set
            {
                Workitem.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }

        public ICommand NextCommand
        {
            get { return new DelegateCommand((o)=>NextAction(),(o)=>IsValid()); }
        }

        private void NextAction()
        {
            Step2AddNomenclatures step2 = new Step2AddNomenclatures();
            step2.ShowDialog();
        }
    }
}
