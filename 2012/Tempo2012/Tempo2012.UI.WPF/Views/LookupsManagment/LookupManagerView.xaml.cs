using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;

namespace Tempo2012.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for LookUpSpecific.xaml
    /// </summary>
    public partial class LookupManagerView : UserControl
    {
        private LookupManagerViewModel vm;
        public LookupManagerView()
        {
            vm = new LookupManagerViewModel();
            InitializeComponent();
            this.DataContext = vm;
        }

        public LookupManagerView(int lookupkey)
        {
            vm=new LookupManagerViewModel(lookupkey);
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CopyLookupFromFirmToFirm cf=new CopyLookupFromFirmToFirm();
            cf.ShowDialog();
        }

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var text = sender as TextBox;
        //    if (text != null)
        //    {
        //        var tag = text.Tag as Filter;
        //        vm.Refresh(tag);
        //        dg.Items.Refresh();
        //    }
        //}

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Dg.SelectRowByIndex(dg, 0);
            }
        }


        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = sender as TextBox;
                if (text != null)
                {
                    var tag = text.Tag as Filter;
                    vm.Refresh(tag);
                    dg.Items.Refresh();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете всички елементи от номенклатурата?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                string rez=vm.DeleteAllItems();
                if (rez=="")
                {
                    MessageBoxWrapper.Show("Записите са изтрити");
                   
                }
                else
                {
                    MessageBoxWrapper.Show("Грешка \n"+rez);
                }

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете всички елементи от номенклатурата за фирмата?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                string rez = vm.DeleteAllItemsFirma();
                if (rez=="")
                {
                    MessageBoxWrapper.Show("Записите са изтрити");

                }
                else
                {
                    MessageBoxWrapper.Show("Грешка \n"+rez);
                }

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
           vm.TransverLookUp();
       
        }
    }
}
