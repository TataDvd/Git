using System.Windows.Controls;

namespace Tempo2012.UI.WPF.CustomControls
{
    public interface IModalDialogHost
    {
        /// <summary>
        /// Gets or sets the content to host
        /// </summary>
        UserControl HostedContent { get; set; }

        /// <summary>
        /// Gets the dialog title
        /// </summary>
        string Title { set; }

        /// <summary>
        /// Gets the dialog result (i.e. OK, Cancel)
        /// </summary>
        bool? DialogResult { get; }

        /// <summary>
        /// Shows the dialog, invoking the given callback when it is closed
        /// </summary>
        void Show(DialogClosed closedCallback);  
    }
}