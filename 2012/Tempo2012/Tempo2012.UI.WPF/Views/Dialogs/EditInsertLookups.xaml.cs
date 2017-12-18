using System;
using System.Collections.Generic;
using System.Globalization;
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
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for EditInsertLookups.xaml
    /// </summary>
    public partial class EditInsertLookups : Window
    {
        
        public EditInsertLookups()
        {
            LookupsEdidViewModels vm = new LookupsEdidViewModels();
            InitializeComponent();
            this.DataContext=vm;
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.First);
            MoveFocus(request);
           
        }
        public EditInsertLookups(LookupsEdidViewModels vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.First);
            MoveFocus(request);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var field in (DataContext as LookupsEdidViewModels).Fields)
            {
                if (string.IsNullOrWhiteSpace(field.Value) && field.IsRequared)
                {
                    if (field.ReadOnly)
                    {
                        MessageBoxWrapper.Show(String.Format("Не е попълненo задължително поле {0} на номенклатурата",
                                                      field.Name));
                        return;
                    }
                }
                if(!validatefield(field))
                {
                    MessageBoxWrapper.Show(String.Format("Стойноста на поле {0} не е валидна", field.Name));
                    return;
                }
            }
            DialogResult = true;
        }

        private bool validatefield(FieldValuePair field)
        {
            if (field.Type == null) return true;
            if (field.Type.ToLower()=="integer")
            {
                int v;
                if (!int.TryParse(field.Value,out v))
                {
                    return false;
                }
            }
            if (field.Type.ToLower().Contains("decimal"))
            {
                decimal v;
                var culture = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                field.Value=field.Value.Replace(".",culture);
                field.Value = field.Value.Replace(",", culture);
                if (!decimal.TryParse(field.Value, out v))
                {
                    return false;
                }
            }
            return true;
        }
        public List<string> GetNewFields()
        {
            var fields=(DataContext as LookupsEdidViewModels).Fields;
            List<string> list = new List<string>();
            foreach (var field in fields)
            {
                list.Add(field.Value);
            }
            return list;  
             
        }

        private void EditInsertLookups_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Input.KeyEventArgs args = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                args.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(args);
                e.Handled = true;
            }
            if (e.Key == Key.F2)
            {
                Button_Click(sender, e);
            }
        }

        
    }
}
