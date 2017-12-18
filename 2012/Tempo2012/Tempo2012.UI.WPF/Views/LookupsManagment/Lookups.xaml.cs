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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for LookUpSpecific.xaml
    /// </summary>
    public partial class Lookups : UserControl 
    {
        private LookupsViewModel vm=new LookupsViewModel(); 
        public Lookups()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        
    }
}
