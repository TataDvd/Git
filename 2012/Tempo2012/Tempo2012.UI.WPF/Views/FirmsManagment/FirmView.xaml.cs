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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using Tempo2012.UI.WPF.Views.Dnevnici;

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
                vm.CancelCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F2)
            {
                if (vm.Mode == EditMode.Add)
                {
                    vm.SaveCommand.Execute(null);
                }
                else
                {
                    vm.AddCommand.Execute(null);
                }
                e.Handled = true;
            }
            if (e.Key == Key.F3)
            {
                if (vm.Mode == EditMode.Edit)
                {
                    vm.SaveCommand.Execute(null);
                }
                else
                {
                    vm.UpdateCommand.Execute(null);
                }
                e.Handled = true;
            }
            if (e.Key == Key.F4)
            {
                vm.DeleteCommand.Execute(null);
                e.Handled = true;
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            if (but != null)
            {
                var firm = but.CommandParameter as FirmModelWraper;
                if (firm != null)
                {
                    DocGenerator.FirmaData(firm.CurrentFirma);
                }
            }
        }
    }
}
