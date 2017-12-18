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
using Tempo2012.UI.WPF.ViewModels.Dnevnici;

namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    /// <summary>
    /// Interaction logic for DdsPurchasesView.xaml
    /// </summary>
    public partial class Oborotki : Window
    {
        private OborotkiViewModel vm = new OborotkiViewModel();
        public Oborotki()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
