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
using PasswordHashMaker;
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
        private TempoDataBaseContext context=new TempoDataBaseContext();
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User currentuser=context.GetUser(userName.Text,"");
            if (currentuser != null && PasswordHash.ValidatePassword(passWord.Password,currentuser.PassHash))
            {
                Config.CurrentUser = currentuser;
                MainViewModel mainViewModel = new MainViewModel(currentuser);
                ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                if (currentconfig.ModeUI==1)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.DataContext = mainViewModel;
                    mainWindow.Show();
                }
                else
                {
                    MainWindowTab mainWindow = new MainWindowTab();
                    mainWindow.DataContext = mainViewModel;
                    mainWindow.Show();
                }
                
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
