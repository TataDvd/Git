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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tempo2012.UI.WPF.ViewModels;
using dragonz.actb.provider;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for FirmView.xaml
    /// </summary>
    public partial class FirmView : UserControl
    {
        private FirmViewModel vm = new FirmViewModel();
        public FirmView()
        {
            InitializeComponent();
            this.DataContext = vm;
            //city.AutoCompleteManager.AutoAppend = true;
            //List<string> test = new List<string>();
            //foreach (var item in vm.Cities)
            //{
            //    test.Add(item.Name);
            //}
            //city.AutoCompleteManager.DataProvider=new SimpleStaticDataProvider(test);
        }

        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
            }
            if (e.Key == Key.F6)
            {
                vm.ViewCommand.Execute(null);
            }
            if (e.Key == Key.F2)
            {
                vm.AddCommand.Execute(null);
            }
            if (e.Key == Key.F3)
            {
                vm.UpdateCommand.Execute(null);
            }
        }
    }
}
