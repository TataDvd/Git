using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DataGrid2DLibrary;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    /// <summary>
    /// Interaction logic for DetailsUniverse.xaml
    /// </summary>
    public partial class DetailsUniverse : Window
    {
        private DetailsUniverseViewModel vm;
        public DetailsUniverse(AccountsModel dAccountsModel,string filter,ContoViewModel cvm,int tip,EditMode mode)
        {
            
            vm=new DetailsUniverseViewModel(dAccountsModel,filter,cvm,tip,mode);
            InitializeComponent();
            DataContext = vm;
        }

        private void Dg_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vm.CurrentRowIndex >= 0)
            {
                SelectedRow = vm.Fields[vm.CurrentRowIndex+1];
                Close();
            }
        }
        public List<string> SelectedRow { get; set;}

        private void OnlySaldo_OnClick(object sender, RoutedEventArgs e)
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

        private void ContoSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (dg != null && dg.SelectedItems != null)
            {
                
                    var detailsUniverseViewModel = DataContext as DetailsUniverseViewModel;
                    if (detailsUniverseViewModel != null)
                        detailsUniverseViewModel.SaveContos(dg.SelectedItems);
                
                var detailsUniverseViewModel1 = DataContext as DetailsUniverseViewModel;
                if (detailsUniverseViewModel1 != null)
                {
                    detailsUniverseViewModel1.Clear();
                }
                Close();
            }
        }

        private void Dg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid2D;
            var detailsUniverseViewModel = DataContext as DetailsUniverseViewModel;
            if (detailsUniverseViewModel != null)
            {
                detailsUniverseViewModel.Suma = 0;
                detailsUniverseViewModel.SumaVal = 0;
            }
            if (grid != null && grid.SelectedItems != null)
            {
                foreach (var item in grid.SelectedItems)
                {
                    var citem = item as System.Data.DataRowView;
                    if (citem != null)
                    {
                        Ref<string> sum = citem.Row.ItemArray[citem.Row.ItemArray.Length - 1] as Ref<string>;
                        if (detailsUniverseViewModel != null)
                        {
                            if (sum != null) detailsUniverseViewModel.Suma += decimal.Parse(sum.Value);
                            if (detailsUniverseViewModel.Acc.Kol > 0 || detailsUniverseViewModel.Acc.Val > 0)
                            {
                                Ref<string> valsum = citem.Row.ItemArray[citem.Row.ItemArray.Length - 5] as Ref<string>;
                                if (valsum != null) detailsUniverseViewModel.SumaVal += decimal.Parse(valsum.Value);
                            }
                        }
                    }
                    
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = sender as TextBox;
                if (text != null)
                {
                    var tag = text.Tag as ViewModels.ContragenManager.Filter;
                    if (!string.IsNullOrWhiteSpace(tag.FilterWord)){
                        int field = 0;
                        foreach (var item in vm.Fields[0])
                        {
                            if (item == tag.FilterName)
                            {
                                vm.Fields = new List<List<string>>(vm.Fields.Where(ee => ee[field].Contains(tag.FilterWord) || ee[field] == tag.FilterName).ToList());
                                vm.UpdateProperty();
                            }
                            field++;
                        }
                    }
                    else
                    {
                        vm.All();
                    }
                }
                
            }
        }
    }

    
}
