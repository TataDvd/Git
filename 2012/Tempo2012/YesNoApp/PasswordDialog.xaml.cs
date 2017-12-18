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

namespace YesNoApp
{
    /// <summary>
    /// Interaction logic for PasswordDialog.xaml
    /// </summary>
    public partial class PasswordDialog : Window
    {
        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void Choisorcho_Click(object sender, RoutedEventArgs e)
        {
            if (Pass.Password == "102938")
            {
                DialogResult = true;
            }
            Close();
        }

        
        private void Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pass.Password == "102938")
                {
                    DialogResult = true;
                }
                Close();
            }
        }
    }
}
