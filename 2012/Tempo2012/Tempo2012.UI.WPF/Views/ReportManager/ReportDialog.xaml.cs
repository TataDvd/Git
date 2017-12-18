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
using WindowsInput;
using ReportBuilder;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.ReportManager
{
    /// <summary>
    /// Interaction logic for ReportDialog.xaml
    /// </summary>
    public partial class ReportDialog : Window
    {
        private ReportManagerViewModel ViewModels;

        public ReportDialog(IReportBuilder builder)
        {
            ViewModels = new ReportManagerViewModel(builder);
            InitializeComponent();
            DataContext = ViewModels;
        }


        private void ReportDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            b1.Focus();
        }

        private void ReportDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

            }
        }

       
    }
}
