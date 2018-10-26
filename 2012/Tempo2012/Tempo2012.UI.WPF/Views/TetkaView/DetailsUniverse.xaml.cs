﻿using System;
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
            if (detailsUniverseViewModel != null) detailsUniverseViewModel.Suma = 0;
            if (grid != null && grid.SelectedItems != null)
            {
                foreach (var item in grid.SelectedItems)
                {
                    var citem = item as System.Data.DataRowView;
                    if (citem != null)
                    {
                        Ref<string> sum = citem.Row.ItemArray[citem.Row.ItemArray.Length - 1] as Ref<string>;
                        if (detailsUniverseViewModel != null)
                            if (sum != null) detailsUniverseViewModel.Suma += decimal.Parse(sum.Value);
                    }
                }
            }
        }
    }

    
}
