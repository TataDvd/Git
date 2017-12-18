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
using Tempo2012.UI.WPF.Events;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.Views.Framework
{
    /// <summary>
    /// Interaction logic for Valuta.xaml
    /// </summary>
    public partial class Col: UserControl
    {
        public event EventHandler<ChangeValutaEventArgs> ColChanged;
        public Col()
        {
            InitializeComponent();
        }

        private void OnColChanged()
        {
            EventHandler<ChangeValutaEventArgs> handler = ColChanged;
            if (handler != null)
            {
                var dc = DataContext as SaldoItem;
                handler(this, new ChangeValutaEventArgs(dc.SumaLeva));
            }
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnColChanged();
            }
        }
    }
}
