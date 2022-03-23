using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;

namespace NavisDataExtraction.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ClickCommand = new RelayCommand(Click);
            ModelItems = new ObservableCollection<ModelItem>();
            Properties = new ObservableCollection<NavisworksProperty>();
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

        public RelayCommand ClickCommand { get; set; }
        private void Click()
        {
            var elements = ElementSelector.ElementGathering("Revit Type", "Category");
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
