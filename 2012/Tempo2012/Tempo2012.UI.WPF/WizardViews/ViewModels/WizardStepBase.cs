using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    /// <summary>
    /// Abstract base class for all pages shown in the wizard.
    /// </summary>
        public abstract class WizardStepBase : INotifyPropertyChanged
        {
        
       
            #region Fields

            readonly Object _dataContext;
            bool _isCurrentPage;

            #endregion // Fields

            #region Constructor

            protected WizardStepBase(Object dataContect)
            {
                _dataContext = dataContect;
            }

            protected WizardStepBase()
            {
                _dataContext = null;
            }
            #endregion // Constructor

            #region Properties

            public Object DataContext
            {
                get { return _dataContext; }
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
