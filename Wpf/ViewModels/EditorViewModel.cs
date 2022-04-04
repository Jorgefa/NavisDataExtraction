using NavisDataExtraction.DataExport;
using System.Collections.ObjectModel;

namespace NavisDataExtraction.Wpf.ViewModels
{
    internal class EditorViewModel : BaseViewModel
    {
        //Constructor
        public EditorViewModel()
        {
            ConfigFile = Config.FromFile();
            ElementExportTypes = new ObservableCollection<ElementExportType>();
            foreach (var exportType in ConfigFile.CurrentElementExportTypes)
            {
                ElementExportTypes.Add(exportType);
            }
        }

        //Properties
        public Config ConfigFile { get; set; }

        private ObservableCollection<ElementExportType> _elementExportTypes;

        public ObservableCollection<ElementExportType> ElementExportTypes
        {
            get => _elementExportTypes;
            set
            {
                _elementExportTypes = value;
                OnPropertyChanged();
            }
        }

        private ElementExportType _selectedElementExportType;

        public ElementExportType SelectedElementExportType
        {
            get => _selectedElementExportType;
            set
            {
                _selectedElementExportType = value;
                OnPropertyChanged();
            }
        }
    }
}