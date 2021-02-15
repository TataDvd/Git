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
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Auto60.xaml
    /// </summary>
    public partial class Stotinka : Window
    {
        private StotinkaVM vm;
        

        public Stotinka()
        {
           vm = new StotinkaVM(); 
           InitializeComponent();
           DataContext = vm;
        }

        public Stotinka(AccountsModel accountsModel)
        {
            vm = new StotinkaVM(accountsModel); 
            InitializeComponent();
            DataContext = vm;
        }

    }
}
