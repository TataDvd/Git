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

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for DataSelector.xaml
    /// </summary>
    public partial class DataSelector : Window
    {
        public DataSelector()
        {
            InitializeComponent();
        }
        public DataSelector(DateTime currentdate)
            :this()
        {
            dater.SelectedDate = currentdate;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public DateTime SelectedDate
        {
            get { return dater.SelectedDate.GetValueOrDefault(); }
        }
    }
}
