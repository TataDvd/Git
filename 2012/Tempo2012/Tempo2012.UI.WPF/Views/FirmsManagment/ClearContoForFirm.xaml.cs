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

namespace Tempo2012.UI.WPF.Views.FirmsManagment
{
    /// <summary>
    /// Interaction logic for ClearContoForFirm.xaml
    /// </summary>
    public partial class ClearContoForFirm : Window
    {
        private ClearContoForFirmViewModel vm = new ClearContoForFirmViewModel();
        public ClearContoForFirm()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
