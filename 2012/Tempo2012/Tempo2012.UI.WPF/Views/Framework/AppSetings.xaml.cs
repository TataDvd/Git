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
    /// Interaction logic for AppSetings.xaml
    /// </summary>
    public partial class AppSetings : Window
    {
        AppSettingsViewModel vm=new AppSettingsViewModel();
        public AppSetings()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            vm.UpdateCommand.Execute(null);
            Close();
        }
    }
}
