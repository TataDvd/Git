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
using Tempo2012.UI.WPF.ViewModels.Saldos;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for saldos.xaml
    /// </summary>
    public partial class Saldoss : Window
    {
        private SaldosViewModel vm=new SaldosViewModel();
        public Saldoss(AccountsModel acc)
        {
            vm = new SaldosViewModel(acc);
            InitializeComponent();
            DataContext = vm;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    vm.SaveCommand.Execute(sender);
        //    Close();
        //}
        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
                e.Handled = true;
            }

            if (e.Key == Key.F2)
            {
                vm.SaveCommand.Execute(sender);
                this.DialogResult = true;
                this.Close();
            }

        }

        //private void CustomControls:TextBoxEx_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.D0:
        //        case Key.D1:
        //        case Key.D2:
        //        case Key.D3:
        //        case Key.D4:
        //        case Key.D5:
        //        case Key.D6:
        //        case Key.D7:
        //        case Key.D8:
        //        case Key.D9:
        //        case Key.NumLock:
        //        case Key.NumPad0:
        //        case Key.NumPad1:
        //        case Key.NumPad2:
        //        case Key.NumPad3:
        //        case Key.NumPad4:
        //        case Key.NumPad5:
        //        case Key.NumPad6:
        //        case Key.NumPad7:
        //        case Key.NumPad8:
        //        case Key.NumPad9:
        //        case Key.Back:
        //            break;
        //        default:
        //            e.Handled = true;
        //            break;
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveCommand.Execute(sender);
            Close();
        }
    }
}
