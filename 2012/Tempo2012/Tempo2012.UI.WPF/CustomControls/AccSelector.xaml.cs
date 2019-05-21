using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.Views;

namespace Tempo2012.UI.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for AccSelector.xaml
    /// </summary>
    public partial class AccSelector : UserControl
    {
        // public static readonly DependencyProperty ShowExProperty =
        //  DependencyProperty.Register("ShowEx", typeof(bool), typeof(AccSelector), new
        //     PropertyMetadata(false, new PropertyChangedCallback(OnShowExChanged)));

        //private static void OnShowExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    AccSelector uc = d as AccSelector;
        //    uc.ShowEx = (bool)e.NewValue;
        //}

        private AccSelectorViewModel vm;
        private bool skiplostfokus;
        public AccSelector()
        {
            vm = new AccSelectorViewModel();
            InitializeComponent();
            DataContext = vm;
            this.Loaded += LoadedEv;
        }
        public AccSelector(bool withSum)
        {
            vm = new AccSelectorViewModel(withSum);
            InitializeComponent();
            DataContext = vm;
            this.Loaded += LoadedEv;
        }
        private void LoadedEv(object sender, RoutedEventArgs e)
        {
            EntryBoxEx.Focus();
        }

        private void SearchElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                skiplostfokus = true;
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsDebit(text);

            }
            if (e.Key == Key.F6)
            {
                _Popup.IsOpen = true;
                e.Handled = true;
            }
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            _Popup.IsOpen = true;
        }


        public AccountsModel Acc
        {
            get
            {
                if (vm.DAccountsModel != null) return vm.DAccountsModel;
                return null;
            }
        }
        public string Text
        {
            get
            {
                if (vm.DAccountsModel != null) return vm.DAccountsModel.Short;
                return "";
            }
        }
        public ObservableCollection<SaldoItem> ItemsDebit { get { return vm.ItemsDebit; } }

        private void Acc_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!skiplostfokus)
            {
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsDebit(text);
            }
            skiplostfokus = false;
        }

        public bool WithContragentSum
        {
            get { return vm.WithContragentSum; }
        }

        public bool ShowEx {
            get
            {
                return vm.ShowContragentSum; }
            set
            {
                vm.ShowContragentSum = value;
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selitem = ((sender as ListBox).SelectedItem as AccountsModel);
            if (vm != null) EntryBoxEx.Text = vm.LoadAnaliticDetailsDebit(selitem.Short);
            _Popup.IsOpen = false;
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((sender as ListBox).SelectedItem != null)
                {
                    var selitem = ((sender as ListBox).SelectedItem as AccountsModel);
                    if (vm != null) EntryBoxEx.Text = vm.LoadAnaliticDetailsDebit(selitem.Short);
                    _Popup.IsOpen = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _Popup.IsOpen = false;
        }
    }
}
