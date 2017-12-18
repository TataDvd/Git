using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using Tempo2012.UI.WPF.ViewModels.Saldos;
using Tempo2012.UI.WPF.Views.Saldos;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for saldos.xaml
    /// </summary>
    public partial class SaldosAnalitic : Window
    {
        private SaldosAnaliticViewModel vm = new SaldosAnaliticViewModel();
        public SaldosAnalitic()
        {
            vm = new SaldosAnaliticViewModel();
            InitializeComponent();
            DataContext = vm;
        }

        public SaldosAnalitic(AccountsModel accountsModel, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> observableCollection, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> observableCollection_2, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> staticFields)
        {
            vm = new SaldosAnaliticViewModel(observableCollection, observableCollection_2, staticFields, accountsModel);
            InitializeComponent();
            DataContext = vm;
        }

        public SaldosAnalitic(AccountsModel CurrentAccount)
        {
            vm = new SaldosAnaliticViewModel(CurrentAccount);
            InitializeComponent();
            DataContext = vm;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = sender as TextBox;
                if (text != null)
                {
                    var tag = text.Tag as Filter;
                    vm.Refresh(tag);
                    dataGrid2D.Items.Refresh();
                }
            }
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = sender as TextBox;
                if (text != null)
                {
                    var tag = text.Tag as Filter;
                    vm.Refresh(tag);
                    dataGrid2D.Items.Refresh();
                }
            }
        }
    }
}
