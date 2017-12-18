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

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for LabelTextBox.xaml
    /// </summary>
    public partial class LabelTextBox : UserControl
    {
        public LabelTextBox()
        {
            InitializeComponent();
        }

        static void textChangedCallBack(DependencyObject property,
        DependencyPropertyChangedEventArgs args)
        {
            LabelTextBox searchTextBox = (LabelTextBox)property;
            searchTextBox.textBox1.Text = (string)args.NewValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(
           "Text",
           typeof(string),
           typeof(LabelTextBox),
           new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(textChangedCallBack)));

        public string Label
        {
            get { return textBlock1.Text; }
            set {textBlock1.Text=value;}
        }

       
    }
}
