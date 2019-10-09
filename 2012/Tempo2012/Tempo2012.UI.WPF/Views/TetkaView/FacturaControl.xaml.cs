using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.Tetka;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    /// <summary>
    /// Interaction logic for TetkaControl.xaml
    /// </summary>
    public partial class FacturaControl : Window
    {
        private FacturaControlViewModel vm;
        public FacturaControl(AccountsModel accountsModel,ContoViewModel cv)
        {
            vm = new FacturaControlViewModel(accountsModel,cv,true);
            DataContext = vm;
            InitializeComponent();
        }
        public FacturaControl(AccountsModel accountsModel,ContoViewModel cv,string contr=null)
        {
            vm = new FacturaControlViewModel(accountsModel,cv,true,contr);
            DataContext = vm;
            InitializeComponent();
        }
        public FacturaControl(AccountsModel accountsModel, ContoViewModelLight cv)
        {
            vm = new FacturaControlViewModel(accountsModel, cv, true);
            DataContext = vm;
            InitializeComponent();
        }
        public FacturaControl(AccountsModel accountsModel, ContoViewModelLight cv, string contr = null)
        {
            vm = new FacturaControlViewModel(accountsModel, cv, true, contr);
            DataContext = vm;
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectedItem = vm.AccItemSaldo;
            Close();
        }

        public AccItemSaldo SelectedItem { get; set; }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            vm.Suma = 0;
            if (grid != null && grid.SelectedItems != null)
            {
                foreach (var item in grid.SelectedItems)
                {
                    var citem = item as AccItemSaldo;
                    if (citem != null)
                    {
                        vm.Suma += citem.Ksd;
                    }
                }
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var sendy = sender as CheckBox;
            if (sendy != null)
            {
                if (sendy.IsChecked != null && sendy.IsChecked.Value)
                {
                    vm.Filter();
                    

                }
                else
                {
                    vm.All();
                   
                }
            }
        }

        private void Base_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (Dg != null && Dg.SelectedItems != null)
            {
                foreach (var item in Dg.SelectedItems)
                {
                    vm.SaveConto(item);
                }
                Close();
            }
            
        }
    }
}
