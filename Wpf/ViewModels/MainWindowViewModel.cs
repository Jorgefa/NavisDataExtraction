using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;
using NavisDataExtraction.DataCollector;
using NavisDataExtraction.DataExport;
using NavisDataExtraction.Wpf.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NavisDataExtraction.Wpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        //Constructors
        public MainWindowViewModel()
        {
            CollectElementsCommand = new RelayCommand(CollectElements);
            SaveConfigCommand = new RelayCommand(SaveConfig);
            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
            ConfigFile = Config.FromFile();
            ElementExportTypes = new ObservableCollection<ElementExportType>();
            foreach (var exportType in ConfigFile.CurrentElementExportTypes)
            {
                ElementExportTypes.Add(exportType);
            }
        }

        //Properties
        public Config ConfigFile { get; set; }

        public List<ElementExportType> SelectedElementExportTypes;

        private ObservableCollection<ModelItem> _modelItems;

        //UI properties
        private BaseViewModel _selectedViewModel = new EditorViewModel();

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; }
        }

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
            var props = new ObservableCollection<NavisworksProperty>();
            foreach (var propertyCategory in modelItem.PropertyCategories)
            {
                foreach (var property in propertyCategory.Properties)
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

        //Select elements using an ElementExportType
        public RelayCommand CollectElementsCommand { get; set; }

        private void CollectElements()
        {
            List<ElementExportType> elementExportTypes = SelectedElementExportTypes;
            var elements = NavisDataCollector.ElementCollector(elementExportTypes);
            ModelItems = new ObservableCollection<ModelItem>(elements);
        }

        //Save config file in a local path
        public RelayCommand SaveConfigCommand { get; set; }

        private void SaveConfig()
        {
            ConfigFile.ToFile();
        }

    }
}