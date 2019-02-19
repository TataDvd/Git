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
using WindowsInput;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.EntityFramework;


namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    /// <summary>
    /// Interaction logic for DdsSellsView.xaml
    /// </summary>
    public partial class DdsSellsView : Window
    {
        private DdsViewModel vm;
        private bool saved;
        public decimal SumaDds { get; set;}
        public DdsSellsView(DdsDnevnikModel model,DdsViewModel.RefreshElement vmRefreshExecuted,DdsViewModel.RefreshElement vmSavedExecuted,DdsViewModel.CancelSaveElement vmCanelExecuted)
        {
            vm = new DdsViewModel(model);
            vm.RefreshExecuted+=vmRefreshExecuted;
            vm.DdsSaved += vmSavedExecuted;
            vm.CancelSave += vmCanelExecuted;
            InitializeComponent();
            this.DataContext = vm;
        }



        public DdsSellsView(DdsDnevnikModel model, DdsDnevnicItem item, DdsViewModel.RefreshElement vmRefreshExecuted, DdsViewModel.RefreshElement vmSavedExecuted,DdsViewModel.CancelSaveElement vmCanelExecuted)
        {
            vm = new DdsViewModel(model,item);
            vm.RefreshExecuted += vmRefreshExecuted;
            vm.DdsSaved += vmSavedExecuted; 
            vm.CancelSave += vmCanelExecuted;
            InitializeComponent();
            this.DataContext = vm;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveCommand.Execute(sender);
            SumaDds = vm.AllFields.Sum(m=>m.Dds);
            saved = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //vm.RaiseCancel();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.DeleteCommand.Execute(sender);
            Close();
        }

        private void dgEmp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void dgEmp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);
            //    var dataGrid = sender as DataGrid;
            //    if (dataGrid != null) dataGrid.BeginEdit();
            }
        }

        private void dgEmp_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        

        //private void Button_Click_3(object sender, RoutedEventArgs e)
        //{
        //    vm.GenerateReport();
        //}
        private void DdsSellsView_OnKeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Enter)
            {
                var uie = e.OriginalSource as UIElement;
                if (e.OriginalSource is TextBoxEx)
                {
                    if ((e.OriginalSource as TextBoxEx).Name=="doki")
                    {
                        e.Handled = true;
                        toki.Focus();
                        return;
                    }
                }
                
                e.Handled = true;
                if (uie != null)
                    uie.MoveFocus(
                        new TraversalRequest(
                            FocusNavigationDirection.Next));
            }
            if (e.Key == Key.Tab)
            {
                if (e.OriginalSource is System.Windows.Controls.Primitives.DatePickerTextBox)
                {
                    e.Handled = true;
                    searchi.Focus();
                    return;
                }
            }
            if (e.Key == Key.F2)
            {
                if (vm.CanSaveDds)
                {
                    Button_Click(null, e);
                    e.Handled = true;
                }
            }
            if (e.Key == Key.F4)
            {
                Button_Click_2(null, e);
                e.Handled = true;
            }
        }

        private void LookupFastSearcherUniverse_OnChangeElement(object sender, FastLookupEventArgs e)
        {
            vm.Refresh(e);
        }

        private void _this_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vm.IsLinked) return;
            if (!saved)
            {
                if (MessageBoxWrapper.Show("Не е записан ДДС в дневник! Сигурни ли сте, че желаете да продължитe?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    vm.OnCancelDdsExecutedOut(new DdsCancelEventArgs(vm.KindActivity));
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

       
    }
}
