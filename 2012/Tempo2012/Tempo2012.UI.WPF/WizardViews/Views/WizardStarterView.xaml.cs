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
using Tempo2012.UI.WPF.WizardViews.ViewModels;

namespace Tempo2012.UI.WPF.WizardViews.Views
{
    /// <summary>
    /// Interaction logic for WizardStarter.xaml
    /// </summary>
    public partial class WizardStarter : Window
    {
        private WizardNavigatorViewModel vm = new WizardNavigatorViewModel();
        //private NomenclatureAddStep2ViewModel vm = new NomenclatureAddStep2ViewModel();
        public WizardStarter()
        {
            InitializeComponent();
            vm = new WizardNavigatorViewModel();
            vm.RequestClose += this.OnViewModelRequestClose;
            base.DataContext = vm;     
        }
        void OnViewModelRequestClose(object sender, EventArgs e)
        {
            base.DialogResult = this.Result != null;
        }
        public Object Result
        {
            get { return "ihuu"; }
        }
    }
}
