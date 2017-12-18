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
using Tempo2012.UI.WPF.ViewModels.WorkVlowViewModels;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for Step1AddNomenklature.xaml
    /// </summary>
    public partial class Step1AddNomenklature : Window
    {
        NomenclatureAddStep1ViewModel vm=new NomenclatureAddStep1ViewModel();
        public Step1AddNomenklature()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
