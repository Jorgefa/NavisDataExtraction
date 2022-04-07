using Autodesk.Navisworks.Api;
using NavisDataExtraction.Others;
using NavisDataExtraction.DataExport;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using NavisDataExtraction.DataClasses;

namespace NavisDataExtraction.Wpf.ViewModels
{
    internal class ExtractionViewModel : BaseViewModel
    {
        public ExtractionViewModel()
        {
            ConfigFile = Config.FromFile();
            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
            ElementExportTypes = new ObservableCollection<ElementExportType>();
            foreach (var exportType in ConfigFile.CurrentElementExportTypes)
            {
                ElementExportTypes.Add(exportType);
            }
            CollectElementsCommand = new RelayCommand(CollectElements);
            ExportDataCommand = new RelayCommand(ExportData);
        }

        //Properties
        public Config ConfigFile { get; set; }

        private ObservableCollection<ModelItem> _modelItems;

        public ObservableCollection<ModelItem> ModelItems
        {
            get => _modelItems;
            set
            {
                _modelItems = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<ElementExportType> SelectedElementExportTypes = new ObservableCollection<ElementExportType>();

        private ObservableCollection<NavisworksProperty> _properties;

        public ObservableCollection<NavisworksProperty> Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                OnPropertyChanged();
            }
        }

        private ModelItem _selectedItem;

        public ModelItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (value == null) return;
                Properties = GetProperties(value);
            }
        }

        //Methods
        //Returns properties from a ModelItem
        private ObservableCollection<NavisworksProperty> GetProperties(ModelItem modelItem)
        {
            ObservableCollection<NavisworksProperty> props = new ObservableCollection<NavisworksProperty>();
            foreach (PropertyCategory propertyCategory in modelItem.PropertyCategories)
            {
                foreach (DataProperty property in propertyCategory.Properties)
                {
                    props.Add(new NavisworksProperty
                    {
                        Category = propertyCategory,
                        Property = property
                    });
                }
            }

            return props;
        }

        //Select elements using ElmentExporTypes
        public RelayCommand CollectElementsCommand { get; set; }

        private void CollectElements()
        {
            ObservableCollection<ElementExportType> elementExportTypes = SelectedElementExportTypes;
            ObservableCollection<ElementExport> elementExportList = NavisDataCollector.ElementCollectorByListOfTypes(elementExportTypes);
            var elements = elementExportList.Select(e => e.Element).ToList();
            ModelItems = new ObservableCollection<ModelItem>(elements);
        }

        public RelayCommand ExportDataCommand { get; set; }

        //Export elements using ElmentExporTypes
        public void ExportData()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = dialog.FileName;

                var config = ConfigFile;
                ObservableCollection<ElementExportType> elementExportTypes = SelectedElementExportTypes;

                var navisDataTable = DataExport.NavisDataExtraction.CreateNavisDatatable(elementExportTypes);

                navisDataTable.ToCSV(filePath);
            }
        }

        //Save config file in a local path
        public RelayCommand SaveConfigCommand { get; set; }

        private void SaveConfig()
        {
            ConfigFile.ToFile();
        }
    }
}