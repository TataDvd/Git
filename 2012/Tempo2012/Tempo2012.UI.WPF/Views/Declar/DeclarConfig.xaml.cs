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
    public partial class DeclarConfig : Window
    {
        DeclarsViewModel vm = new DeclarsViewModel();
        public DeclarConfig()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
            }
        }
    }
}
