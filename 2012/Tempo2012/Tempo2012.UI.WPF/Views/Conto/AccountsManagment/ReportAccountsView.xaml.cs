using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.AccountManagment;

namespace Tempo2012.UI.WPF.Views
{
    
    /// <summary>
    /// Interaction logic for MainAccountsView.xaml
    /// </summary>
    public partial class ReportAccountsView : UserControl
    {
        private ReportAccountsViewModel vm = new ReportAccountsViewModel();
        public ReportAccountsView()
        {
            InitializeComponent();
            DataContext = vm;
        }

        
        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
            }
            if (e.Key == Key.F5)
            {
                vm.ViewCommand.Execute(null);
            }
            if (e.Key == Key.F2)
            {
                vm.AddCommand.Execute(null);
            }
            if (e.Key == Key.F3)
            {
                vm.UpdateCommand.Execute(null);
            }
        }

        
    }
}
