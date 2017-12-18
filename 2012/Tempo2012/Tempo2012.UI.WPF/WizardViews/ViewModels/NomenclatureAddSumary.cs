using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    class NomenclatureAddSumary:WizardStepBase
    {
        public override string DisplayName
        {
            get { return "Резюме";}
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
