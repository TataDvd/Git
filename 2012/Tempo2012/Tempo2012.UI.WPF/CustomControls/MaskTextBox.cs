using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using WindowsInput;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class MaskTextBox : TextBox
    {
        public static DependencyProperty DataTypeProperty =
   DependencyProperty.Register("DataType", typeof(string), typeof(MaskTextBox), new PropertyMetadata("string"));
        public static DependencyProperty RegExProperty =
  DependencyProperty.Register("RegEx", typeof(string), typeof(MaskTextBox), new PropertyMetadata("string"));


        public string DataType
        {
            get { return (string)GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        public string RegEx
        {
            get { return (string)GetValue(RegExProperty); }
            set { SetValue(RegExProperty, value); }
        }

        public MaskTextBox()
            : base()
        {
            EventManager.RegisterClassHandler(
                typeof(MaskTextBox),
                DataObject.PastingEvent,
                (DataObjectPastingEventHandler)((sender, e) =>
                                                     {
                                                         if (!IsDataValid(e.DataObject))
                                                         {
                                                             DataObject data = new DataObject();
                                                             data.SetText(String.Empty);
                                                             e.DataObject = data;
                                                             e.Handled = false;
                                                         }
                                                     }));
            this.AddHandler(MaskTextBox.PreviewKeyDownEvent, new RoutedEventHandler(PreviewKeyDownEventHandler));
            this.AddHandler(MaskTextBox.LostFocusEvent, new RoutedEventHandler(LostFocusEventHandler));
            if (Entrence.FontSize > 0)
            {
                FontSize = Entrence.FontSize;
            }

        }

        //public void GotFocusEventHandler(object sender, RoutedEventArgs e)
        //{
        //    (sender as MaskTextBox).SelectAll();
        //    e.Handled = true;
        //}
        public void LostFocusEventHandler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Text) || this.Text == "-")
                this.Text = "0";
            
        }

        public void PreviewKeyDownEventHandler(object sender, RoutedEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Key.Space)
            {
                ke.Handled = true;
            }
            if (ke.Key == Key.Enter)
            {
                var bindingExpression = ((MaskTextBox)sender).GetBindingExpression(TextProperty);
                if (bindingExpression != null)
                    bindingExpression.UpdateSource();
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = !IsDataValid(e.Data);
            base.OnDrop(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (!IsDataValid(e.Data))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.None;
            }
            base.OnDragEnter(e);
        }

        private Boolean IsDataValid(
            IDataObject data)
        {

            Boolean isValid = false;
            if (data != null)
            {
                String text = data.GetData(DataFormats.Text) as String;
                if (!String.IsNullOrEmpty(text == null ? null : text.Trim()))
                {
                    switch (DataType)
                    {
                        case "INT":
                            Int32 result = -1;
                            if (Int32.TryParse(text.Trim(), out result))
                            {
                                if (result > 0)
                                {
                                    isValid = true;
                                }
                            }
                            break;

                        case "decimal":
                            decimal decimalResult = -1;
                            if (decimal.TryParse(text.Trim(), out decimalResult))
                            {
                                if (decimalResult > 0)
                                {
                                    isValid = true;
                                }
                            }
                            
                            break;
                        case "RegEx":
                            if (System.Text.RegularExpressions.Regex.IsMatch(text, RegEx))
                            {
                                isValid = true;
                            }
                            break;

                    }
                }
            }
            return isValid;
        }


        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string text = this.Text;
            if (text.Length >= this.CaretIndex) text = text.Insert(this.CaretIndex, e.Text);
            switch (DataType)
            {
                case "INT":
                    Int32 result = -1;
                    if (!Int32.TryParse(text.Trim(), out result))
                    {
                        if (!text.Equals("-"))
                            e.Handled = true;
                    }
                    break;

                case "decimal":
                    decimal decimalResult = -1;
                    NumberStyles ns = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
                    if (!decimal.TryParse(text.Trim(), ns, Thread.CurrentThread.CurrentCulture, out decimalResult))
                    {
                        if (!text.Equals("-"))
                        {
                            e.Handled = true;
                        }
                    }
                    
                    
                    break;
                case "RegEx":
                    if (!System.Text.RegularExpressions.Regex.IsMatch(text, RegEx))
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }
        private string _oldText;
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            _oldText = Text;
            base.OnGotFocus(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_modify)
                {
                    Text = _oldText;
                    e.Handled = true;
                    _modify = false;
                }
                else
                {
                    e.Handled = true;
                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            _modify = true;
            base.OnTextChanged(e);
        }

        private bool _modify;
    }
}