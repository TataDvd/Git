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
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.UI.WPF.ViewModels.Saldos;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;


namespace Tempo2012.UI.WPF.Views.Saldos
{
    public partial class EditorAddSaldo : Window
    {
        private EditorAddSaldosViewModel vm;
        public EditorAddSaldo(AccountsModel acc, int groupid, int typeAnaliticalKey)
        {
            vm=new EditorAddSaldosViewModel(acc,groupid,typeAnaliticalKey);
            InitializeComponent();
            this.DataContext = vm;
        }

        public EditorAddSaldo(AccountsModel acc, System.Collections.ObjectModel.ObservableCollection<Models.SaldoItem> SaldoItems)
        {
            vm = new EditorAddSaldosViewModel(acc, SaldoItems,false);
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IInputElement focusedElement = FocusManager.GetFocusedElement(this);
            if (focusedElement is TextBox)
            {
                var expression = (focusedElement as TextBox).GetBindingExpression(TextBox.TextProperty);
                if (expression != null) expression.UpdateSource();
            }
            if (vm.IsEdit)
            {
                vm.UpdateCommand.Execute(sender);
                DialogResult = true;
                this.Close();
            }
            else
            {
                if (vm.CheckSaldoItem())
                {
                    vm.SaveCommand.Execute(sender);
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBoxWrapper.Show("Вече е въведено салдо по тази номенклаура");
                }
            }
            
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    var sen = (sender as Button);
        //    if (sen!=null)
        //    {
        //        var cp = sen.CommandParameter as SaldoItem;
        //        if (cp != null && (cp.Relookup!=0 || cp.RCODELOOKUP!=0))
        //        {
                    
        //            List<FieldValuePair> current = new List<FieldValuePair>();
        //            LookupModel lookup = null;
        //            if (cp.Relookup > 0)
        //            {
        //                lookup = vm.Context.GetLookup(cp.Relookup);
                       
        //            }
        //            else
        //            {
        //                lookup = vm.Context.GetLookup(cp.RCODELOOKUP);
        //            }

        //            foreach (var item in lookup.Fields)
        //            {
        //                current.Add(new FieldValuePair
        //                {
        //                    Name = item.Name,
        //                    Value = "",
        //                    Length = item.Length,
        //                    ReadOnly = (item.NameEng == "Id") ? false : true,
        //                    IsRequared=item.IsRequared,
        //                    IsUnique=item.IsUnique
        //                });

        //            }
        //            LookupsEdidViewModels vmm = new LookupsEdidViewModels(current,lookup.LookUpMetaData.Tablename);
        //            EditInsertLookups ds = new EditInsertLookups(vmm);
        //            ds.ShowDialog();
        //            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
        //            {
        //                   //nov red
        //                if (vm.Context.SaveRow(ds.GetNewFields(), lookup,
        //                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id))
        //                {
        //                    cp.LookUp.Add(new SaldoItem {Value = ds.GetNewFields()[2], Key = ds.GetNewFields()[1]});
        //                    vm = new EditorAddSaldosViewModel(vm.Acc, vm.Items, true);
        //                    this.DataContext = vm;
        //                }
        //                else
        //                {
        //                    System.Windows.MessageBoxWrapper.Show("Получвава се дублиране на елемент от номенклатура!Номенклатурата не е записана!");
        //                }

        //            }
                    

        //        }
        //    }
        //}
        //private void DialogClosedHandler(IModalDialogHost dialogHost)
        //{
            
        //    if (dialogHost.DialogResult.GetValueOrDefault())
        //    {
        //        var result = (dialogHost.HostedContent as LookupManagerView);
        //    }
        //}
        private void _this_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
                // TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                //MoveFocus(request);
            }
            if (e.Key == Key.F2)
            {
                Button_Click(btnSave, e);
                e.Handled = true;
            }
            
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //var uie = e.OriginalSource as UIElement;

            //if (e.Key == Key.Enter)
            //{
            //    e.Handled = true;
            //    if (uie != null)
            //        uie.MoveFocus(
            //            new TraversalRequest(
            //                FocusNavigationDirection.Next));
            //}
        }

        private void ValutaValutaChanged(object sender, Events.ChangeValutaEventArgs e)
        {
            if (vm.GetValidationError("Items") == null)
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private void TextBoxEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vm.GetValidationError("Items") == null)
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private void LookupFastSearcherUniverse_ChangeElement(object sender, ViewModels.ContoManagment.FastLookupEventArgs e)
        {
            if (vm.GetValidationError("Items") == null)
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }

        }

       
    }
}
