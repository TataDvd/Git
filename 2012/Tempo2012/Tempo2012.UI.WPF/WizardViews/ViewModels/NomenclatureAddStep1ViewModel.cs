using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    public class NomenclatureAddStep1ViewModel:WizardStepBase
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
        

        public override string DisplayName
        {
            get { return "Стъпка 1";}
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }
    }
}
