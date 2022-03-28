using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;
using NavisDataExtraction.DataCollector;
using NavisDataExtraction.DataExport;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NavisDataExtraction.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            CollectElementsCommand = new RelayCommand(CollectElements);
            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
            ConfigFile = Config.FromFile();
            ElementExportTypes = new ObservableCollection<ElementExportType>();
            foreach (var exportType in ConfigFile.CurrentElementExportTypes)
            {
                ElementExportTypes.Add(exportType);
            }
        }

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

        public List<ElementExportType> SelectedElementExportTypes;

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

        public RelayCommand CollectElementsCommand { get; set; }

        private void CollectElements()
        {
            List<ElementExportType> elementExportTypes = ConfigFile.CurrentElementExportTypes;
            var elements = NavisDataCollector.ElementCollector(elementExportTypes);
            ModelItems = new ObservableCollection<ModelItem>(elements);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}