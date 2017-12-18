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
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.Views;

namespace Tempo2012.UI.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for AccSelector.xaml
    /// </summary>
    public partial class ContrSelector : UserControl
    {
        private ContrSelectorViewModel vm;
        public ContrSelector()
        {
            vm = new ContrSelectorViewModel();
            InitializeComponent();
            DataContext = vm;
           // this.Loaded += LoadedEv;
        }

        //private void LoadedEv(object sender, RoutedEventArgs e)
        //{
        //    //EntryBoxEx.Focus();
        //}



        public static readonly DependencyProperty LookupNumProperty =
          DependencyProperty.Register("LookupNum", typeof(int), typeof(ContrSelector), new
             PropertyMetadata(1, new PropertyChangedCallback(OnLookupCodeChanged)));


        public static readonly DependencyProperty LookupNameProperty =
          DependencyProperty.Register("LookupName", typeof(string), typeof(ContrSelector), new
             PropertyMetadata("Kлиент", new PropertyChangedCallback(OnLookupNameChanged)));

        private static void OnLookupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContrSelector uc = d as ContrSelector;
            uc.OnSetLookupNameChanged(e);
        }

        private static void OnLookupCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContrSelector uc = d as ContrSelector;
            uc.OnLookupCodeChanged(e);
        }
        private void OnSetLookupNameChanged(DependencyPropertyChangedEventArgs e)
        {
            vm.NameLookup = e.NewValue.ToString();
        }

        private void OnLookupCodeChanged(DependencyPropertyChangedEventArgs e)
        {
            vm.LookupCode = (int)e.NewValue;
        }
        public int LookupNum
        {
            get { return (int)GetValue(LookupNumProperty); }
            set { SetValue(LookupNumProperty, value); }
        }

        public string LookupName
        {
            get { return (string)GetValue(LookupNameProperty); }
            set { SetValue(LookupNameProperty, value); }
        }
        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       
        public ObservableCollection<SaldoItem> ItemsDebit { get { return vm.ItemsDebit; } }

        

       
    }
}
