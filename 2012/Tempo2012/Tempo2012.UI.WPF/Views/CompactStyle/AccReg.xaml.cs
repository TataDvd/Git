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
using Tempo2012.EntityFramework.Interface;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;
using Tempo2012.UI.WPF.ViewModels.Tetka;
using Tempo2012.UI.WPF.Views.Dialogs;
using Tempo2012.UI.WPF.Views.Framework;
using Tempo2012.UI.WPF.Views.ReportManager;
using Tempo2012.UI.WPF.Views.ValutaReport;

namespace Tempo2012.UI.WPF.Views.AccountRegisters
{
    /// <summary>
    /// Interaction logic for AccReg.xaml
    /// </summary>
    public partial class AccReg : Window
    {
        public AccReg()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
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
                    ReportDialog rd = new ReportDialog(new AnaliticRegisterViewModel { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc,ItemsDebit=acc.ItemsDebit });
                    rd.ShowDialog();
                    Entrence.Mask.CreditAcc=oldacc;
                    Entrence.Mask.DebitAcc=oldaccd;
                }
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
           
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    var sm = new SearchViewModelAcc(false);
                    Entrence.Mask.FromDate=reportMenuProvider.Vm.FromDate();
                    Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                    sm.Hrono  = (sender as MenuItem).Tag.ToString();
                    sm.AddNewCommand.Execute(null);
                    
                }
           
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            SearchFormAcc ds = new SearchFormAcc();
            ds.ShowDialog();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                var sm = new SearchViewModelWithAcc();
                sm.Tipdnev = 1;
                Entrence.Mask.FromDate = reportMenuProvider.Vm.FromDate();
                Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                ReportDialog rmDialog=new ReportDialog(sm);
                rmDialog.Title= (sender as MenuItem).Tag.ToString();
                rmDialog.ShowDialog();
            }
        }

        private void MenuItem_OnClick_DnevProd(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                var sm = new SearchViewModelWithAcc();
                sm.Tipdnev = 2;
                Entrence.Mask.FromDate = reportMenuProvider.Vm.FromDate();
                Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                ReportDialog rmDialog = new ReportDialog(sm);
                rmDialog.Title = (sender as MenuItem).Tag.ToString();
                rmDialog.ShowDialog();
            }
        }

        private void MenuItem_OnClick_bezDDS(object sender, RoutedEventArgs e)
        {

            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                var sm = new SearchViewModelAcc(false);
                sm.Hrono = (sender as MenuItem).Tag.ToString(); 
                Entrence.Mask.FromDate = reportMenuProvider.Vm.FromDate();
                Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                sm.DeleteCommand.Execute(null);

            }
        }

        private void MenuItemValRep_OnClick(object sender, RoutedEventArgs e)
        {
            ChoiserValutaandAcc acc = new ChoiserValutaandAcc();
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
                        ReportDialog rd = new ReportDialog(new ValutareportDialog { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, VidVal = acc.VidV, CodeClient = acc.CodeClient, Client = acc.Client,Title = (sender as MenuItem).Tag.ToString()});
                        Entrence.Mask.CreditAcc = oldacc;
                        Entrence.Mask.DebitAcc = oldaccd;
                        rd.ShowDialog();

                    }
                

                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void MenuItem_Click_Contr(object sender, RoutedEventArgs e)
        {
            SelectContDialog acc = new SelectContDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                SelectAccDialog acc1 = new SelectAccDialog();
                acc1.ShowDialog();
                if (acc1.DialogResult.HasValue && acc1.DialogResult.Value)
                {
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
                        ReportDialog rd = new ReportDialog(new AnaliticRegisterViewModelContr { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), ItemsDebit = acc.ItemsDebit,CurrenAcc=acc1.CurrentAcc, Title = (sender as MenuItem).Tag.ToString() });
                        rd.ShowDialog();
                        Entrence.Mask.CreditAcc = oldacc;
                        Entrence.Mask.DebitAcc = oldaccd;
                    }
                }
            }
        }

        private void MenuItem_Click_Dost(object sender, RoutedEventArgs e)
        {
            SelectDostDialog acc = new SelectDostDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                SelectAccDialog acc1 = new SelectAccDialog();
                acc1.ShowDialog();
                if (acc1.DialogResult.HasValue && acc1.DialogResult.Value)
                {
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
                        ReportDialog rd = new ReportDialog(new AnaliticRegisterViewModelDost { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), ItemsDebit = acc.ItemsDebit,CurrenAcc=acc1.CurrentAcc, Title = (sender as MenuItem).Tag.ToString() });
                        rd.ShowDialog();
                        Entrence.Mask.CreditAcc = oldacc;
                        Entrence.Mask.DebitAcc = oldaccd;
                    }
                }
            }
        }

        private void MenuItem_Click_Quantity(object sender, RoutedEventArgs e)
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
                    ReportDialog rd = new ReportDialog(new QuantityDialog { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, VidVal = acc.Sklad, KindStock = acc.CodeMaterial, Stock = acc.Material, Title = (sender as MenuItem).Tag.ToString() });
                    Entrence.Mask.CreditAcc = oldacc;
                    Entrence.Mask.DebitAcc = oldaccd;
                    rd.ShowDialog();

                }

                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            ChoiserValutaandAcc acc = new ChoiserValutaandAcc();
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
                    ReportDialog rd = new ReportDialog(new ValutaExtendedDialog { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), AccShortName = acc.Acc, VidVal = acc.VidV, CodeClient = acc.CodeClient, Client = acc.Client, Title = (sender as MenuItem).Tag.ToString() });
                    Entrence.Mask.CreditAcc = oldacc;
                    Entrence.Mask.DebitAcc = oldaccd;
                    rd.ShowDialog();

                }

                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void MenuItem_Grouping(object sender, RoutedEventArgs e)
        {
            SelectAccDialog acc = new SelectAccDialog();
            acc.ShowDialog();
            if (acc.DialogResult.HasValue && acc.DialogResult.Value)
            {
                ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
                reportMenuProvider.ShowDialog();
                if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
                {
                    ReportDialog rd = new ReportDialog(new AnaliticRegisterViewModelContrGrupaDocument { FromDate = reportMenuProvider.Vm.FromDate(), ToDate = reportMenuProvider.Vm.ToDate(), CurrenAcc = acc.CurrentAcc, ItemsDebit = acc.ItemsDebit, Title = (sender as MenuItem).Tag.ToString() });
                    rd.ShowDialog();
                   
                }
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                var sm = new SearchViewModelAcc(true);
                Entrence.Mask.FromDate = reportMenuProvider.Vm.FromDate();
                Entrence.Mask.ToDate = reportMenuProvider.Vm.ToDate();
                sm.Hrono = (sender as MenuItem).Tag.ToString();
                sm.AddNewCommand.Execute(null);

            }
        }
    }
}
