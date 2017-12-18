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
    public partial class ChoiserValutaandAcc : Window
    {
        public ChoiserValutaandAcc()
        {
            InitializeComponent();
        }

        private void Do_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Acc = cccValuta.EntryBoxEx.Text;
            foreach (var a in cccValuta.ItemsDebit)
            {
                if (a.Name == "Вид валута")
                {
                    VidV = a.Value;
                }
            }
            CodeClient= cccValuta.ItemsDebit[0].Value;
            Client= cccValuta.ItemsDebit[0].Lookupval;
            Close();
        }

        public string VidV { get; set; }
        public string Acc { get; set; }
        public string CodeClient { get;  set; }
        public string Client { get;  set; }
    }
}
