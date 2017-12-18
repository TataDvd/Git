using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.EntityFramework;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // *******************************************************************
            // TODO - Uncomment one of the lines of code that create a CultureInfo
            // in order to see the application run with localized text in the UI.
            // *******************************************************************

            CultureInfo culture = new CultureInfo("bg-BG");

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

            if (culture != null)
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(
                  XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
        public App():base()
        {
            string langDictPath = "Shemes/ShinyRed.xaml";
            ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
            Entrence.ConnectionString =string.Format(currentconfig.ConectionString,AppDomain.CurrentDomain.BaseDirectory);
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
    }
}
