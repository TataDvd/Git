using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.EntityFramework;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using System.Diagnostics;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex theMutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            // Get Reference to the current Process
            bool retV;
            theMutex = new Mutex(true, "Tempo-3e8c9b1f-7798-4c3c-be4f-f4a39c0d8eb1", out retV);

            if (!retV)
            {
                MessageBoxWrapper.Show("Темпо е стартирано!");
                theMutex = null;
                Application.Current.Shutdown();
                return;
            }


            // *******************************************************************
            // TODO - Uncomment one of the lines of code that create a CultureInfo
            // in order to see the application run with localized text in the UI.
            // *******************************************************************

            CultureInfo culture = new CultureInfo("bg-BG");
            culture.NumberFormat.NumberDecimalSeparator = ".";
            var context = new TempoDataBaseContext();

            Vf.SetFormaters(context.GetSettings("LV"), context.GetSettings("KURS"),context.GetSettings("VAL"), context.GetSettings("KOL"));

            // ITALIAN
            // *******************************************************************
            // Thanks to Corrado Cavalli for translating this application's display 
            // text to Italian, as spoken in Italy.
            // Corrado's blog: http://blogs.ugidotnet.org/corrado/Default.aspx
            //
            //culture = new CultureInfo("it-IT");


            // FRENCH
            // *******************************************************************
            // Thanks to Laurent Bugnion for translating this application's display 
            // text to French, as spoken in Switzerland.
            // Laurent's blog: http://www.galasoft.ch/
            //
            //culture = new CultureInfo("fr-CH");


            // GERMAN
            // *******************************************************************
            // Thanks to Marco Goertz , Microsoft Cider Team Senior Development Lead, 
            // for translating this application's display text to German, as spoken in Germany.
            //
            //culture = new CultureInfo("de-DE");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(
                  XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            // define application exception handler
            Application.Current.DispatcherUnhandledException += new
               DispatcherUnhandledExceptionEventHandler(
                  AppDispatcherUnhandledException);
            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent,
            new RoutedEventHandler(WindowLoaded));
            //EventManager.RegisterClassHandler(typeof(UserControl), UserControl.LoadedEvent,
            //new RoutedEventHandler(UserControlLoaded));
            // defer other startup processing to base class
            
            EventManager.RegisterClassHandler(typeof(DatePicker),
            DatePicker.PreviewKeyDownEvent,
            new KeyEventHandler(DatePicker_PreviewKeyDown));
            base.OnStartup(e);
        }
        void AppDispatcherUnhandledException(object sender,DispatcherUnhandledExceptionEventArgs e)
        {
            //do whatever you need to do with the exception
            //e.Exception
            Logger.Instance().WriteLogError(e.Exception.Message, "AppDispatcherUnhandledException");
            e.Handled = true;
            Application.Current.Shutdown();
        }
        public App():base()
        {
            string langDictPath = "Shemes/ShinyRed.xaml";
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            Entrence.FontSize = 13.0;
            Entrence.ConnectionString = string.Format(currentconfig.ConectionString, AppDomain.CurrentDomain.BaseDirectory);
            Entrence.CurrentFirmaPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Groups","H" + currentconfig.ActiveHolding.ToString(), currentconfig.ActiveFirma.ToString());
            Entrence.CurrentFirmaPathTemplate = Path.Combine(Entrence.CurrentFirmaPath, "Template");
            Entrence.CurrentFirmaPathReport = Path.Combine(Entrence.CurrentFirmaPath, "Reports");
            if (!Directory.Exists(Entrence.CurrentFirmaPath)) Directory.CreateDirectory(Entrence.CurrentFirmaPath);
            if (!Directory.Exists(Entrence.CurrentFirmaPathReport)) Directory.CreateDirectory(Entrence.CurrentFirmaPathReport);
            if (!Directory.Exists(Entrence.CurrentFirmaPathTemplate)) Directory.CreateDirectory(Entrence.CurrentFirmaPathTemplate);
            //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempo.ini"));
            //LoadConfig(loadConfigManager);
            //loadConfigManager = new LoadConfigManager(Path.Combine(currentconfig.BaseDbPath,"H"+currentconfig.ActiveHolding, "tempo.ini"));
            LoadConfig();
            if (Entrence.IsShowLogin)
            {
                StartupUri = new Uri(@"Views\Framework\Login.xaml", UriKind.Relative);
            }
            else
            {
                Config.CurrentUser = new User{UserName = Entrence.UserName,Id = Entrence.UserId,Name=Entrence.UserName};
                StartupUri = currentconfig.ModeUI==1 ? new Uri(@"Views\Framework\MainWindow.xaml",UriKind.Relative) : new Uri(@"Views\Framework\MainWindowTab.xaml",UriKind.Relative);
            }
            switch (currentconfig.Shema)
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

            Application.Current.Resources.MergedDictionaries.Clear();
            if (!String.IsNullOrEmpty(langDictPath))
            {
                Uri langDictUri = new Uri(langDictPath, UriKind.Relative);
                ResourceDictionary langDict = Application.LoadComponent(langDictUri) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(langDict);
            }
        }

        public static void LoadConfig()
        {
            var loadConfigManager = new LoadConfigManager();
            
            Entrence.Mask = new CSearchAcc();
           
            Entrence.DdsSmetkaD = loadConfigManager.GetPrameter("DDSSMETKAD") ?? Entrence.DdsSmetkaD??"453/1";
            Entrence.DdsSmetkaK = loadConfigManager.GetPrameter("DDSSMETKAK") ?? Entrence.DdsSmetkaK??"453/2";
            string t = loadConfigManager.GetPrameter("INFOCOUNT");
            Entrence.InfoCount = t!=null?int.Parse(t):5;
            Entrence.IsShowPopUp = loadConfigManager.GetPrameter("ISSHOWPOPUP") != null &&
                                   loadConfigManager.GetPrameter("ISSHOWPOPUP") == "ON";
            Entrence.IsShowLogin =
                !(loadConfigManager.GetPrameter("ISSHOWLOGIN") != null && loadConfigManager.GetPrameter("ISSHOWLOGIN") == "OFF");
            Entrence.UserName = loadConfigManager.GetPrameter("USERNAME") ??Entrence.UserName??"Анонимен";
            int id;
            Entrence.UserId = int.TryParse(loadConfigManager.GetPrameter("USERID"), out id) ? id : Entrence.UserId>0?Entrence.UserId:1;
            Entrence.UseIntelliSense = loadConfigManager.GetPrameter("USEINTELISENSE") != null &&
                                   loadConfigManager.GetPrameter("USEINTELISENSE") == "ON";
        }

        void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var window = e.Source as Window;
            System.Threading.Thread.Sleep(100);
            window.Dispatcher.Invoke(
            new Action(() =>
            {
                window.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            }));
        }
        void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            var window = e.Source as UserControl;
            System.Threading.Thread.Sleep(100);
            window.Dispatcher.Invoke(
            new Action(() =>
            {
                window.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            }));
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp == null) return;

            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, DateTime.Today);
                return;
            }

            if (!dp.SelectedDate.HasValue) return;

            var date = dp.SelectedDate.Value;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(1));
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(-1));
            }
            if (e.Key==Key.Enter)
            {

                e.Handled = true;
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
            }
        }
    }
}
