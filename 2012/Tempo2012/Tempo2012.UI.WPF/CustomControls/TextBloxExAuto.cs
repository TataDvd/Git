using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using dragonz.actb.core;
using dragonz.actb.provider;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class TextBoxExAuto:TextBoxEx
    {
        public TextBoxExAuto()
        {
            _acm = new AutoCompleteManager();
            _acm.DataProvider = new SimpleStaticDataProvider(Entrence.ProviderList);
            _acm.AutoAppend = true;
            this.Loaded += AutoCompleteTextBoxLoaded;
        }
        private AutoCompleteManager _acm;

        public AutoCompleteManager AutoCompleteManager
        {
            get { return _acm; }
        }
        void AutoCompleteTextBoxLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
           if (Entrence.UseIntelliSense)  _acm.AttachTextBox(this);
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (Entrence.UseIntelliSense && !Entrence.ProviderList.Contains(Text))
            {
                Entrence.ProviderList.Add(Text);
            }
            base.OnLostFocus(e);
        }
    }
}
