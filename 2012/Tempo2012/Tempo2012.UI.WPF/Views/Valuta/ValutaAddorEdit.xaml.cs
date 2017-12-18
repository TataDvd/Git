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
using System.Windows.Shapes;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.Views.Valuta
{
    /// <summary>
    /// Interaction logic for ValutaAddorEdit.xaml
    /// </summary>
    public partial class ValutaAddorEdit : Window
    {
        bool isInsertMode = false;
        bool isBeingEdited = false;
        ValutaAddorEditViewMode vm;
        public ValutaAddorEdit()
        {
            InitializeComponent();
            vm = new ValutaAddorEditViewMode(ConfigTempoSinglenton.GetInstance().WorkDate,0);
            DataContext = vm;
        }

       

        private void dgEmp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ValutaEntityWraper emp = e.Row.DataContext as ValutaEntityWraper;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                e.Cancel = false;
                return;
            }

            if (e.EditAction == DataGridEditAction.Commit)
            {
                vm.SaveKursFromOutSide(emp);
            }
            
        }

        private void dgEmp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && !isBeingEdited)
            {
                var grid = (DataGrid)sender;
                if (grid.SelectedItems.Count > 0)
                {
                    var Res = MessageBoxWrapper.Show("Сигурен ли си че искаш да изтриеш " + grid.SelectedItems.Count + " валутни курса?", "Изтриване на записи", MessageBoxWrapperButton.YesNo);
                    
                    if (Res == MessageBoxWrapperResult.Yes)
                    {
                        List<ValutaEntity> itemsfordelete= grid.SelectedItems.OfType<ValutaEntityWraper>().Select(li => new ValutaEntity {CodeVal = li.CodeVal, Date = li.Date}).ToList();
                        vm.DeleteContextFromOutside(itemsfordelete);
                        MessageBoxWrapper.Show(grid.SelectedItems.Count + "Маркираните курсове са изтрити!");
                    }
                    
                }
            }
        }

        private void dgEmp_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
            ValutaEntityWraper emp = e.Row.DataContext as ValutaEntityWraper;
            if (emp != null) emp.State = 1;
        }
    }
}
