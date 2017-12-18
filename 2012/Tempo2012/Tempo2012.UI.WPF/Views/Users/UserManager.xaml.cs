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

namespace Tempo2012.UI.WPF.Views.Users
{
    /// <summary>
    /// Interaction logic for UsaerManager.xaml
    /// </summary>
    public partial class UsеrManager : Window
    {
        public UsеrManager()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
        }
    }
}
