using System.Windows;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.Tetka;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    /// <summary>
    /// Interaction logic for TetkaControl.xaml
    /// </summary>
    public partial class FacturaContragentControl : Window
    {
        private FacturaControlInfoViewModel vm;
      
        public FacturaContragentControl(AccountsModel DAccountsModel, string nomerf, string contr)
        {
            // TODO: Complete member initialization
            vm = new FacturaControlInfoViewModel(DAccountsModel,nomerf,contr);
            DataContext = vm;
            InitializeComponent();
        }

        public FacturaContragentControl(AccountsModel DAccountsModel, string contr)
        {
            vm = new FacturaControlInfoViewModel(DAccountsModel,contr);
            DataContext = vm;
            InitializeComponent();
        }
    }
}
