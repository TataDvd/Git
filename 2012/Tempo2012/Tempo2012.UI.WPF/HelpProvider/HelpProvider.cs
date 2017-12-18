using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using form = System.Windows.Forms;
using System.Windows.Media;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Tempo2012.UI.WPF.HelpProvider
{
    public static class HelpProvider
    {
        public static string GetHelpStringFromObject(DependencyObject obj)
        {
            return (string)obj.GetValue(HelpStringProperty);
        }

        public static void SetHelpString(DependencyObject obj, string value)
        {
            obj.SetValue(HelpStringProperty, value);
        }

        public static readonly DependencyProperty HelpStringProperty =
            DependencyProperty.RegisterAttached("HelpString", typeof(string), typeof(HelpProvider));

        public static string GetHelpString(MouseEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            if (source != null)
            {
                string helpString = GetHelpStringFromObject(source);
                return helpString;
            }
            return null;
        }

        public static void OpenHelper(string topicId)
        {
            string path = Environment.CurrentDirectory + @"\Help\Tempo2012.chm";
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                topicId = String.IsNullOrEmpty(topicId) ? "0" : topicId;
                Help.ShowHelp(null, path, HelpNavigator.TopicId, topicId);
            }
        }
    }
}   
 