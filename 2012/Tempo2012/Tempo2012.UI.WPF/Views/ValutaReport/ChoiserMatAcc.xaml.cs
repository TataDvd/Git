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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Tempo2012.UI.WPF.Views.ValutaReport
{
    /// <summary>
    /// Interaction logic for ChoiserValutaandAcc.xaml
    /// </summary>
    public partial class ChoiserMatAcc : Window
    {
        public ChoiserMatAcc()
        {
            InitializeComponent();
        }

        private void Do_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Acc = cccValuta.EntryBoxEx.Text;
            Sklad = cccValuta.ItemsDebit[0].Value;
            CodeMaterial = cccValuta.ItemsDebit[1].Value;
            Material = cccValuta.ItemsDebit[1].Lookupval;
            Close();
        }

        public string Sklad { get; set; }
        public string Acc { get; set; }
        public string CodeMaterial { get;  set; }
        public string Material { get;  set; }
    }
}
