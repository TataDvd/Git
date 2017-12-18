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
using System.Windows.Shapes;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.TemplateSelector;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Views.Dialogs;

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for MainAcc.xaml
    /// </summary>
    public partial class MainAcc : Window
    {
        public AccountsDialogViewModel vm;
        public MainAcc(AccountsModel accounts,EditMode mode,bool isMain,string parent="",bool saveandclose=false)
        {
            InitializeComponent();
            vm = new AccountsDialogViewModel(accounts,mode,isMain,parent,saveandclose);
            vm.ShowMain = accounts.SubNum == 0 && mode != EditMode.Edit;
            vm.IsMain = isMain;
            DataContext = vm;
        }
       
        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
                if (!(sender is Button)) e.Handled = true;
            }
            
            if (e.Key == Key.F2)
            {
                Save();
            }
           
        }

        public bool NoAcc
        {
            get { return vm.NoAcc;}
        }
        private void Save()
        {
            vm.SaveOut();
            if (vm.SaveAndClose)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
               

            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var selectedItem = (sender as ComboBox).SelectedItem;
                if (selectedItem != null)
                {
                    string[] spliter = selectedItem.ToString().Split('-');
                    string text = spliter[1];
                    if (spliter.Length > 1)
                    {
                        for (int i = 2; i < spliter.Length; i++)
                        {
                            text += "-" + spliter[i];
                        }
                    };
                    vm.Num = int.Parse(spliter[0]);
                    vm.NameMain = text;
                }
                else
                {
                    return;
                }
                LookUpSpecific ls = ((sender as ComboBox).SelectedItem as LookUpSpecific);
                if (ls != null && ls.TypeAcc==1) vm.TypeAccountEnumIn=TypeAccountEnum.IsActive;
                if (ls != null && ls.TypeAcc==2) vm.TypeAccountEnumIn=TypeAccountEnum.IsActivePasiv;
                if (ls != null && ls.TypeAcc==3) vm.TypeAccountEnumIn=TypeAccountEnum.IsPasiv;
                if (ls != null && ls.TypeAcc==4) vm.TypeAccountEnumIn=TypeAccountEnum.IsTranzitiv;
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //SetUpMapAnaliticAccToLookup ds = new SetUpMapAnaliticAccToLookup(vm.CurrentSelectedAnaliticalField);
            //ds.ShowDialog();
            //if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            //{
            //    var setUpMapAnaliticAccToLookupViewModel = ds.DataContext as SetUpMapAnaliticAccToLookupViewModel;
            //    if (setUpMapAnaliticAccToLookupViewModel != null)
            //    {
            //        vm.CurrentSelectedAnaliticalField = setUpMapAnaliticAccToLookupViewModel.WorkedItem;
            //        Mapper.Items.Refresh();
                   
            //    }
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ClearConection()) Mapper.Items.Refresh();
        }

       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SetUpMapAnaliticAccToLookup ds = new SetUpMapAnaliticAccToLookup(vm.CurrentSelectedAnaliticalField,vm.CurrentAccount);
            string OldLookup=vm.CurrentSelectedAnaliticalField.NameLookUp; 
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                var setUpMapAnaliticAccToLookupViewModel = ds.DataContext as SetUpMapAnaliticAccToLookupViewModel;
                if (setUpMapAnaliticAccToLookupViewModel != null)
                {
                    vm.CurrentSelectedAnaliticalField = setUpMapAnaliticAccToLookupViewModel.WorkedItem;
                    Mapper.Items.Refresh();

                }
            }
            else
            {
                vm.CurrentSelectedAnaliticalField.NameLookUp = OldLookup;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
