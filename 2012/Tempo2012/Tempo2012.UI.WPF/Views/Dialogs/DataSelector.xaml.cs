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
        public DataSelector(DateTime currentdate,string title,bool chekforpermition=true)
            :this()
        {
            dater.SelectedDate = currentdate;
            Title = title;
            CheckForPermition = chekforpermition;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForPermition)
            {
                ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                if (currentconfig.Periods.FirstOrDefault(e1 => e1.Fr <= dater.SelectedDate && e1.To >= dater.SelectedDate && e1.Firma == currentconfig.ActiveFirma && e1.Holding == currentconfig.ActiveHolding) != null)
                {
                    MessageBoxWrapper.Show("Забранен период не може да се сетне датата " + dater.SelectedDate);
                    return;
                }
            }
            DialogResult = true;
        }

        public DateTime SelectedDate
        {
            get { return dater.SelectedDate.GetValueOrDefault(); }
        }

        public bool CheckForPermition { get; private set; }
    }
}
