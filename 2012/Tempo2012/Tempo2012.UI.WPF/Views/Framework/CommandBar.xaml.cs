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
using Tempo2012.UI.WPF.CustomControls;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CommandBarr.xaml
    /// </summary>
    public partial class CommandBar : UserControl
    {
        public CommandBar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IInputElement focusedElement = Keyboard.FocusedElement;
            if (focusedElement is MaskTextBox)
            {
                var expression = (focusedElement as MaskTextBox).GetBindingExpression(MaskTextBox.TextProperty);
                if (expression != null) expression.UpdateSource();
                (focusedElement as MaskTextBox).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            
        }
    }
}
