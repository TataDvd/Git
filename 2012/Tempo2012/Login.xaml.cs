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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Tempo2012DataBaseContext context=new Tempo2012DataBaseContext();
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Users currentuser=context.GetUser(userName.Text, passWord.Password);
            if (currentuser != null)
            {

                MainViewModel mainViewModel = new MainViewModel(currentuser);
                MainWindow mainWindow = new MainWindow();
                mainWindow.DataContext = mainViewModel;
                mainWindow.Show();
            }
            this.Close();
        }

        private void passWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
