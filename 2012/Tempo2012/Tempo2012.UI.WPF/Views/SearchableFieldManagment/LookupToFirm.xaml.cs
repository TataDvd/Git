using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Tempo2012.UI.WPF.Views.SearchableFieldManagment;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CopyAccsFromFirmToFirm.xaml
    /// </summary>
    public partial class LookupToFirm : Window
    {
        private LookupToFirmViewModel vm = new LookupToFirmViewModel();
        public LookupToFirm()
        {
            InitializeComponent();
            DataContext = vm;
        }

       
    }
}
