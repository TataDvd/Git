namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    /// <summary>
    /// The first wizard page in the workflow.  
    /// This page has no functionality.
    /// </summary>
    public class NomenclatureWelcomePageViewModel : WizardStepBase
    {
        public NomenclatureWelcomePageViewModel()
            : base(null)
        {
        }

        public override string DisplayName
        {
            get { return "Помощник за добавяне на номернклатури";}
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}