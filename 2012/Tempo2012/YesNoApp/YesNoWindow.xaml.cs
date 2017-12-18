using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YesNoApp
{
    /// <summary>
    /// Interaction logic for YesNoWindow.xaml
    /// </summary>
    public partial class YesNoWindow : Window
    {
        public YesNoWindow()
        {
            InitializeComponent();
        }

        public YesNoWindow(string message,string title,bool dontshowcancel=false)
        {
            InitializeComponent();
            Tit.Text = title;
            Mes.Text = message;
            if (dontshowcancel)
            {
                Cancelcho.Visibility = Visibility.Hidden;
                Choisorcho.Content = "OK";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
