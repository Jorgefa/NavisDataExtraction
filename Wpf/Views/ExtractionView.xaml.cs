using NavisDataExtraction.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NavisDataExtraction.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Extraction.xaml
    /// </summary>
    public partial class ExtractionView : UserControl
    {
        public ExtractionView()
        {
            InitializeComponent();
        }

        ////Methods
        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var vm = DataContext as MainWindowViewModel;
        //    vm.SelectedElementExportTypes = ElementExportTypesList.SelectedItems.Cast<ElementExportType>().ToList();
        //}
    }
}
