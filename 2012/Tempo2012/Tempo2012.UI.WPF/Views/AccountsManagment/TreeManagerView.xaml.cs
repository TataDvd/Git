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
using Tempo2012.UI.WPF.ViewModels.treeviewmodel;
using Tempo2012.UI.WPF.Views.Dialogs;

namespace Tempo2012.UI.WPF.Views
{
    
    /// <summary>
    /// Interaction logic for MainAccountsView.xaml
    /// </summary>
    public partial class TreeManagerView : UserControl
    {
        private TreeManagerViewModel vm = new TreeManagerViewModel();
        public TreeManagerView()
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
                e.Handled = true;
            }
            if (e.Key == Key.F6)
            {
                vm.ViewCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F8)
            {
                vm.AddNewCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F2)
            {
                vm.AddCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F3)
            {
                vm.UpdateCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void tvSuppliers_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                var treeViewModel = treeView.SelectedItem as TreeViewModel;
                if (treeViewModel != null)
                    vm.CurrentAccount = treeViewModel.CurrAcc;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
             if (e.ClickCount==2)
             {
                 if (vm.CurrentAccount.Num != 0)
                 {
                     vm.EditFromOutside();
                 }
             }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
             vm.AddSubAcc();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             var sen = (sender as Button);
             if (sen != null)
             {
                 AccountsModel cp = (sen.CommandParameter as TreeViewModel).CurrAcc;
                 if (cp.Num > 0)
                 {
                     if (cp.AnaliticalNum > 0)
                     {
                         vm.Saldo(cp);
                     }
                     else
                     {
                         Saldoss s=new Saldoss(cp);
                         s.ShowDialog();
                     }
                 }
             }
        }
    }
}
