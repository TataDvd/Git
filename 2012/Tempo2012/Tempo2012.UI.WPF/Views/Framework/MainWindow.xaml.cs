using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.Tetka;
using Tempo2012.UI.WPF.Views;
using Tempo2012.UI.WPF.Views.AccountRegisters;
using Tempo2012.UI.WPF.Views.Declar;
using Tempo2012.UI.WPF.Views.Dialogs;
using Tempo2012.UI.WPF.Views.Dnevnici;
using Tempo2012.UI.WPF.Views.FirmsManagment;
using Tempo2012.UI.WPF.Views.Framework;
using Tempo2012.UI.WPF.Views.ReportManager;
using Tempo2012.UI.WPF.Views.TetkaView;
using Tempo2012.UI.WPF.Views.Users;
using Tempo2012.UI.WPF.Views.Valuta;
using WWPF.MDI;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MdiLayout = WWPF.MDI.MdiLayout;
using MenuItem = System.Windows.Controls.MenuItem;
using UserControl = System.Windows.Controls.UserControl;
using Tempo2012.UI.WPF.Views.Periods;
using Tempo2012.UI.WPF.Views.FirebirdUpdater;
using TemplateGenerator;
using Tempo2012.UI.WPF.ViewModels.AccountManagment;
using System.ComponentModel;
using System.IO;
using Tempo2012.UI.WPF.Views.ValutaReport;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private MainViewModel vm=new MainViewModel();
        private HelpModel _HelpModel = new HelpModel
        {
            TopicId = "1"
        };
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            //confi.DataContext = vm.ConfigParams;
            CommandBinding cb = new CommandBinding(CloseCommand, RemoveTabItem);
            Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();
            this.CommandBindings.Add(cb);
        }    

        void Menu_RefreshWindows()
        {
            WindowsMenu.Items.Clear();
            MenuItem mi;
            WindowsMenu.Items.Add(mi = new MenuItem { Header="Каскада"});
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.Cascade;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Хоризонтално"});
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileHorizontal;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Вертикално"});
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileVertical;
            WindowsMenu.Items.Add(new Separator());
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Затвори всички"});
            mi.Click += (o, e) => Container.Children.Clear(); 
            for (int i = 0; i < Container.Children.Count; i++)
            {
                MdiChild child = Container.Children[i];
                child.KeyDown+=child_KeyDown;
                mi = new MenuItem { Header = child.Title};
                mi.Click += (o, e) => child.Focus();
                WindowsMenu.Items.Add(mi);
            }
            WindowsMenu.Items.Add(new Separator());
           
           
        }

        private void child_KeyDown(object sender, KeyEventArgs e)
        {
            var mdiChild = sender as MdiChild;
            if (mdiChild != null)
            {
                var basev = mdiChild.Content as ScrollViewer;
                if (basev == null) return;
                var baseView = basev.Content as UserControl;
                if (baseView != null)
                {
                    var baseViewModel = baseView.DataContext as BaseViewModel;
                    if (baseViewModel == null) return;

                    if (e.Key == Key.Enter)
                    {
                        //if (Keyboard.PrimaryDevice != null)
                        //{
                        //    KeyEventArgs args = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                        //    args.RoutedEvent = Keyboard.KeyDownEvent;
                        //    InputManager.Current.ProcessInput(args);
                        //}
                        e.Handled = true;
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                    }
                    if (e.Key == Key.F5)
                    {
                        baseViewModel.ViewCommand.Execute(null);
                        e.Handled = true;
                    }

                    if (e.Key == Key.F2)
                    {
                        if (baseViewModel.Mode == EditMode.Add)
                        {
                            baseViewModel.SaveCommand.Execute(null);
                        }
                        else
                        {
                            baseViewModel.AddCommand.Execute(null);
                            // mainfocus.Focus();
                        }
                        e.Handled = true;

                    }
                    if (e.Key == Key.F3)
                    {

                        if (baseViewModel.Mode == EditMode.Edit)
                        {
                            baseViewModel.SaveCommand.Execute(null);
                            //mainfocus.Focus();
                        }
                        else
                        {
                            if (baseViewModel.Mode == EditMode.Add)
                            {
                                baseViewModel.SaveWithRepeatCommand.Execute(this);
                                return;
                            }
                            baseViewModel.UpdateCommand.Execute(null);
                            //mainfocus.Focus();
                        }
                        e.Handled = true;
                    }
                    if (e.Key == Key.F4)
                    {
                        baseViewModel.DeleteCommand.Execute(null);
                        e.Handled = true;
                    }
                }
            }
            //if (e.Key == Key.Left && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            //{
            //    firstimedown = 0;
            //    vm.Prev();
            //    if (myDataGridEvtCode != null && myDataGridEvtCode.SelectedIndex == 0)
            //    {
            //        firstime++;

            //        if (firstime > 1)
            //        {
            //            Mouse.OverrideCursor = Cursors.Wait;
            //            firstime = 0;
            //            vm.MovePreviusPageCommand.Execute(sender);
            //            /* set the SelectedItem property */
            //            myDataGridEvtCode.Focus();
            //            WraperConto p = (WraperConto)myDataGridEvtCode.Items[myDataGridEvtCode.Items.Count - 1];
            //            myDataGridEvtCode.CurrentCell = new DataGridCellInfo(p, myDataGridEvtCode.Columns[0]);
            //            myDataGridEvtCode.ScrollIntoView(p);
            //            myDataGridEvtCode.BeginEdit();
            //            e.Handled = true;
            //            Mouse.OverrideCursor = Cursors.Arrow;
            //        }
            //    }
            //    e.Handled = true;
            //}
            //if (e.Key == Key.Right && (Keyboard.Modifiers & (ModifierKeys.Control)) == ModifierKeys.Control)
            //{
            //    firstime = 0;
            //    vm.Next();
            //    if (myDataGridEvtCode != null && myDataGridEvtCode.SelectedIndex == myDataGridEvtCode.Items.Count - 1)
            //    {
            //        firstimedown++;
            //        if (firstimedown > 1)
            //        {
            //            Mouse.OverrideCursor = Cursors.Wait;
            //            firstimedown = 0;
            //            vm.MoveNextPageCommand.Execute(sender);
            //            /* set the SelectedItem property */
            //            myDataGridEvtCode.Focus();
            //            WraperConto p = (WraperConto)myDataGridEvtCode.Items[0];
            //            myDataGridEvtCode.CurrentCell = new DataGridCellInfo(p, myDataGridEvtCode.Columns[0]);
            //            myDataGridEvtCode.ScrollIntoView(p);
            //            myDataGridEvtCode.BeginEdit();
            //            e.Handled = true;
            //            Mouse.OverrideCursor = Cursors.Arrow;
            //        }
            //    }
            //    e.Handled = true;
            //}
            //if (e.SystemKey == Key.F1 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsPurchases(1, true, true, true);
            //    vm.CheckedDdsSales(-1, true, true, false);
            //    e.Handled = true;
            //}

            //if (e.SystemKey == Key.F2 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsSales(0, true, true, true);
            //    vm.CheckedDdsPurchases(-1, true, true, false);
            //    e.Handled = true;
            //}
            //if (e.SystemKey == Key.F3 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsPurchases(1, true, false, true);
            //    vm.CheckedDdsSales(1, true, false, true);
            //    e.Handled = true;
            //}
            //if (e.SystemKey == Key.F4 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsPurchases(0, false, false, true);
            //    vm.CheckedDdsSales(-1, false, false, false);
            //    e.Handled = true;
            //}
            //if (e.SystemKey == Key.F5 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsSales(10, false, false, true);
            //    vm.CheckedDdsPurchases(-1, false, false, false);
            //    e.Handled = true;
            //}
            //if (e.SystemKey == Key.F6 && (Keyboard.Modifiers & (ModifierKeys.Alt)) == ModifierKeys.Alt)
            //{
            //    vm.CheckedDdsPurchases(-1, false, false, false);
            //    vm.CheckedDdsSales(-1, false, false, false);
            //    e.Handled = true;
            //}
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

            }
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string langDictPath = "";

            switch ((sender as MenuItem).Name)
            {

                case "sh1":
                    langDictPath = "Shemes/BureauBlack.xaml";
                    break;
                case "sh2":
                    langDictPath = "Shemes/BureauBlue.xaml";
                    break;

                case "sh3":
                    langDictPath = "Shemes/ExpressionDark.xaml";
                    break;
                case "sh4":
                    langDictPath = "Shemes/ExpressionLight.xaml";
                    break;

                case "sh5":
                    langDictPath = "Shemes/ShinyBlue.xaml";
                    break;
                case "sh6":
                    langDictPath = "Shemes/ShinyRed.xaml";
                    break;

                case "sh7":
                    langDictPath = "Shemes/WhistlerBlue.xaml";
                    break;
                case "sh8":
                    langDictPath = "Shemes/MainShema.xaml";
                    break;
                

            }
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            currentconfig.Shema = (sender as MenuItem).Name;
            currentconfig.SaveConfiguration();
            Application.Current.Resources.MergedDictionaries.Clear();
            if (!String.IsNullOrEmpty(langDictPath))
            {
                Uri langDictUri = new Uri(langDictPath, UriKind.Relative);
                ResourceDictionary langDict = Application.LoadComponent(langDictUri) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(langDict);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"/WWPF.MDI;component/Themes/Luna.xaml", UriKind.Relative) });
            }
        }
        private void Button_ClickChange(object sender, RoutedEventArgs e)
        { 
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            switch ((sender as Button).Tag.ToString())
            {

                case "firm":
                    SelectFirm sf = new SelectFirm();
                    sf.ShowDialog();
                    if (sf.DialogResult.HasValue && sf.DialogResult.Value)
                    {
                        vm.ChangeConfigValue(0, sf.CurrentFirma.Name, sf.CurrentFirma.Bulstad);
                        currentconfig.CurrentFirma = sf.CurrentFirma.Clone();
                        currentconfig.SaveConfiguration();
                        //confi.DataContext = vm.ConfigParams;
                        //AccountsViewModel avm = new AccountsViewModel();
                        //AccountView.DataContext = avm;
                    }
                    break;
                case "data":
                    DataSelector ds=new DataSelector(currentconfig.WorkDate,"Избери работна дата");
                    ds.ShowDialog();
                    if (ds.DialogResult.HasValue && ds.DialogResult.Value)
                    {
                        currentconfig.WorkDate = ds.SelectedDate;
                        vm.ChangeConfigValue(1,ds.SelectedDate.ToShortDateString(),"Днес "+DateTime.Now.ToShortDateString());
                        currentconfig.SaveConfiguration();
                        //confi.DataContext = vm.ConfigParams;
                    }
                    break;

                case "user":
                    //select user
                    break;
            }
        }

        private void MenuItem_Firma_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Фирма", new FirmView());
        }

        private void AddItem(string header,object content)
        {
            //foreach (var item in MDIContainer.Items)
            //{
            //    if (((item as TabItem).Header as TabHeader).HeaderTabTitle.Text == header)
            //    {
            //        (item as TabItem).Focus();
            //        return;
            //    }
            //}
            //TabItem tb = new TabItem();
            //TabHeader th = new TabHeader();
            //th.HeaderTabCloseButton.Command = CloseCommand;
            //th.HeaderTabCloseButton.CommandParameter = header;
            //th.HeaderTabTitle.Text = header;
            //tb.Header = th;
            //tb.Content = content;
            //tb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            //tb.VerticalContentAlignment = VerticalAlignment.Top;
            //MDIContainer.Items.Add(tb);
            //MDIContainer.SelectedItem = tb;
            
            if (Container.Children.Count(e => e.Title == header)==0)
            {
                ScrollViewer sv = new ScrollViewer { Content = content as UIElement, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
               // double x = this.ActualHeight;
                //double y = this.ActualWidth-200;
                //if (x < 0) x = 900;
                //if (y < 0) y = 900;
                //Container.AnimateArangeChildren();
                MdiChild element = new MdiChild
                                       {
                                           Content = sv,
                                           Title = header,
                                           Width = 800,
                                           Height = 600,
                                           Position=new Point(0,0),
                                           Resizable = true,
                                           Icon =
                                               new BitmapImage(new Uri(@"../../Images/tempo2012icon.png",
                                                                       UriKind.Relative)),
                                                                       MainChild=false,
                                            
                                          Container = Container
                                       };
                Container.Children.Add(element);
                //Container.SunSet(element);


            }

        }
        private void MenuItem_Conto_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Контиране", new ContoView());
        }
        
        private void MenuItem_Nomenclature_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Номенклатури", new LookupManagerView());
        }
        private void RemoveTabItem(object sender, ExecutedRoutedEventArgs e)
        {
            //foreach (var item in MDIContainer.Items)
            //{
            //    if (((item as TabItem).Header as TabHeader).HeaderTabTitle.Text == e.Parameter.ToString())
            //    {
            //        MDIContainer.Items.Remove(item);
            //        return;
            //    }
            //}
           

        }

        private void MenuItem_NewNomenclature_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Типове номенклатури", new Lookups());
            
        }

       

        private void MenuItem_MainAccountReport_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Индивидуален сметкоплан", new ReportAccountsView());
        }

        private void MenuItem_MainAccountReportWithSaldo_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Сметкоплан с начално салдо", new ReportAccountsWithSaldoView());
        }

       
        private void MenuTree_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Индивидуален сметкоплан", new TreeManagerView());
            _HelpModel.TopicId = "13";
        }

        private void MenuItem_FirmaChoise_Click(object sender, RoutedEventArgs e)
        {
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            SelectFirm sf = new SelectFirm();
            sf.ShowDialog();
            if (sf.DialogResult.HasValue && sf.DialogResult.Value)
            {
                vm.ChangeConfigValue(0, sf.CurrentFirma.Name, sf.CurrentFirma.Bulstad);
                currentconfig.CurrentFirma = sf.CurrentFirma.Clone();
                currentconfig.SaveConfiguration();
                //confi.DataContext = vm.ConfigParams;
                //AccountsViewModel avm = new AccountsViewModel();
                //AccountView.DataContext = avm;
            }
        }

        private void MenuItem_Data_Click(object sender, RoutedEventArgs e)
        {
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            DataSelector ds = new DataSelector(currentconfig.WorkDate,"Избери работна дата");
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                currentconfig.WorkDate = ds.SelectedDate;
                vm.ChangeConfigValue(1, ds.SelectedDate.ToShortDateString(), "Днес " + DateTime.Now.ToShortDateString());
                currentconfig.SaveConfiguration();
                //confi.DataContext = vm.ConfigParams;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Управление на номенклатури", new SysLookUpView());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddItem("Управление на Аналитични сметки", new AnaliticManager());
 
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            AddItem("Управление на Типове Аналитични сметки", new AnaliticTypeManager());
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBoxWrapper.Show("Тази опция ще бъде активна след рестартиране на програмата!Потвърждавате ли?", "Предупреждение", MessageBoxWrapperButton.OKCancel) == MessageBoxWrapperResult.Yes)
            {
                ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                currentconfig.ModeUI = 0;
                currentconfig.SaveConfiguration();
            }

        }

        

       

        

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            CopyAccsFromFirmToFirm sf = new CopyAccsFromFirmToFirm();
            sf.ShowDialog();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            //ReportDialog report = new ReportDialog(new DdsSallesViewModel(new DdsDnevnikModel{KindActivity = 1,Title = "Дневник покупки"}));
            //report.ShowDialog();
            //DnSales dn = new DnSales();
            //dn.ShowDialog();
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                DocGenerator.GenerateDdsSales(reportMenuProvider.Vm.FromDate(), reportMenuProvider.Vm.ToDate(), false);
            }
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            //ReportDialog report = new ReportDialog(new DdsSallesViewModel(new DdsDnevnikModel{KindActivity = 2,Title = "Дневник продажби"}));
            //report.ShowDialog();
            //DnPokupki dn = new DnPokupki();
            //dn.ShowDialog();
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                DocGenerator.GenerateDdsPurchases(reportMenuProvider.Vm.FromDate(), reportMenuProvider.Vm.ToDate(), false);
            }
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            DeclarConfig dk = new DeclarConfig();
            dk.ShowDialog();
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                if (MessageBoxWrapper.Show("С включени нулеви редове?", "Изберете", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    ReportDialog rd = new ReportDialog(new OborotkiViewModel { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), HideAllZero = false });
                    rd.ShowDialog();
                }
                else
                {
                    ReportDialog rd = new ReportDialog(new OborotkiViewModel { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), HideAllZero = true });
                    rd.ShowDialog();

                }
            }
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                CopyAccsFromYearToYear copyAccsFrom = new CopyAccsFromYearToYear();
                copyAccsFrom.ShowDialog();
            }
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            NumberFormats nf = new NumberFormats();
            nf.ShowDialog();
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            ValutaAddorEdit valutaAddorEdit = new ValutaAddorEdit();
            valutaAddorEdit.ShowDialog();
        }

        private void MenuItem_Click_14(object sender, RoutedEventArgs e)
        {
            DeclarVies dv = new DeclarVies();
            dv.ShowDialog();
        }

        private void MenuItem_Click_15(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                ClearContoForFirm clearConto = new ClearContoForFirm();
                clearConto.ShowDialog();
            }
        }

        private void MenuItem_Click_16(object sender, RoutedEventArgs e)
        {
            DdsAll dds = new DdsAll();
            dds.ShowDialog();
        }

        
        private void MenuItem_Click_18(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                UsеrManager um = new UsеrManager();
                um.ShowDialog();
            }
        }

        private void MenuItem_Click_19(object sender, RoutedEventArgs e)
        {
            AccReg accReg = new AccReg();
            accReg.ShowDialog();
        }


        public DateTime FromDate { get; set; }

        private void MenuItem_Click_17(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_20(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_21(object sender, RoutedEventArgs e)
        {
            ClearContoForFirm clearConto = new ClearContoForFirm();
            clearConto.ShowDialog();
        }

        private void MenuItem_Click_22(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            DocGenerator.FirmaData();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void MenuItem_OnClick24(object sender, RoutedEventArgs e)
        {
            HoldingsWindow hw = new HoldingsWindow();
            hw.ShowDialog();
        }

        private void MenuItem_OnClick25(object sender, RoutedEventArgs e)
        {
            HoldingSelector hs = new HoldingSelector();
            hs.ShowDialog();
            if (hs.DialogResult.HasValue && hs.DialogResult.Value)
            {
                if (
                   MessageBoxWrapper.Show(
                       "При смяна на група програмата ще се рестартира. Сигурни ли сте че искате да продължите?",
                       "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    
                    var conf = ConfigTempoSinglenton.GetInstance();
                    if (hs.Holding != null)
                    {
                        //"User ID=sysdba;Password=masterkey;Database={0}:{1}\\{2}\\TEMPO2012.FDB;DataSource={0};Charset=UTF8"
                        conf.ConectionString = string.Format(Entrence.ConectionStringTemplate, hs.Holding.IpServer,
                            conf.BaseDbPath, "H" + hs.Holding.Nom);
                        conf.ActiveHolding = hs.Holding.Nom;
                        conf.ActiveFirma = 1;
                        conf.SaveConfiguration();
                        //Entrence.ConnectionString = conf.ConectionString;
                        //RealDataContext.DBManager = null;
                        //if (Entrence.IsShowLogin)
                        //{
                        //    Login login = new Login();
                        //    login.ShowDialog();
                        //    Close();
                        //}
                        //else
                        //{
                            Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        //}

                    }
                }
            }
        }

        private void MenuItem_OnClick26(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {

                    ReportDialog rd = new ReportDialog(new ReportDebit { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, IsCredit = false });
                    rd.ShowDialog();
                }
            }
        }

        private void MenuItem_OnClick27(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    ReportDialog rd = new ReportDialog(new ReportDebit { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, IsCredit = true });
                    rd.ShowDialog();
                }
            }
        }

        private void MenuItem_OnClick28(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    ReportDialog rd = new ReportDialog(new MainBook { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc });
                    rd.ShowDialog();
                }
            }
        }

        private void MenuItem_OnClick29(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            new ReorderViewModel().AddCommand.Execute(null);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void MenuItem_OnClick23(object sender, RoutedEventArgs e)
        {

            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                if (accountsModel == null)
                {
                    MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                    return;
                }
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    var f = new FacturaControlViewModel(accountsModel, new ContoViewModelLight(), acc.WithContragentSum);
                    f.FromDate = reportMenuProvider.Vm.FromDate();
                    f.ToDate = reportMenuProvider.Vm.ToDate();
                    ReportDialog report = new ReportDialog(f);
                    report.ShowDialog();
                }
            }
        }

        private void MenuItem_OnClick30(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                AppSetings appSetings = new AppSetings();
                appSetings.ShowDialog();
            }
        }

        private void MenuItem_Click_23(object sender, RoutedEventArgs e)
        {
            Periods per = new Periods();
            per.ShowDialog();
        }

        private void MenuItem_Click_24(object sender, RoutedEventArgs e)
        {
            FirebirdUpdater fb = new FirebirdUpdater();
            fb.ShowDialog();
        }

        private void MenuItem_Click_25(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    {
                        var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                        if (accountsModel == null)
                        {
                            MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                            return;
                        }
                        var f = new FacturaControlViewModel(accountsModel, new ContoViewModelLight(), acc.WithContragentSum,true);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                }

            }
        }

        private void MenuItem_Click_26(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                var testbuilder = new TemplateBuilder();
                testbuilder.LoadTemplate("E:\\TempoData\\Templates\\testblanka.txt");
                var t = testbuilder.ResultTemplate;
            }
        }

        private void MenuItem_Click_27(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                ReportDialog rd = new ReportDialog(new OborotkiViewModelDetail { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate() });
                rd.ShowDialog();
            }
        }
        private void MenuItem_Click_28(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                string pathdir = System.IO.Path.GetDirectoryName(filename);
                string filenameold = System.IO.Path.GetFileName(filename);
                string filetemplate = System.IO.Path.Combine(pathdir, "_" + filenameold);
                var testbuilder = new TemplateBuilder();
                testbuilder.LoadTemplate(filename);
                using (StreamWriter sw = new StreamWriter(filetemplate))
                {
                    sw.Write(testbuilder.ResultTemplate, Encoding.UTF8);
                }
                Process.Start(filetemplate);

            }
        }

        private void MenuItem_Click_29(object sender, RoutedEventArgs e)
        {
            SelectSimpleAcc acc = new SelectSimpleAcc();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {

                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    ReportDialog rd = new ReportDialog(new OborotkiViewModelDetail { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccString = acc.Acc });
                    rd.ShowDialog();
                }
            }
        }

        private void MenuItem_Click_30(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                var bd = new BusyDialog();
                bd.Show();
                var m_oWorker = new BackgroundWorker();

                // Create a background worker thread that ReportsProgress &
                // SupportsCancellation
                // Hook up the appropriate events.
                m_oWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    var f = new ReportNoUseDost();
                    f.DeleteAll();
                });
                m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (delegate (object sender2, RunWorkerCompletedEventArgs e2) { bd.Close(); });

                m_oWorker.RunWorkerAsync();
            }
        }

        private void MenuItem_Click_31(object sender, RoutedEventArgs e)
        {
            var f = new ReportNoUseDost();
            ReportDialog report = new ReportDialog(f);
            report.ShowDialog();
        }

        private void MenuItem_Click_32(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                if (MessageBoxWrapper.Show("С включени нулеви редове?", "Изберете", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    ReportDialog rd = new ReportDialog(new OborotkiViewModel { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), FullReport = 1, HideAllZero = false });
                    rd.ShowDialog();
                }
                else
                {
                    ReportDialog rd = new ReportDialog(new OborotkiViewModel { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), FullReport = 1, HideAllZero = true });
                    rd.ShowDialog();
                }
            }
        }

        private void MenuItem_Click_33(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                if (accountsModel == null)
                {
                    MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                    return;
                }
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    string contri = "";
                    string antetka = "";
                    string kindval=null;
                    foreach (SaldoItem item in acc.ItemsDebit)
                    {
                        if (item.Name == "Контрагент")
                        {
                            contri = item.Value;
                            antetka = string.Format(" за клиент  {0} - {1}", item.Value, item.Lookupval);
                        }
                        if (item.Name == "Вид валута")
                        {
                            kindval = item.Value;
                            
                        }

                    }
                    if (contri == "")
                    {
                        var f = new FacturaComplexViewModelDetail(accountsModel, null, true,kindval);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                    else
                    {
                        var f = new FacturaComplexViewModelDetail(accountsModel, null, true, antetka, contri,false,kindval);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                }
               
            }
        }

        private void MenuItem_Click_34(object sender, RoutedEventArgs e)
        {
            if (PassDialog.Show())
            {
                var numselector = new NumSelector("1");
                numselector.ShowDialog();
                if (numselector.DialogResult.HasValue && numselector.DialogResult.Value)
                {
                    var num = numselector.Num;
                    
                }
            }
        }

        private void MenuItem_Click_35(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                if (accountsModel == null)
                {
                    MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                    return;
                }
                string contri = "";
                string antetka = "";
                string kindval=null;
                foreach (SaldoItem item in acc.ItemsDebit)
                {
                    if (item.Name == "Контрагент")
                    {
                        contri = item.Value;
                        antetka = string.Format(" за клиент  {0} - {1}", item.Value, item.Lookupval);
                    }
                    if (item.Name == "Вид валута")
                    {
                        kindval = item.Value;

                    }
                }
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    if (contri == "")
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, true, true,1,kindval);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                    else
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, antetka, contri, true, true,1,kindval);
                        ReportDialog report = new ReportDialog(f);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        report.ShowDialog();
                    }
                }
            }
        }

        private void MenuItem_Click_36(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                if (accountsModel == null)
                {
                    MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                    return;
                }
                string contri = "";
                string antetka = "";
                string kindval = null;
                foreach (SaldoItem item in acc.ItemsDebit)
                {
                    if (item.Name == "Контрагент")
                    {
                        contri = item.Value;
                        antetka = string.Format(" за клиент  {0} - {1}", item.Value, item.Lookupval);
                    }
                    if (item.Name == "Вид валута")
                    {
                        kindval = item.Value;

                    }

                }
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    if (contri == "")
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, true, true, 1, kindval);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                    else
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, antetka, contri, true, true, 1, kindval);
                        ReportDialog report = new ReportDialog(f);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        report.ShowDialog();
                    }
                }
            }
        }

        private void MenuItem_Click_37(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                ReportDialog rd = new ReportDialog(new CheckSellsPurchases(reportMenuProvider.Vm.FromDate(), reportMenuProvider.Vm.ToDate(), 1));
                rd.ShowDialog();
            }
        }

        private void MenuItem_Click_38(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                ReportDialog rd = new ReportDialog(new CheckSellsPurchases(reportMenuProvider.Vm.FromDate(), reportMenuProvider.Vm.ToDate(), 2));
                rd.ShowDialog();
            }
        }

        private void MenuItem_Click_39(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                var accountsModel = (DataContext as MainViewModel).Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).FirstOrDefault(p => p.Short == acc.Acc);
                if (accountsModel == null)
                {
                    MessageBoxWrapper.Show("Не е намерена сметка с номер " + acc.Acc);
                    return;
                }
                string contri = "";
                string antetka = "";
                string kindval = null;
                foreach (SaldoItem item in acc.ItemsDebit)
                {
                    if (item.Name == "Контрагент")
                    {
                        contri = item.Value;
                        if (contri == "")
                        {
                            antetka = "за всички клиенти";
                        }
                        else
                        {
                            antetka = string.Format(" за клиент  {0} - {1}", item.Value, item.Lookupval);
                        }
                    }
                    if (item.Name == "Вид валута")
                    {
                        kindval = item.Value;
                        //antetka = string.Format("{0} за валута  {1} - {2}",antetka,item.Value, item.Lookupval);
                    }
                }
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    if (contri == "")
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, antetka, null, true, false, 1, kindval);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        ReportDialog report = new ReportDialog(f);
                        report.ShowDialog();
                    }
                    else
                    {
                        var f = new FacturaComplexViewModel(accountsModel, null, true, antetka, contri, true, false, 1, kindval);
                        ReportDialog report = new ReportDialog(f);
                        f.FromDate = reportMenuProvider.Vm.FromDate();
                        f.ToDate = reportMenuProvider.Vm.ToDate();
                        report.ShowDialog();
                    }
                }
            }
        }

        private void MenuItem_Click_40(object sender, RoutedEventArgs e)
        {
            ChoiserMatAcc acc = new ChoiserMatAcc();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {

                    var oldacc = Entrence.Mask.CreditAcc;
                    var oldaccd = Entrence.Mask.DebitAcc;
                    Entrence.Mask.CreditAcc = null;
                    Entrence.Mask.DebitAcc = null;
                    Entrence.Mask.FromDate = reportMenuProvider.Vm.FromDate();
                    Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                    ReportDialog rd = new ReportDialog(new QuantityDialog { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, VidVal = acc.Sklad, KindStock = acc.CodeMaterial, Stock = acc.Material });
                    Entrence.Mask.CreditAcc = oldacc;
                    Entrence.Mask.DebitAcc = oldaccd;
                    rd.ShowDialog();

                }

                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
    }
}
