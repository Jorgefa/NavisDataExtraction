using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Configuration;
using NavisDataExtraction.DataEdition;
using NavisDataExtraction.NavisUtils;
using NavisDataExtraction.Utils;
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
            ConfigFile = NdeConfig.FromFile();

            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
            ExportDataCommand = new RelayCommand(ExportData);
            ImportConfigCommand = new RelayCommand(ImportConfig);
            ExportConfigCommand = new RelayCommand(ConfigFile.ExportConfigToFile);

            SearchModelItemsCommand = new RelayCommand(SearchModelItems);

            AddDataCommand = new RelayCommand(AddData);
            SelectElementsCommand = new RelayCommand(SelectElements);
        }

        //Properties
        private NdeConfig _configFile;

        public NdeConfig ConfigFile
        {
            get => _configFile;
            set
            {
                _configFile = value;
                OnPropertyChanged();
            }
        }

        private NdeCollection _selectedCollection;

        public NdeCollection SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                _selectedCollection = value;
                OnPropertyChanged();
            }
        }

        private NdeType _selectedType;

        public NdeType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NdeModelItemGroup> _modelItemGroups;

        public ObservableCollection<NdeModelItemGroup> ModelItemGroups
        {
            get { return _modelItemGroups; }
            set { _modelItemGroups = value; OnPropertyChanged(); }
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

        public ObservableCollection<NdeType> SelectedElementExportTypes = new ObservableCollection<NdeType>();

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

        public RelayCommand SearchModelItemsCommand { get; set; }

        private void SearchModelItems()
        {
            if (SelectedCollection == null || SelectedCollection.Types == null)
            {
                MessageBox.Show("Please, select a collection with extraction types.", "Error");
                return;
            }
            ModelItems = SelectedCollection.SearchModelItems();
            ModelItemGroups = SelectedCollection.SearchModelItemsGroups();
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

                ObservableCollection<NdeType> elementExportTypes = SelectedCollection.Types;

                var navisDataTable = SelectedCollection.GetDataTable();

                navisDataTable.ToCSV(filePath);
            }
        }

        //Config commands

        public RelayCommand ImportConfigCommand { get; set; }

        private void ImportConfig()
        {
            var newConfigFile = NdeConfig.ImportConfigFromFile();
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
            if (SelectedCollection == null)
            {
                return;
            }
            if (ModelItemGroups == null)
            {
                ModelItemGroups = SelectedCollection.SearchModelItemsGroups();
            }
            SelectedCollection.AddDataToNaviswork(ModelItemGroups);
        }

        //Isolate elements
        public RelayCommand SelectElementsCommand { get; set; }
        private void SelectElements()
        {
            NavisStaticCommands.SelectElements(ModelItems);
        }
    }
}