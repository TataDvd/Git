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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tempo2012.UI.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for DialogTemplate.xaml
    /// </summary>
    public partial class DialogTemplate : Window,IModalDialogHost
    {
        public DialogTemplate()
        {
            InitializeComponent();
        }

        
        public UserControl HostedContent
        {
            get
            {
                return (UserControl)contentHost.Content;
            }
            set
            {
                contentHost.Content = value;
            }
        }

        public void Show(DialogClosed closedCallback)
        {
            ShowDialog();
            closedCallback(this);
        }

        public new string Title
        {
            set { base.Title = value; }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

       
    }
}
