using NavisDataExtraction.Configuration;

using NavisDataExtraction.Wpf.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

using System.Windows.Controls;

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

        //Methods
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as ExtractionViewModel;
            vm.SelectedElementExportTypes = new ObservableCollection<NdeType>(ElementExportTypesList.SelectedItems.Cast<NdeType>());
        }
    }
}