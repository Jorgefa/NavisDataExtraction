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

            //NewElementExportType = new NavisExtractionType();

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

        private NavisExtractionType _selectedElementExportType;

        public NavisExtractionType SelectedElementExportType
        {
            get => _selectedElementExportType;
            set
            {
                _selectedElementExportType = value;
                OnPropertyChanged();
                SaveConfig();
            }
        }

        private NavisExtractionSearcher _selectedNavisSearcher;

        public NavisExtractionSearcher SelectedNavisSearcher
        {
            get => _selectedNavisSearcher;
            set
            {
                _selectedNavisSearcher = value;
                OnPropertyChanged();
            }
        }

        private NavisExtractionData _selectedNavisData;

        public NavisExtractionData SelectedNavisData
        {
            get { return _selectedNavisData; }
            set
            {
                _selectedNavisData = value;
                OnPropertyChanged();
            }
        }

        //private NavisExtractionType _newElementExportType;

        //public NavisExtractionType NewElementExportType
        //{
        //    get => _newElementExportType;
        //    set
        //    {
        //        _newElementExportType = value;
        //        OnPropertyChanged();
        //    }
        //}

        //Methods

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            var input = Dialogs.Dialogs.ShowInputDialog("New ElementExportType", "Please, enter the name for the new ElementExportType");
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            ConfigFile.CurrentElementExportTypes.Add(new NavisExtractionType(input));
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
            SelectedElementExportType.Searchers.Remove(SelectedNavisSearcher);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }
        private void DeleteNavisDataExport()
        {
            SelectedElementExportType.Datas.Remove(SelectedNavisData);
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