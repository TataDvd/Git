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
using System.Windows.Shapes;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for MainAcc.xaml
    /// </summary>
    public partial class MainAcc : Window
    {
        private AccountsDialogViewModel vm;
        public MainAcc(AccountsModel accounts)
        {
            InitializeComponent();
            vm = new AccountsDialogViewModel(accounts);
            if (accounts.SubNum!=0)
            {
                vm.ShowMain = false;
            }
            else
            {
                vm.ShowMain = true;
            }
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
            if (e.Key == Key.F6)
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
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                string[] spliter = (sender as ComboBox).SelectedItem.ToString().Split('-');
                accountnum.Text = spliter[0];
                string text = spliter[1];
                if (spliter.Length > 1)
                {
                    for (int i = 2; i < spliter.Length; i++)
                    {
                        text += "-" + spliter[i];
                    }
                };
                vm.Num = int.Parse(spliter[0]);
                vm.NameMain = text;
                

            }
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            SetUpMapAnaliticAccToLookup ds = new SetUpMapAnaliticAccToLookup(vm.CurrentSelectedAnaliticalField);
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                var setUpMapAnaliticAccToLookupViewModel = ds.DataContext as SetUpMapAnaliticAccToLookupViewModel;
                if (setUpMapAnaliticAccToLookupViewModel != null)
                {
                    vm.CurrentSelectedAnaliticalField = setUpMapAnaliticAccToLookupViewModel.WorkedItem;
                    Mapper.Items.Refresh();
                   
                }
            }
        }

        
    }
}
