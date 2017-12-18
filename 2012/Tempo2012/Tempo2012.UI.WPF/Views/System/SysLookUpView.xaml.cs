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
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for LookUpSpecific.xaml
    /// </summary>
    public partial class SysLookUpView: UserControl 
    {
        private SysLookUpViewModel vm=new SysLookUpViewModel();
        public SysLookUpView()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = sender as TextBox;
            if (text != null)
            {
                var tag = text.Tag as Filter;
                vm.Refresh(tag);
                dg.Items.Refresh();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Dg.SelectRowByIndex(dg, 0);
            }
        }
        
    }
}
