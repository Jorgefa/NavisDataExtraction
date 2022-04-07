using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Others;
using System.Windows;

namespace NavisDataExtraction.Wpf.ViewModels
{
    internal class EditorViewModel : BaseViewModel
    {
        //Constructor
        public EditorViewModel()
        {
            ConfigFile = Config.FromFile();

            AddNewElementExportTypeCommand = new RelayCommand(AddNewExportType);
            DeleteElementExportTypeCommad = new RelayCommand(DeleteElementExportType);
            DeleteNavisSearcherCommand = new RelayCommand(DeleteNavisSearcher);
            DeleteNavisDataExportCommand = new RelayCommand(DeleteNavisDataExport);
            SaveConfigCommand = new RelayCommand(SaveConfig);
            NewElementExportType = new ElementExportType();

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

        private ElementExportType _selectedElementExportType;

        public ElementExportType SelectedElementExportType
        {
            get => _selectedElementExportType;
            set
            {
                _selectedElementExportType = value;
                OnPropertyChanged();
                SaveConfig();
            }
        }

        private NavisSearcher _selectedNavisSearcher;

        public NavisSearcher SelectedNavisSearcher
        {
            get => _selectedNavisSearcher;
            set
            {
                _selectedNavisSearcher = value;
                OnPropertyChanged();
            }
        }

        private NavisDataExport _selectedNavisDataExport;

        public NavisDataExport SelectedNavisDataExport
        {
            get { return _selectedNavisDataExport; }
            set
            {
                _selectedNavisDataExport = value;
                OnPropertyChanged();
            }
        }

        private ElementExportType _newElementExportType;

        public ElementExportType NewElementExportType
        {
            get => _newElementExportType;
            set
            {
                _newElementExportType = value;
                OnPropertyChanged();
            }
        }

        //Methods

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            var input = Dialogs.Dialogs.ShowInputDialog();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            ConfigFile.CurrentElementExportTypes.Add(new ElementExportType(input));
            ConfigFile.ToFile();
        }
        public RelayCommand DeleteElementExportTypeCommad { get; set; }
        private void DeleteElementExportType()
        {
            ConfigFile.CurrentElementExportTypes.Remove(SelectedElementExportType);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteNavisSearcherCommand { get; set; }
        private void DeleteNavisSearcher()
        {
            SelectedElementExportType.SearcherList.Remove(SelectedNavisSearcher);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }
        private void DeleteNavisDataExport()
        {
            SelectedElementExportType.DataExportList.Remove(SelectedNavisDataExport);
            ConfigFile.ToFile();
        }

        public RelayCommand SaveConfigCommand { get; set; }
        private void SaveConfig()
        {
            if (ConfigFile.ConfigValidation())
            {
                ConfigFile.ToFile();
            }
            else
            {
                MessageBox.Show("Please enter correct data or remove empty searchers or data to export");
            }
        }

    }
}