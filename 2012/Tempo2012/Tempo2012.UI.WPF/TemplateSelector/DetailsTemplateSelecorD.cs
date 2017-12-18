using System.Windows;
using System.Windows.Controls;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;

namespace Tempo2012.UI.WPF.TemplateSelector
{
    public class DetailsTemplateSelecorD : DataTemplateSelector
    {
        public DataTemplate ShowTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                                                    DependencyObject container)
        {
            ContentPresenter presenter = container as ContentPresenter;
            DataGridCell cell = presenter.Parent as DataGridCell;
            if (cell != null)
            {
                var wraperConto = cell.DataContext as WraperConto;
                if (wraperConto != null)
                {
                    string node = wraperConto.DDetails;
                    if (string.IsNullOrWhiteSpace(node))
                        return EmptyTemplate;
                }
            }


            return ShowTemplate;
            
        }
    }
}