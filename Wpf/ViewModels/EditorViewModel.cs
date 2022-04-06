using NavisDataExtraction.Commands;
using NavisDataExtraction.DataExport;
using System.Collections.Generic;
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
            AddNewElementExportTypeCommand = new RelayCommand(AddNewExportType);
            DeleteElementExportTypeCommad = new RelayCommand(DeleteElementExportType);

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

        private string _newElementExportTypeName;

        public string NewElementExportTypeName
        {
            get => _newElementExportTypeName;
            set
            {
                _newElementExportTypeName = value;
                OnPropertyChanged();
            }
        }


        //Methods

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            ElementExportType newExportType = new ElementExportType(NewElementExportTypeName);
            List<ElementExportType> curEET = ConfigFile.CurrentElementExportTypes;
            curEET.Add(newExportType);
            ConfigFile.ToFile();
            ElementExportTypes.Add(newExportType);
        }

        public RelayCommand DeleteElementExportTypeCommad { get; set; }
        private void DeleteElementExportType()
        {
            List<ElementExportType> curEET = ConfigFile.CurrentElementExportTypes;
            curEET.Remove(SelectedElementExportType);
            ElementExportTypes.Remove(SelectedElementExportType);
            ConfigFile.ToFile();
        }
    }
}