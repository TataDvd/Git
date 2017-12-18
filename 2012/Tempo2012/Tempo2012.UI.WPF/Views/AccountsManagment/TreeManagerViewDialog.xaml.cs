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

namespace Tempo2012.UI.WPF.Views
{
    
    /// <summary>
    /// Interaction logic for MainAccountsView.xaml
    /// </summary>
    public partial class TreeManagerViewDialog : Window
    {
        private TreeManagerViewModel vm = new TreeManagerViewModel();
        public TreeManagerViewDialog()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                Close();
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
                 DialogResult = true;
                 Close();
             }
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        public AccountsModel CurrentAcc
        {
            get { return vm.CurrentAccount; }
        }
    }
}
