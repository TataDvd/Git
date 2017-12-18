using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using Tempo2012.EntityFramework.Interface;
using Tempo2012.UI.WPF.PaggingControlProject;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SearchFormAcc.xaml
    /// </summary>
    public partial class SearchFormAcc : Window
    {
        private readonly SearchViewModelAcc vm;
        public SearchFormAcc()
        {
            InitializeComponent();
            vm = new SearchViewModelAcc();
            DataContext = vm;
        }

        

        public ViewModels.ContoManagment.WraperConto CurrentWraperConto { get { return vm.CurrentWraperConto;}}

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                vm.FindDebitAcc();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.AddCommand.Execute(null);
            Close();
        }

        private void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                vm.FindCreditAcc();
            }

        }

       

        private void LoadMask()
        {

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".bin";
          

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //var model = SerializeUtil.DeserializeFromXML<CSearchAcc>(filename);
                // Open the file containing the data that you want to deserialize.
                FileStream fs = new FileStream(filename, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and  
                    // assign the reference to the local variable.
                    var model = (CSearchAcc)formatter.Deserialize(fs);
                    CopyInterface(model, vm);
                    Entrence.Mask = model;
                    string pod = model.CreditAcc != null && model.CreditAcc.SubNum > 0 ? String.Format("/{0}", model.CreditAcc.SubNum) : "";
                    if (model.CreditAcc != null) vm.Credit =string.Format("{0}{1}",model.CreditAcc.Num,pod);
                    string pod1 = model.DebitAcc != null && model.DebitAcc.SubNum > 0 ? String.Format("/{0}", model.DebitAcc.SubNum) : "";
                    if (model.DebitAcc != null) vm.Debit = string.Format("{0}{1}", model.DebitAcc.Num, pod1);
                    vm.PrCh("CreditItems");
                    vm.PrCh("DebitItems");
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }

                
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveMask();
        }

        private void SaveMask()
        {
           
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();



            // Set filter for file extension and default file extension 
            dlg.FileName = "Маск";
            dlg.DefaultExt = ".bin";
            

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                CSearchAcc ac=new CSearchAcc();
                CopyInterface(vm,ac);
                string filename = dlg.FileName;
                //SerializeUtil.SerializeToXML<CSearchAcc>(filename, ac);
                FileStream fs = new FileStream(filename, FileMode.Create);

                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, ac);
                }
                catch (SerializationException e)
                {
                    MessageBoxWrapper.Show("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        
        }
        private void CopyInterface(ISearchAcc source,ISearchAcc dest)
        {
            
           foreach (PropertyInfo info in typeof(ISearchAcc)
                                             .GetProperties(BindingFlags.Instance
                                                             |BindingFlags.Public))
           {
               info.SetValue(dest,info.GetValue(source,null),null);
           }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadMask();
        }
        
    }
}
