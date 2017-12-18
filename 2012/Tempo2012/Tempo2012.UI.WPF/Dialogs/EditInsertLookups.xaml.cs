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
        }
        public EditInsertLookups(LookupsEdidViewModels vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
    }
}
