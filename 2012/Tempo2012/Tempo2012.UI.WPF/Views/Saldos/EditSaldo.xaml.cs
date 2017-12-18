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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.UI.WPF.ViewModels.Saldos;

namespace Tempo2012.UI.WPF.Views.Saldos
{
    /// <summary>
    /// Interaction logic for EditSaldo.xaml
    /// </summary>
    public partial class EditSaldo : Window
    {
        private EditorAddSaldosViewModel vm;
        public EditSaldo(AccountsModel acc, int groupid, int typeAnaliticalKey)
        {
            vm=new EditorAddSaldosViewModel(acc,groupid,typeAnaliticalKey);
            InitializeComponent();
            this.DataContext = vm;
        }

        public EditSaldo(AccountsModel acc, System.Collections.ObjectModel.ObservableCollection<Models.SaldoItem> SaldoItems)
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
            vm.UpdateCommand.Execute(sender);
            DialogResult = true;
            this.Close();
        }

        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                Button_Click(btnSave,e);
            }
           
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentItem!=null)
            {
                if (vm.CurrentItem.Relookup>0)
                {
                    new LookupManagerView(vm.CurrentItem.Relookup);
                    IModalDialogHost dlg = new DialogTemplate();
                    dlg.Title = "Избери елемент от номенклатура";
                    dlg.HostedContent = new LookupManagerView(vm.CurrentItem.Relookup); ;
                    dlg.Show(DialogClosedHandler);

                }
            }
        }
        private void DialogClosedHandler(IModalDialogHost dialogHost)
        {
            
            if (dialogHost.DialogResult.GetValueOrDefault())
            {
                var result = (dialogHost.HostedContent as LookupManagerView);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (uie != null)
                    uie.MoveFocus(
                        new TraversalRequest(
                            FocusNavigationDirection.Next));
            }
        }
    }
}
