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
using System.IO;

namespace Tempo2012.UI.WPF
{
    /// <summary>
    /// Interaction logic for Banner.xaml
    /// </summary>
    public partial class Banner : UserControl
    {
        public Banner()
        {
            InitializeComponent();
            LoadReclama();
        }

        private void LoadReclama()
        {
            TextRange textRange;
            FileStream fileStream;

            if (File.Exists("Tempo.rtf"))
            {
                textRange = new TextRange(ReclaMa.Document.ContentStart, ReclaMa.Document.ContentEnd);
                using (fileStream = new System.IO.FileStream("Tempo.rtf", System.IO.FileMode.OpenOrCreate))
                {
                    textRange.Load(fileStream, System.Windows.DataFormats.Rtf);
                }
            }
        }
    }
}
