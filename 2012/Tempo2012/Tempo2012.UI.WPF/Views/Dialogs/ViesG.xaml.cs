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
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Auto60.xaml
    /// </summary>
    public partial class ViesG : Window
    {
        private ViesGvm vm;
        public ViesG()
        {
            vm = new ViesGvm(); 
            InitializeComponent();
            DataContext = vm;
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Dg.SelectRowByIndex(dg, 0);
            }
        }


        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = sender as TextBox;
                if (text != null)
                {
                    var tag = text.Tag as Filter;
                    vm.Refresh(tag);
                    dg.Items.Refresh();
                }
            }
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            b.ContextMenu.DataContext = b.DataContext;
            b.ContextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteAllItemsFirma();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            vm.DeleteAllItems();
        }
    }
}
