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
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectFirm.xaml
    /// </summary>
    public partial class SelectFirm : Window
    {
        private FirmViewModel vm = new FirmViewModel();
        public SelectFirm()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            DialogResult = true;
        }

        public FirmModel CurrentFirma
        {
            get 
            {
                if (vm.CurrentFirmaWraper != null) 
                    return vm.CurrentFirmaWraper.CurrentFirma;
                return null;
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                Close();
                e.Handled = true;
            }
        }

        
    }
}
