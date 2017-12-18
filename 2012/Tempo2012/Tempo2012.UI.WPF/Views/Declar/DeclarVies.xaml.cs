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
using Tempo2012.UI.WPF.ViewModels.Deklar;

namespace Tempo2012.UI.WPF.Views.Declar
{
    /// <summary>
    /// Interaction logic for DeclarConfig.xaml
    /// </summary>
    public partial class DeclarVies : Window
    {
        DeclarsViewModel vm = new DeclarsViewModel();
        public DeclarVies()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
