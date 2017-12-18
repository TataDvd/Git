using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoffeeLibrary;

namespace GlobalizedWizard.ViewModel
{
    class NomenAddStep0 : NomenWizardPageViewModelBase
    {
        public NomenAddStep0(NomenclatureWizardModel nomenclatureWizardModel) : base(nomenclatureWizardModel)
        {
            
        }
        public override string DisplayName
        {
            get { return "Здравей"; }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
