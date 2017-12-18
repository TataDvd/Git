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
using Tempo2012.UI.WPF.ViewModels.Saldos;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for saldos.xaml
    /// </summary>
    public partial class Saldos : Window
    {
        private SaldosViewModel vm=new SaldosViewModel();
        public Saldos(int lookupkey,int currentAccId)
        {
            vm = new SaldosViewModel(lookupkey, currentAccId);
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveCommand.Execute(sender);
            Close();
        }
    }
}
