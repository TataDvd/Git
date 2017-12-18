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
using Tempo2012.UI.WPF.ViewModels.ReportMenuProvider;

namespace Tempo2012.UI.WPF.Views.ReportManager
{
    /// <summary>
    /// Interaction logic for ReportMenuProviderView.xaml
    /// </summary>
    public partial class ReportMenuProviderView : Window
    {
        private ReportMenuProviderViewModel vm = new ReportMenuProviderViewModel(ConfigTempoSinglenton.GetInstance().WorkDate.Year,ConfigTempoSinglenton.GetInstance().WorkDate.Month);
        public ReportMenuProviderView()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var name = button.Content.ToString();
                switch (name)
                {
                    case "Текущ месец": vm.AddCommand.Execute(null);
                        break;
                    case "Произволен месец": vm.AddNewCommand.Execute(null);
                        break;
                    case "Произволен период": vm.UpdateCommand.Execute(null);
                        break;
                    case "Произволна дата": vm.DeleteCommand.Execute(null);
                        break;
                    case "Цяла година":vm.ViewCommand.Execute(null); break;     
                }
            }
            DialogResult = true;
            Close();
        }

        public ReportMenuProviderViewModel Vm { get { return vm; } }
    }
}
