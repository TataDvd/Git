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

namespace Tempo2012.UI.WPF.Views.Framework
{
    /// <summary>
    /// Interaction logic for HoldingSelector.xaml
    /// </summary>
    public partial class HoldingSelector : Window
    {
        readonly HoldingSelectorViewModel _vm=new HoldingSelectorViewModel();
        public HoldingSelector()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public HoldingViewModel Holding
        {
            get
            {
                return _vm.Holding;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
