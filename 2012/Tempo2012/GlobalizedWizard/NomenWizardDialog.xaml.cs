using System;
using System.Collections.Generic;
using System.Windows;
using CoffeeLibrary;
using GlobalizedWizard.ViewModel;
using Tempo2012.EntityFramework.FakeData;

namespace GlobalizedWizard
{
    public partial class NomenWizardDialog : Window
    {
        readonly NomenWizardViewModel _NoNomenWizardViewModel;

        public NomenWizardDialog()
        {
            InitializeComponent();
            _NoNomenWizardViewModel = new NomenWizardViewModel();
            _NoNomenWizardViewModel.RequestClose += this.OnViewModelRequestClose;
            base.DataContext=_NoNomenWizardViewModel;
                        
        }
              
        /// <summary>
        /// Returns the cup of coffee ordered by the user, 
        /// or null if the user cancelled the order.
        /// </summary>
        public NomenclatureWizardModel Result
        {
            get { return _NoNomenWizardViewModel.NomenclatureWizardModel; }
        }

        void OnViewModelRequestClose(object sender, EventArgs e)
        {
            base.DialogResult = this.Result != null;
        }

       }
}