﻿using System;
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
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views
{
    
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        private AccountsViewModel vm=new AccountsViewModel();
        public AccountsView()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                string[] spliter=(sender as ComboBox).SelectedItem.ToString().Split('-');
                accountnum.Text=spliter[0];
                accountname.Text = spliter[1];
                vm.Num = int.Parse(spliter[0]);
                vm.NameMain = spliter[1];
                //if (Details.SelectedIndex <= -1) 
                //{
                //    vm.AllAccounts[Details.SelectedIndex] = vm.Num;
                //    Details.ItemsSource = vm.AllAccounts;
                //}
                
            }
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
    }
}
                               