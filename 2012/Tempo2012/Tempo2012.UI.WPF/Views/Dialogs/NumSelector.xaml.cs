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

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for MonthSelector.xaml
    /// </summary>
    public partial class NumSelector : Window
    {
        public NumSelector(string month)
        {
            InitializeComponent();
            mo.Text = month;
            mo.Focus();
        }

        public int Num
        {
            get { 
                int result;
                if (int.TryParse(mo.Text, out result))
                {
                    return result;
                }
                return 1;
            }
        }

        private void mo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
