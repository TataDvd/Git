using System.ComponentModel;
using CoffeeLibrary;

namespace GlobalizedWizard.ViewModel
{
    /// <summary>
    /// Abstract base class for all pages shown in the wizard.
    /// </summary>
    public abstract class NomenWizardPageViewModelBase : INotifyPropertyChanged
    {
        #region Fields

        readonly NomenclatureWizardModel _NomenclatureWizardModel;
        bool _isCurrentPage;

        #endregion // Fields

        #region Constructor

        protected NomenWizardPageViewModelBase(NomenclatureWizardModel nomenclatureWizardModel)
        {
            _NomenclatureWizardModel = nomenclatureWizardModel;
        }

        #endregion // Constructor

        #region Properties

        public NomenclatureWizardModel NomenclatureWizardModel
        {
            get { return _NomenclatureWizardModel; }
        } 

        public abstract string DisplayName { get; }

        public bool IsCurrentPage
        {
            get { return _isCurrentPage; }
            set 
            {
                if (value == _isCurrentPage)
                    return;

                _isCurrentPage = value;
                this.OnPropertyChanged("IsCurrentPage");
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Returns true if the user has filled in this page properly
        /// and the wizard should allow the user to progress to the 
        /// next page in the workflow.
        /// </summary>
        public abstract bool IsValid();

        #endregion // Methods

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}