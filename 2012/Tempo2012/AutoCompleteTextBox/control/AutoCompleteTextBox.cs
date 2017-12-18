using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using dragonz.actb.core;
using dragonz.actb.provider;
using System.Windows;
using System.Collections;

namespace dragonz.actb.control
{
    public class AutoCompleteTextBox : TextBox
    {
        private AutoCompleteManager _acm;

        public AutoCompleteManager AutoCompleteManager
        {
            get { return _acm; }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoCompleteTextBox), new UIPropertyMetadata(null,
                ItemsSource2DPropertyChanged));

        private static void ItemsSource2DPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox ac = source as AutoCompleteTextBox;
            ac.OnItemsSource2DChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
        }
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private void OnItemsSource2DChanged(IEnumerable iEnumerable, IEnumerable iEnumerable_2)
        {
            if (iEnumerable_2 == null) return;
            List<string> provider = new List<string>();
            bool first=true;
            bool tip = false;
            foreach (var item in iEnumerable_2)
            {
                if (first)
                { 
                    first = false;
                    var split = item.ToString().Split('|');
                    if (split.Length > 1) tip = true;
                }
                provider.Add(item.ToString());
            }
            if (tip)
            {
                _acm.DataProvider=new ContragentProvider(provider);
            }
            else
            {
                _acm.DataProvider = new SimpleStaticDataProvider(provider);
            }
            
        }
        public AutoCompleteTextBox()
        {
            _acm = new AutoCompleteManager();
            _acm.DataProvider = new FileSysDataProvider();
            this.Loaded += AutoCompleteTextBox_Loaded;
        }

        void AutoCompleteTextBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _acm.AttachTextBox(this);
        }
        

    }
}
