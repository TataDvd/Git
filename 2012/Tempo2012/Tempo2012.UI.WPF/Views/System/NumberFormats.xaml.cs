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


namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for NumberFormats.xaml
    /// </summary>
    public partial class NumberFormats : Window
    {
        private NumberFormatsViewModel vm = new NumberFormatsViewModel();
        public NumberFormats()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.AddCommand.Execute(null);
            Close();
        }
    }
}
