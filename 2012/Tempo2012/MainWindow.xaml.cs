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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Views;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private MainViewModel vm=new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            confi.DataContext = vm.ConfigParams;
            CommandBinding cb = new CommandBinding(CloseCommand, RemoveTabItem);
            this.CommandBindings.Add(cb);
            this.AddItem("Информация", new Banner());

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
                    DataSelector ds=new DataSelector(currentconfig.WorkDate);
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
            foreach (var item in MDIContainer.Items)
            {
                if (((item as TabItem).Header as TabHeader).HeaderTabTitle.Text == header)
                {
                    (item as TabItem).Focus();
                    return;
                }
            }
            TabItem tb = new TabItem();
            TabHeader th = new TabHeader();
            th.HeaderTabCloseButton.Command = CloseCommand;
            th.HeaderTabCloseButton.CommandParameter = header;
            th.HeaderTabTitle.Text = header;
            tb.Header = th;
            tb.Content = content;
            tb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            tb.VerticalContentAlignment = VerticalAlignment.Top;
            MDIContainer.Items.Add(tb);
            MDIContainer.SelectedItem = tb;
        }
        private void MenuItem_Conto_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Контиране", new ContoView());
        }
        private void MenuItem_ContoPlan_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Сметкоплан", new AccountsView());
        }
        private void MenuItem_Nomenclature_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Номенклатури", new LookupManagerView());
        }
        private void RemoveTabItem(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in MDIContainer.Items)
            {
                if (((item as TabItem).Header as TabHeader).HeaderTabTitle.Text == e.Parameter.ToString())
                {
                    MDIContainer.Items.Remove(item);
                    return;
                }
            }
           

        }

        private void MenuItem_NewNomenclature_Click(object sender, RoutedEventArgs e)
        {
            AddItem("Нова Номенклатура", new Lookups());
        }
    }
}
