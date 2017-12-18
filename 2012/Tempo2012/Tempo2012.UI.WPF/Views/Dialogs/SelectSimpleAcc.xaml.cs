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
using Tempo2012.UI.WPF.CustomControls;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectSimpleAcc.xaml
    /// </summary>
    public partial class SelectSimpleAcc : Window
    {
        private AccSelectorViewModel vm;
        public SelectSimpleAcc()
        {
            vm = new AccSelectorViewModel();
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void EntryAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                TreeManagerViewDialog sf = new TreeManagerViewDialog();
                sf.ShowDialog();
                if (sf.DialogResult.HasValue && sf.DialogResult.Value)
                {
                    if (sf.CurrentAcc != null) vm.DAccountsModel = sf.CurrentAcc;
                    EntryAcc.Text = vm.DAccountsModel.Short;
                }
                e.Handled = true;
            }
        }

        public string Acc { get { return EntryAcc.Text; } set { EntryAcc.Text = value; } }
    }
}
