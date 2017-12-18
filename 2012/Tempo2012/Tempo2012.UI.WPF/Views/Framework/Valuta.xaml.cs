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
    public partial class Valuta : UserControl
    {
        public event EventHandler<ChangeValutaEventArgs> ValutaChanged;
        public Valuta()
        {
            InitializeComponent();
        }

        private void OnValutaChanged()
        {
            EventHandler<ChangeValutaEventArgs> handler = ValutaChanged;
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
                var dc = DataContext as SaldoItem;
                if (dc != null)
                {
                    dc.SumaLeva = dc.ValueVal * dc.ValueKurs;
                    dc.KursDif = Math.Round((dc.ValueKurs - dc.MainKurs) * dc.ValueVal,2);
                }
                OnValutaChanged();
            }
        }


        private void Oporen_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var dc = DataContext as SaldoItem;
                if (dc != null)
                {
                    dc.KursDif = Math.Round((dc.ValueKurs - dc.MainKurs) * dc.ValueVal,2);

                }
              }
        }

        private void MaskTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SaldoItem;
            var te = (sender as MaskTextBox).Text;
            if (dc != null)
            {
                dc.SumaLeva =dc.ValueVal * decimal.Parse(te) ;
                dc.KursDif = Math.Round((decimal.Parse(te)-dc.MainKurs) * dc.ValueVal, 2);
                OnValutaChanged();
            }

        }

        private void MaskTextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SaldoItem;
            var te = (sender as MaskTextBox).Text;
            if (dc != null)
            {
                dc.KursDif = Math.Round((dc.ValueKurs - decimal.Parse(te)) * dc.ValueVal, 2);
            }
        }

        private void MaskTextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SaldoItem;
            var te = (sender as MaskTextBox).Text;
            if (dc != null)
            {
                dc.SumaLeva = decimal.Parse(te) * dc.ValueKurs;
                dc.KursDif = Math.Round((dc.ValueKurs - dc.MainKurs) * decimal.Parse(te), 2);
                OnValutaChanged();
            }
            
        }
    }
}
