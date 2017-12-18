using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CopyAccsFromFirmToFirm.xaml
    /// </summary>
    public partial class CopyLookupFromFirmToFirm : Window
    {
        private CopyLookupFromFirmToFirmViewModel vm = new CopyLookupFromFirmToFirmViewModel();
        public CopyLookupFromFirmToFirm()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (vm.CopyCommand.CanExecute(null))
            {
                if (dataGrid2D.SelectedItems != null && dataGrid2D.SelectedItems.Count > 0)
                {
                    vm.SFields = new ObservableCollection<ObservableCollection<string>>();
                    foreach (var item in dataGrid2D.SelectedItems)
                    {
                        var iii = item as DataRowView;
                        var newrow = new ObservableCollection<string>();
                        if (iii != null)
                        {
                            foreach (var o in iii.Row.ItemArray)
                            {
                                newrow.Add((o as DataGrid2DLibrary.Ref<System.String>).Value);
                            }
                            vm.SFields.Add(newrow);
                        }
                    }
                    vm.CopyCommand.Execute(null);
                }
            }
            else
            {
                MessageBoxWrapper.Show("Не е избрана фирма, от която да се копират номенклатури");
            }
        }
    }
}
