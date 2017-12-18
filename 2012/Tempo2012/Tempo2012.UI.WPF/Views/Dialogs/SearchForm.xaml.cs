using System;
using System.Collections.Generic;
using System.Data;
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
using ReportBuilder;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;

namespace Tempo2012.UI.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for SearchForm.xaml
    /// </summary>
    public partial class SearchForm : Window
    {
        private SearchViewModel vm;
        public SearchForm()
        {
            InitializeComponent();
        }
        public SearchForm(ISerchable iSerchable)
        {
            vm = new SearchViewModel(iSerchable);
            DataContext = vm;
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                var MyCollectionView = CollectionViewSource.GetDefaultView(vm.Items.Rows) as CollectionView;
                if (MyCollectionView != null)
                {
                    MyCollectionView.Filter = o => (o as DataRow).ItemArray[0].ToString().Contains("2");
                }
            }
        }
    }
}
