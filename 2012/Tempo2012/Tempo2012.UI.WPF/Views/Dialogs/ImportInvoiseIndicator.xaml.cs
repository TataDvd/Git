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

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CopyAccsFromYearToYear.xaml
    /// </summary>
    public partial class ImportInvoiseIndicator : Window
    {
        
        public ImportInvoiseIndicator(string fileName,int defdocnumber,bool isTransport=false)
        {
            ImportInvoiseIndicatorViewModel vm =new ImportInvoiseIndicatorViewModel(fileName,defdocnumber,isTransport);
            InitializeComponent();
            DataContext = vm;
        }

        
    }
}
