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
    public partial class SelectAccDialog : Window
    {
        public ObservableCollection<SaldoItem> ItemsDebit { get; set; }
        public string Acc { get; set;}
        public bool WithContragentSum { get { return accName.WithContragentSum;} }

        public bool ShowEx { get; set; }

        public SelectAccDialog()
        {
            InitializeComponent();
        }

        public SelectAccDialog(bool showacc)
        {
            InitializeComponent();
            accName.ShowEx = showacc;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Acc = accName.Text;
            ItemsDebit = accName.ItemsDebit;
            Close();
        }

        private void AccName_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                Acc = accName.Text;
                 
                Close();
            }
        }
    }
}
