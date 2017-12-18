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
    public partial class SetUpMapAnaliticAccToLookup : Window
    {
       

        
        public SetUpMapAnaliticAccToLookup()
        {
            SetUpMapAnaliticAccToLookupViewModel vm = new SetUpMapAnaliticAccToLookupViewModel();
            InitializeComponent();
            this.DataContext=vm;
        }
        public SetUpMapAnaliticAccToLookup(SetUpMapAnaliticAccToLookupViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        public SetUpMapAnaliticAccToLookup(EntityFramework.Models.AnaliticalFields analiticalFields)
        {
            SetUpMapAnaliticAccToLookupViewModel vm = new SetUpMapAnaliticAccToLookupViewModel(analiticalFields);
            InitializeComponent();
            this.DataContext = vm;
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
    }
}
