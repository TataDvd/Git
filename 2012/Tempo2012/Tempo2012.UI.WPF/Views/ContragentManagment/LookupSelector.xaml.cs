using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Tempo2012.UI.WPF.Views.ContragentManagment
{
    /// <summary>
    /// Interaction logic for ContragenSelector.xaml
    /// </summary>
    public partial class LookUpSelector : Window
    {
        private LookUpSelectorViewModel vm;
        public LookUpSelector(SaldoItem fields)
        {
            vm = new LookUpSelectorViewModel(fields);
            DataContext = vm; 
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded); 
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
            Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
            args.RoutedEvent = Keyboard.KeyDownEvent;
            InputManager.Current.ProcessInput(args);
            this.Loaded -= MainWindow_Loaded;
        }
        public ObservableCollection<string> SelectedItem
        {
            get { return vm.SelectedItem; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = sender as TextBox;
            if (text != null)
            {
                var tag = text.Tag as Filter;
                vm.Refresh(tag);
                dg.SelectedIndex = 0;
                dg.Items.Refresh();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key==Key.Down)
            {
               Dg.SelectRowByIndex(dg,0);
            }
        }

        private void dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void dg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DialogResult = true;
                Close();
            }
        }

        

        
    }
}
