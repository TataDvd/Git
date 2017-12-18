using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CoffeeLibrary;
using GlobalizedWizard.Command;
using Tempo2012.EntityFramework.FakeData;

namespace GlobalizedWizard.ViewModel
{
    /// <summary>
    /// The main ViewModel class for the wizard.
    /// This class contains the various pages shown
    /// in the workflow and provides navigation
    /// between the pages.
    /// </summary>
    public class NomenWizardViewModel : INotifyPropertyChanged
    {
        #region Fields

        RelayCommand _cancelCommand;
        NomenclatureWizardModel _NomenclatureWizardModel;
        NomenWizardPageViewModelBase _currentPage;
        RelayCommand _moveNextCommand;
        RelayCommand _movePreviousCommand;
        ReadOnlyCollection<NomenWizardPageViewModelBase> _pages;

        #endregion // Fields

        #region Constructor

        public NomenWizardViewModel()
        {
            _NomenclatureWizardModel = new NomenclatureWizardModel(new List<Tempo2012.EntityFramework.Models.LookUpMetaData>(FakeDataContext.GetAllNomenclatureFields()));
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
            _NomenclatureWizardModel = null;
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
        /// Returns the cup of coffee ordered by the customer.
        /// If this returns null, the user cancelled the order.
        /// </summary>
        public NomenclatureWizardModel NomenclatureWizardModel
        {
            get { return _NomenclatureWizardModel; }
        }

        /// <summary>
        /// Returns the page ViewModel that the user is currently viewing.
        /// </summary>
        public NomenWizardPageViewModelBase CurrentPage
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

                OnPropertyChanged("CurrentPage");
                OnPropertyChanged("IsOnLastPage");
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
        public ReadOnlyCollection<NomenWizardPageViewModelBase> Pages
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
            var step1 = new NomenAddStep0(NomenclatureWizardModel);
            var step2 = new NomenAddStep1(NomenclatureWizardModel);
            var step3 = new NomenAddStep2(NomenclatureWizardModel);
            var step4 = new NomenAddStep3(NomenclatureWizardModel);


            var pages=new List<NomenWizardPageViewModelBase>{step1,step2,step3,step4};
            _pages = new ReadOnlyCollection<NomenWizardPageViewModelBase>(pages);
        }

        int CurrentPageIndex
        {
            get
            {

                if (this.CurrentPage == null)
                {
                    Debug.Fail("Why is the current page null?");
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