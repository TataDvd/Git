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

namespace Tempo2012.UI.WPF.Views.Periods
{
    /// <summary>
    /// Interaction logic for Periods.xaml
    /// </summary>
    public partial class Periods : Window
    {
        public Periods()
        {
            InitializeComponent();
            DataContext = new PeriodsViewModel();
        }
    }
}
