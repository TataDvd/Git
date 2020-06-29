using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WindowsInput;
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.UI.WPF.Events;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for ContoView.xaml
    /// </summary>
    public partial class ContoViewLight : UserControl
    {
        private ContoViewModelLight vm;
        private int firstime;
        private int firstimedown;
        private bool skiplostfokusdebit;
        private bool skipcreditlostfocus;
        public ContoViewLight()
        {
            InitializeComponent();
            vm = new ContoViewModelLight();
            vm.SetFocusExecuted+= OnVmOnSetFocusExecuted;
            vm.SinkInfo += OnSinkInfo;
            DataContext = vm;
            datePicker1.DisplayDateStart = vm.MinDate;
            datePicker1.DisplayDateEnd = vm.MaxDate;
            datePicker2.DisplayDateEnd = vm.MaxDate;
           
        }

        private void OnSinkInfo(object sender, SinkEventArgs e)
        {
            
            //try
            //{
            //    WraperConto p = null;
            //    if (e.Direction == 1)
            //    {
            //        p = (WraperConto)myDataGridEvtCode.Items[myDataGridEvtCode.Items.Count - 1];
            //    }
            //    else
            //    {
            //        p = (WraperConto)myDataGridEvtCode.Items[0];
            //    }
            //    if (p != null)
            //    {
            //        myDataGridEvtCode.Focus();
            //        myDataGridEvtCode.ScrollIntoView(p);
            //        myDataGridEvtCode.CurrentCell = new DataGridCellInfo(p, myDataGridEvtCode.Columns[0]);
            //        myDataGridEvtCode.BeginEdit();
                    
            //    }
               
            //}
            //catch (Exception ee)
            //{
            //    Logger.Instance().WriteLogError(ee.Message, "private void DataGrid_KeyDown(object sender, KeyEventArgs e)");
            //}
        }

        private void OnVmOnSetFocusExecuted(object sender, SetFocusEventArg arg)
        {
            if (arg.ElementName == "DDS")
            {
                ComboBoxDoc.Focus();
            }
            if (arg.ElementName == "Ob")
            {
                if (!Ob.IsFocused)
                {
                    Ob.Focus();
                }
            }
            
        }


        private void _this_KeyDown(object sender, KeyEventArgs e)
        {
            if (vm.isddsmode)
            {
                MessageBoxWrapper.Show("Отворен е дневник! Моля довършете");
                e.Handled = true;
                return;
            }
            
            if (e.Key == Key.Enter)
            {
                //if (Keyboard.PrimaryDevice != null)
                //{
                //    KeyEventArgs args = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                //    args.RoutedEvent = Keyboard.KeyDownEvent;
                //    InputManager.Current.ProcessInput(args);
                //}
                InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
            }
            if (e.Key == Key.F5)
            {
                vm.ViewCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F9)
            {
                vm.SumaDdsCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.F2 && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            {
                vm.AddFromV();
                e.Handled = true;
                return;
            }
            if (e.Key == Key.F2)
            {
                if (vm.Mode == EditMode.Add)
                {
                    vm.SaveCommand.Execute(null);
                }
                else
                {
                    vm.AddCommand.Execute(null); 
                   // mainfocus.Focus();
                }
                e.Handled = true;
               
            }
            if (e.Key == Key.F3)
            {
                
                if (vm.Mode == EditMode.Edit)
                {
                    vm.SaveCommand.Execute(null);
                    //mainfocus.Focus();
                }
                else
                {
                    if (vm.Mode == EditMode.Add)
                    {
                        vm.SaveF3();
                        return;
                    }
                    vm.UpdateCommand.Execute(null);
                    //mainfocus.Focus();
                }
                e.Handled = true;
            }
            if (e.Key == Key.F4)
            {
                if (vm.Mode == EditMode.Add) return;
                vm.DeleteCommand.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.Left && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            {
                MoveNextOut(sender, e);
                e.Handled = true;
            }
            if (e.Key == Key.Right && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            {
                MovePrevOut(sender, e);
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

            }
            if (vm.Mode != EditMode.Add) return;
            if (e.SystemKey == Key.F1 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsPurchases(1,true,true,true);
                vm.CheckedDdsSales(-1, true, true, false);
                e.Handled = true;
            }
           
            if (e.SystemKey == Key.F2 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsSales(0,true,true,true);
                vm.CheckedDdsPurchases(-1, true, true, false);
                e.Handled = true;
            }
            if (e.SystemKey == Key.F3 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsPurchases(1,true,false,true);
                vm.CheckedDdsSales(1, true, false, true);
                e.Handled = true;
            }
            if (e.SystemKey == Key.F4 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsPurchases(0, false, false, true);
                vm.CheckedDdsSales(-1, false, false, false);
                e.Handled = true;
            }
            if (e.SystemKey == Key.F5 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsSales(10, false, false, true);
                vm.CheckedDdsPurchases(-1, false, false, false);
                e.Handled = true;
            }
            if (e.SystemKey == Key.F6 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            {
                vm.CheckedDdsPurchases(-1, false, false, false);
                vm.CheckedDdsSales(-1,false,false,false);
                e.Handled = true;
            }
            
        }

        public void MovePrevOut(object sender, KeyEventArgs e)
        {
            //firstime = 0;
            vm.Next();
            //if (myDataGridEvtCode != null && myDataGridEvtCode.SelectedIndex == myDataGridEvtCode.Items.Count - 1)
            //{
            //    firstimedown++;
            //    if (firstimedown > 1)
            //    {
            //        Mouse.OverrideCursor = Cursors.Wait;
            //        firstimedown = 0;
            //        vm.MoveNextPageCommand.Execute(sender);
            //        /* set the SelectedItem property */
            //        myDataGridEvtCode.Focus();
            //        WraperConto p = (WraperConto) myDataGridEvtCode.Items[0];
            //        myDataGridEvtCode.CurrentCell = new DataGridCellInfo(p, myDataGridEvtCode.Columns[0]);
            //        myDataGridEvtCode.ScrollIntoView(p);
            //        myDataGridEvtCode.BeginEdit();
            //        e.Handled = true;
            //        Mouse.OverrideCursor = Cursors.Arrow;
            //    }
            //}
        }

        public void MoveNextOut(object sender, KeyEventArgs e)
        {
            //firstimedown = 0;
            vm.Prev();
            //if (myDataGridEvtCode != null && myDataGridEvtCode.SelectedIndex == 0)
            //{
            //    firstime++;

            //    if (firstime > 1)
            //    {
            //        Mouse.OverrideCursor = Cursors.Wait;
            //        firstime = 0;
            //        vm.MovePreviusPageCommand.Execute(sender);
            //        /* set the SelectedItem property */
            //        myDataGridEvtCode.Focus();
            //        WraperConto p = (WraperConto) myDataGridEvtCode.Items[myDataGridEvtCode.Items.Count - 1];
            //        myDataGridEvtCode.CurrentCell = new DataGridCellInfo(p, myDataGridEvtCode.Columns[0]);
            //        myDataGridEvtCode.ScrollIntoView(p);
            //        myDataGridEvtCode.BeginEdit();
            //        e.Handled = true;
            //        Mouse.OverrideCursor = Cursors.Arrow;
            //    }
            //}
            
        }

        private void kreditsmetka_KeyDown(object sender, KeyEventArgs e)
        {
            vm.WorkValuta = null;
            if (e.Key == Key.Enter)
            {
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsCredit(text);
                skipcreditlostfocus=true;
            }
            if (e.Key == Key.F6)
            {
                TreeManagerViewDialog sf = new TreeManagerViewDialog();
                sf.ShowDialog();
                if (sf.DialogResult.HasValue && sf.DialogResult.Value)
                {
                    if (sf.CurrentAcc != null) vm.CAccountsModel = sf.CurrentAcc;
                }
                e.Handled = true;
            }
        }

        private void debitsmetka_KeyDown(object sender, KeyEventArgs e)
        {
            vm.WorkValuta = null;
            if (e.Key == Key.Enter)
            {
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsDebit(text);
                
                skiplostfokusdebit = true;
            }
            if (e.Key == Key.F6)
            {
                TreeManagerViewDialog sf = new TreeManagerViewDialog();
                sf.ShowDialog();
                if (sf.DialogResult.HasValue && sf.DialogResult.Value)
                {
                    if (sf.CurrentAcc != null) vm.DAccountsModel = sf.CurrentAcc;
                    
                }
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TreeManagerViewDialog sf=new TreeManagerViewDialog();
            sf.ShowDialog();
            if (sf.DialogResult.HasValue && sf.DialogResult.Value)
            {
                if (sf.CurrentAcc != null) vm.DAccountsModel = sf.CurrentAcc;
                vm.RefreshAcc();
            }   
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            TreeManagerViewDialog sf = new TreeManagerViewDialog();
            sf.ShowDialog();
            if (sf.DialogResult.HasValue && sf.DialogResult.Value)
            {
                if (sf.CurrentAcc != null) vm.CAccountsModel = sf.CurrentAcc;
                vm.RefreshAcc();
            }
        }
        
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             vm.CodeDnev = 2;
             vm.CalculateDds(0m,!vm.IsDdsSales,false,false,true);
             //pageControl.Navigate(PageChanges.Current);
             //vm.ViewCommand.Execute(null);
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                firstimedown = 0;
                var dataGrid = sender as DataGrid;
                if (dataGrid != null && dataGrid.SelectedIndex == 0)
                {
                    firstime++;
                    if (firstime > 1)
                    {
                        //Mouse.OverrideCursor = Cursors.Wait;
                        firstime = 0;
                        vm.MovePreviusPageCommand.Execute(sender);
                       /* set the SelectedItem property */
                        //dataGrid.Focus();
                        //try
                        //{
                        //    WraperConto p = (WraperConto) dataGrid.Items[dataGrid.Items.Count - 1];
                        //    dataGrid.CurrentCell = new DataGridCellInfo(p, dataGrid.Columns[0]);
                        //    dataGrid.ScrollIntoView(p);
                        //    dataGrid.BeginEdit();

                        //}
                        //catch (Exception ee)
                        //{
                        //    Logger.Instance().WriteLogError(ee.Message, "private void DataGrid_KeyDown(object sender, KeyEventArgs e)");
                        //}
                        e.Handled = true;
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                }
            }
            if (e.Key == Key.Down)
            {
                firstime = 0;
                var dataGrid = sender as DataGrid;
                if (dataGrid != null && dataGrid.SelectedIndex == dataGrid.Items.Count-1)
                {
                    firstimedown++;
                    if (firstimedown > 1)
                    {
                        //Mouse.OverrideCursor = Cursors.Wait;
                        firstimedown = 0;
                        vm.MoveNextPageCommand.Execute(sender);
                        /* set the SelectedItem property */
                        //dataGrid.Focus();
                        //try
                        //{
                        //    WraperConto p = (WraperConto)dataGrid.Items[0];
                        //    dataGrid.CurrentCell = new DataGridCellInfo(p, dataGrid.Columns[0]);
                        //    dataGrid.ScrollIntoView(p);
                        //    dataGrid.BeginEdit();
                        //}
                        //catch (Exception ee)
                        //{
                        //    Logger.Instance().WriteLogError(ee.Message, "private void DataGrid_KeyDown(object sender, KeyEventArgs e)");
                        //}
                       
                        //e.Handled = true;
                        //Mouse.OverrideCursor = Cursors.Arrow;
                    }
                }
            }
            //if (e.Key == Key.Left && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            //{
            //    e.Handled = true;
            //    var dataGrid = sender as DataGrid;
            //    if (dataGrid != null && dataGrid.SelectedIndex > 0)
            //    {
            //        dataGrid.SelectedIndex--;
            //        dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(dataGrid.SelectedIndex));
            //    }
            //    else
            //    {
            //        firstimedown = 0;
            //        if (dataGrid != null && dataGrid.SelectedIndex == 0)
            //        {
            //            firstime++;
            //            if (firstime > 1)
            //            {
            //                Mouse.OverrideCursor = Cursors.Wait;
            //                firstime = 0;
            //                vm.MovePreviusPageCommand.Execute(sender);
            //                /* set the SelectedItem property */
            //                dataGrid.Focus();
            //                try
            //                {
            //                     WraperConto p = (WraperConto)dataGrid.Items[dataGrid.Items.Count - 1];
            //                     dataGrid.CurrentCell = new DataGridCellInfo(p, dataGrid.Columns[0]);
            //                     dataGrid.ScrollIntoView(p);
            //                     dataGrid.BeginEdit();
            //                }
            //                catch (Exception ee)
            //                {

            //                    Logger.Instance().WriteLogError(ee.Message, "private void DataGrid_KeyDown(object sender, KeyEventArgs e)");
            //                }
                           
            //                e.Handled = true;
            //                Mouse.OverrideCursor = Cursors.Arrow;
            //            }
            //        }
            //    }
            //}
            //if (e.Key == Key.Right && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            //{
            //    e.Handled = true;
            //    var dataGrid = sender as DataGrid;
            //    if (dataGrid != null && dataGrid.Items.Count - 1 > dataGrid.SelectedIndex)
            //    {
            //        dataGrid.SelectedIndex++;
            //        dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(dataGrid.SelectedIndex));
            //    }
            //    else
            //    {
            //        firstime = 0;
            //        if (dataGrid != null && dataGrid.SelectedIndex == dataGrid.Items.Count - 1)
            //        {
            //            firstimedown++;
            //            if (firstimedown > 1)
            //            {
            //                Mouse.OverrideCursor = Cursors.Wait;
            //                firstimedown = 0;
            //                vm.MoveNextPageCommand.Execute(sender);
            //                /* set the SelectedItem property */
            //                dataGrid.Focus();
            //                try
            //                {
            //                    WraperConto p = (WraperConto)dataGrid.Items[0];
            //                    dataGrid.CurrentCell = new DataGridCellInfo(p, dataGrid.Columns[0]);
            //                    dataGrid.ScrollIntoView(p);
            //                    dataGrid.SelectedIndex = 0;
            //                    dataGrid.BeginEdit();
            //                }
            //                catch (Exception ee)
            //                {
            //                    Logger.Instance().WriteLogError(ee.Message, "private void DataGrid_KeyDown(object sender, KeyEventArgs e)");
            //                }
                           
            //                e.Handled = true;
            //                Mouse.OverrideCursor = Cursors.Arrow;
            //            }
            //        }
            //    }
            //}
        }
        public static T FindVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? FindVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }
        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            //dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (row != null)
            {
                DataGridCell cell = GetCell(dataGrid, row, 1);
                if (cell != null)
                    cell.Focus();
            }
        }

        public int Last { get; set; }

        private void ValutaValutaChanged(object sender, Events.ChangeValutaEventArgs e)
        {
           vm.UpdateOborot(e.Valuta);
        }

       

        private void ButtonClick2(object sender, RoutedEventArgs e)
        {
            vm.CodeDnev = 1;
            vm.CalculateDds(0m, !vm.IsDdsPurchases,false,false,true);

            //pageControl.Navigate(PageChanges.Current);
            //vm.ViewCommand.Execute(null);
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {

                if (vm.SearcByNum())
                {

                    //myDataGridEvtCode.CurrentCell = new DataGridCellInfo(vm.CurrentWraperConto, myDataGridEvtCode.Columns[0]);
                    //myDataGridEvtCode.ScrollIntoView(vm.CurrentWraperConto);
                    //myDataGridEvtCode.BeginEdit();
                    //myDataGridEvtCode.Focus();

                }
                else
                {
                    MessageBoxWrapper.Show("Няма намерен резултат");
                }
                 e.Handled = true;
            }
        }

        private void TextBoxKeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (vm.SearcByNum1())
                {
                    //myDataGridEvtCode.CurrentCell = new DataGridCellInfo(vm.CurrentWraperConto, myDataGridEvtCode.Columns[0]);
                    //myDataGridEvtCode.ScrollIntoView(vm.CurrentWraperConto);
                    //myDataGridEvtCode.BeginEdit();
                    //myDataGridEvtCode.Focus();
                    //e.Handled = true;
                }
                else
                {
                    MessageBoxWrapper.Show("Няма намерен резултат");
                }
                 e.Handled = true;
            }
        }
        private void ContoView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

       

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            vm.DoFacturaInfoDebit();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            vm.DoFacturaInfoCredit();
            
        }

        
        //private void ComboBoxDoc_OnPreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.F4)
        //    {
        //        e.Handled = true;
        //        vm.DeleteCommand.Execute(this);
        //    }
        //}
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            vm.DoDetailsCredit(vm.Mode);
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            vm.DoDetailsDebit(vm.Mode);
        }

        private void Col_OnColChanged(object sender, ChangeValutaEventArgs e)
        {
            vm.UpdateCol();
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            {
                
                    MoveNextOut(sender, e);
                    e.Handled = true;
                
            }
            if (e.Key == Key.Right && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            {
                MovePrevOut(sender, e);
                e.Handled = true;
           }
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.OborotChange = true;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            vm.OborotChange = true;
        }

        private void LookupFastSearcherUniverse_OnChangeElement(object sender, FastLookupEventArgs e)
        {
            vm.OborotChange = true;
        }

        private void Prev_OnKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.SystemKey == Key.F10)
            {
                if (sender is TextBoxEx)
                {
                    var sendy = sender as TextBoxEx;
                    {
                        string s = sendy.Tag.ToString();
                        vm.GetPrevVal(s);
                    }
                }
            }
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            vm.OnUpdateValuta();
        }

        private void Mainfocus_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!skiplostfokusdebit)
            {
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsDebit(text);
            } 
            skiplostfokusdebit = false;
        }


        private void Kredit_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!skipcreditlostfocus)
            {
                string text = (sender as TextBox).Text;
                if (vm != null) (sender as TextBox).Text = vm.LoadAnaliticDetailsCredit(text);
            } 
            skipcreditlostfocus = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.ClearContoAll();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            vm.DoDetailsDebit(vm.Mode);
        }
    }
}
