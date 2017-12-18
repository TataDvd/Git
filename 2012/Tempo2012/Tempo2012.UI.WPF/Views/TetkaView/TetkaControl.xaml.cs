using System.Windows;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.Tetka;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    /// <summary>
    /// Interaction logic for TetkaControl.xaml
    /// </summary>
    public partial class TetkaControl : Window
    {
        private TetkaViewModel vm;
        public TetkaControl(AccountsModel accountsModel)
        {
            vm=new TetkaViewModel(accountsModel);
            DataContext = vm;
            InitializeComponent();
        }
    }
}
