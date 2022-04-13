using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataClasses;
using NavisDataExtraction.DataEdition;
using NavisDataExtraction.DataExport;
using NavisDataExtraction.Others;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace NavisDataExtraction.Wpf.ViewModels
{
    internal class ExtractionViewModel : BaseViewModel
    {
        //COnstructor
        public ExtractionViewModel()
        {
            ConfigFile = Config.FromFile();

            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
            ElementExportTypes = new ObservableCollection<NavisExtractionType>(ConfigFile.CurrentElementExportTypes);
            CollectElementsCommand = new RelayCommand(CollectElements);
            ExportDataCommand = new RelayCommand(ExportData);
            ImportConfigCommand = new RelayCommand(ImportConfig);
            ExportConfigCommand = new RelayCommand(ConfigFile.ExportConfigToFile);

            AddDataCommand = new RelayCommand(AddData);
            IsolateElementsCommand = new RelayCommand(IsolateElements);
        }

        //Properties
        private Config _configFile;

        public Config ConfigFile
        {
            get => _configFile;
            set
            {
                _configFile = value;
                OnPropertyChanged();
            }
        }

        private NavisExtractionTypeCollection _selectedCollection;

        public NavisExtractionTypeCollection SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                _selectedCollection = value;
                OnPropertyChanged();
            }
        }

        private NavisExtractionType _selectedNavisExtractionType;

        public NavisExtractionType SelectedNavisExtractionType
        {
            get => _selectedNavisExtractionType;
            set
            {
                _selectedNavisExtractionType = value;
                OnPropertyChanged();
            }
        }

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

        private ObservableCollection<NavisExtractionType> _elementExportTypes;

        public ObservableCollection<NavisExtractionType> ElementExportTypes
        {
            get => _elementExportTypes;
            set
            {
                _elementExportTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NavisExtractionType> SelectedElementExportTypes = new ObservableCollection<NavisExtractionType>();

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

        //Select elements using NavisExtractionTypes
        public RelayCommand CollectElementsCommand { get; set; }

        private void CollectElements()
        {
            if (SelectedCollection == null)
            {
                MessageBox.Show("Please, select a collection.", "Error");
                return;
            }
            if (SelectedCollection.Types == null)
            {
                MessageBox.Show("Please, select a collection with extraction types.", "Error");
                return;
            }
            ObservableCollection<NavisExtractionElement> elementExportList = NavisDataCollector.ElementCollectorByListOfTypes(SelectedCollection.Types);
            var elements = elementExportList.Select(e => e.Element).ToList();
            ModelItems = new ObservableCollection<ModelItem>(elements);
        }

        //Export elements using ElmentExporTypes
        public RelayCommand ExportDataCommand { get; set; }

        public void ExportData()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = dialog.FileName;

                ObservableCollection<NavisExtractionType> elementExportTypes = SelectedCollection.Types;

                var navisDataTable = DataExport.NavisDataExtraction.CreateNavisDatatable(elementExportTypes);

                navisDataTable.ToCSV(filePath);
            }
        }

        //Config commands

        public RelayCommand ImportConfigCommand { get; set; }

        private void ImportConfig()
        {
            var newConfigFile = Config.ImportConfigFromFile();
            if (newConfigFile != null)
            {
                ConfigFile = newConfigFile;
            }
            else
            {
                System.Windows.MessageBox.Show("Wrong config file. New config not imported.");
            }
        }

        public RelayCommand ExportConfigCommand { get; set; }

        // TESTING
        //Adding data
        public RelayCommand AddDataCommand { get; set; }
        private void AddData()
        {
            var elements = new List<ModelItem>(ModelItems);
            DataAdder.AddCoordinates(elements);
        }

        //Isolate elements
        public RelayCommand IsolateElementsCommand { get; set; }
        private void IsolateElements()
        {
            NavisDataCollector.IsolateElements(ModelItems);
        }
    }
}