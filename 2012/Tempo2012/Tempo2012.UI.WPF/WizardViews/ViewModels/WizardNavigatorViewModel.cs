using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.UI.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using Tempo2012.UI.WPF.WizardViews.Views;

namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    class WizardNavigatorViewModel
    {
        #region Fields

        RelayCommand _cancelCommand;
        RelayCommand _moveNextCommand;
        RelayCommand _movePreviousCommand;
        WizardStepBase _currentPage;
        ReadOnlyCollection<WizardStepBase> _pages;

        #endregion // Fields
        #region Constructor

        public WizardNavigatorViewModel()
        {
           this.CurrentPage = this.Pages[0];
        }

        #endregion // Constructor
        #region Commands
        #region CancelCommand
        /// <summary>
        /// Returns the command which, when executed, cancels the order 
        /// and causes the Wizard to be removed from the user interface.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(() => this.CancelOrder());

                return _cancelCommand;
            }
        }

        void CancelOrder()
        {
            this.OnRequestClose();
        }

        #endregion // CancelCommand
        #region MovePreviousCommand

        /// <summary>
        /// Returns the command which, when executed, causes the CurrentPage 
        /// property to reference the previous page in the workflow.
        /// </summary>
        public ICommand MovePreviousCommand
        {
            get
            {
                if(_movePreviousCommand == null)
                    _movePreviousCommand = new RelayCommand(
                        () => this.MoveToPreviousPage(), 
                        () => this.CanMoveToPreviousPage);

                return _movePreviousCommand;
            }
        }

        bool CanMoveToPreviousPage
        {
            get { return 0 < this.CurrentPageIndex; }
        }

        void MoveToPreviousPage()
        {
            if (this.CanMoveToPreviousPage)
                this.CurrentPage = this.Pages[this.CurrentPageIndex - 1];
        }

        #endregion // MovePreviousCommand
        #region MoveNextCommand

        /// <summary>
        /// Returns the command which, when executed, causes the CurrentPage 
        /// property to reference the next page in the workflow.  If the user
        /// is viewing the last page in the workflow, this causes the Wizard
        /// to finish and be removed from the user interface.
        /// </summary>
        public ICommand MoveNextCommand
        {
            get
            {
                if (_moveNextCommand == null)
                    _moveNextCommand = new RelayCommand(
                        () => this.MoveToNextPage(),
                        () => this.CanMoveToNextPage);

                return _moveNextCommand;
            }
        }

        bool CanMoveToNextPage
        {
            get { return this.CurrentPage != null && this.CurrentPage.IsValid(); }
        }

        void MoveToNextPage()
        {
            if (this.CanMoveToNextPage)
            {
                if (this.CurrentPageIndex < this.Pages.Count - 1)
                    this.CurrentPage = this.Pages[this.CurrentPageIndex + 1];
                else
                    this.OnRequestClose();
            }
        }

        #endregion // MoveNextCommand
        #endregion // Commands
        #region Properties

        /// <summary>
        /// Returns the page ViewModel that the user is currently viewing.
        /// </summary>
        public WizardStepBase CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                if (value == _currentPage)
                    return;

                if (_currentPage != null)
                    _currentPage.IsCurrentPage = false;

                _currentPage = value;

                if (_currentPage != null)
                    _currentPage.IsCurrentPage = true;

                this.OnPropertyChanged("CurrentPage");
                this.OnPropertyChanged("IsOnLastPage");
            }
        }

        /// <summary>
        /// Returns true if the user is currently viewing the last page 
        /// in the workflow.  This property is used by CoffeeWizardView
        /// to switch the Next button's text to "Finish" when the user
        /// has reached the final page.
        /// </summary>
        public bool IsOnLastPage
        {
            get { return this.CurrentPageIndex == this.Pages.Count - 1; }
        }

        /// <summary>
        /// Returns a read-only collection of all page ViewModels.
        /// </summary>
        public ReadOnlyCollection<WizardStepBase> Pages
        {
            get
            {
                if (_pages == null)
                    this.CreatePages();

                return _pages;
            }
        }

        #endregion // Properties
        #region Events

        /// <summary>
        /// Raised when the wizard should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        #endregion // Events
        #region Private Helpers
        void CreatePages()
        {
            //Step1AddNomenklature view1=new Step1AddNomenklature();
            //Step2AddNomenclatures view2=new Step2AddNomenclatures();
            //SummaryWizardStep view3=new SummaryWizardStep();
            NomenclatureAddStep1ViewModel step1 =new NomenclatureAddStep1ViewModel();
            NomenclatureAddStep2ViewModel step2 =new NomenclatureAddStep2ViewModel();
            NomenclatureAddSumary step3 = new NomenclatureAddSumary();
            //view1.DataContext = step1;
            //view2.DataContext = step2;
            //view3.DataContext = step3;
            
            var pages = new List<WizardStepBase> {step1, step2, step3};
            _pages = new ReadOnlyCollection<WizardStepBase>(pages);
        }

        int CurrentPageIndex
        {
            get
            {

                if (this.CurrentPage == null)
                {
                    return -1;
                }

                return this.Pages.IndexOf(this.CurrentPage);
            }
        }

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion // Private Helpers

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
