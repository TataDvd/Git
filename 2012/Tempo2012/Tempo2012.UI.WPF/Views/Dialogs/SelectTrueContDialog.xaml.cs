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
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectAccDialog.xaml
    /// </summary>
    public partial class SelectTrueContDialog : Window
    {
        public ObservableCollection<SaldoItem> ItemsDebit { get; set; }
        public string Acc { get; set;}
        public SelectTrueContDialog()
        {
            InitializeComponent();
        }

         

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            ItemsDebit = cccName.ItemsDebit;
            Close();
        }

      
    }
}
