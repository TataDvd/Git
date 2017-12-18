using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.Views.FirebirdUpdater
{
    /// <summary>
    /// Interaction logic for FirebirdUpdater.xaml
    /// </summary>
    public partial class FirebirdUpdater : Window
    {
        public string Command { get; private set; }
        public string Rez { get; private set; }

        public FirebirdUpdater()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".sql";
            dlg.Filter = "Text Files (.txt)|*.txt|Sql Files (*.sql)|*.sql|All Files (*.*)|*.*";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Editor.Text = File.ReadAllText(filename); 
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            showvartok();
            Command = Editor.Text;
            var bw = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            bw.DoWork += new DoWorkEventHandler(DoWork1);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
           
        }

        private void DoWork1(object sender, DoWorkEventArgs e)
        {
            var context = new EntityFramework.TempoDataBaseContext();
            Rez=context.FbBatchExecution(Command);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lab.Visibility = Visibility.Hidden;
            et.Visibility = Visibility.Hidden;
            vartok.Visibility = Visibility.Hidden;
            if (string.IsNullOrWhiteSpace(Rez))
            {
                MessageBoxWrapper.Show("Командата се изпълни успешно");
            }
            else
            {
                MessageBoxWrapper.Show("Командата даде грешка "+Rez);
            }
        }
        private void showvartok()
        {
            lab.Visibility = Visibility.Visible;
            et.Visibility = Visibility.Visible;
            vartok.Visibility = Visibility.Visible;
        }
    }
}
