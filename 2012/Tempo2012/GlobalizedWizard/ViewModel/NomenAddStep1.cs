using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoffeeLibrary;

namespace GlobalizedWizard.ViewModel
{
    class NomenAddStep1 : NomenWizardPageViewModelBase
    {
        public NomenAddStep1(NomenclatureWizardModel nomenclatureWizardModel) : base(nomenclatureWizardModel)
        {
            
        }
        public override string DisplayName
        {
            get { return "Стъпка1";}
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(NomenclatureWizardModel.Name);
        }
    }
}
