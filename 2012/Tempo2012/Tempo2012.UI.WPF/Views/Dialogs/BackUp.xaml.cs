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

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for BackUp.xaml
    /// </summary>
    public partial class BackUp : Window
    {
        public BackUp()
        {
            BackUpViewModel vm = new BackUpViewModel();
            InitializeComponent();
            DataContext = vm;
        }
    }
}
