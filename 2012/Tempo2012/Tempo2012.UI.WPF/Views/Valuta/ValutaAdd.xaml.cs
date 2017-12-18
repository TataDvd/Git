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
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Tempo2012.UI.WPF.Views.Valuta
{
    /// <summary>
    /// Interaction logic for ValutaAddorEdit.xaml
    /// </summary>
    public partial class ValutaAdd : Window
    {
        ValutaAddViewMode vm;
        public ValutaAdd()
        {
            InitializeComponent();
            vm = new ValutaAddViewMode(DateTime.Now,"eur");
            DataContext = vm;
        }
        public ValutaAdd(DateTime date,string CodeValuata)
        {
            InitializeComponent();
            vm = new ValutaAddViewMode(date,CodeValuata);
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.AddCommand.Execute(sender);
            this.DialogResult = true;
            Close();
        }

        public decimal Kurs { get { return vm.Kurs; } set { vm.Kurs = value;}}

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Bu.Focus();
                vm.AddCommand.Execute(sender);
                this.DialogResult = true;
                Close();
            }
        }
    }
}
