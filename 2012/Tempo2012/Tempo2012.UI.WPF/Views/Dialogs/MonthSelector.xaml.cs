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
    public partial class MonthSelector : Window
    {
        public MonthSelector(string month, string title)
        {
            InitializeComponent();
            Title = title;
            mo.Text = month;
            mo.Focus();
        }

        public int Month
        {
            get { 
                int result;
                if (int.TryParse(mo.Text, out result))
                {
                    if (result > 0 && result < 13)
                    {
                        return result;
                    }
                    else
                    {
                        return 12;
                    }
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
