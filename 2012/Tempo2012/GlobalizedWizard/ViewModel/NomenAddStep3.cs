using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoffeeLibrary;

namespace GlobalizedWizard.ViewModel
{
    class NomenAddStep3 : NomenWizardPageViewModelBase
    {
        public NomenAddStep3(NomenclatureWizardModel nomenclatureWizardModel) : base(nomenclatureWizardModel)
        {
            
        }
        public override string DisplayName
        {
            get { return "Финал";}
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
