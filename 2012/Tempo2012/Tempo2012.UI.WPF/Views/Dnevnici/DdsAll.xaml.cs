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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    /// <summary>
    /// Interaction logic for DdsSellsView.xaml
    /// </summary>
    public partial class DdsAll : Window
    {
        private DdsAllModel vm;
        public decimal SumaDds { get; set;}
        public DdsAll()
        {
            vm = new DdsAllModel();
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("PURCHASES");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("PURCHASESF");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("SALES");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("SALESF");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("DECLAR");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("DECLARF");
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("VIES");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("VIESF");
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            
            vm.GenreteDocAsync("ALL");
           
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            vm.GenreteDocAsync("ALLF");
        }
       
    }
}
