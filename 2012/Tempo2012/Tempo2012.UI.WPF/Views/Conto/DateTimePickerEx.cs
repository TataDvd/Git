using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Tempo2012.UI.WPF.Views
{
    public class DateTimePickerEx:DatePicker
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DatePickerTextBox _textBox = GetTemplateChild("TextBox") as DatePickerTextBox;

            if (_textBox != null)
            {
                _textBox.AddHandler(KeyDownEvent, new KeyEventHandler(TextBox_KeyDown), true);
            }
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                e.Handled = false;
        } 
    }
}