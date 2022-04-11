using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Others;
using System.Collections.ObjectModel;
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
            AddNewCollectionCommand = new RelayCommand(AddNewCollection);
            DeleteCollectionCommad = new RelayCommand(DeleteCollection);
            DeleteElementExportTypeCommad = new RelayCommand(DeleteElementExportType);
            DeleteNavisSearcherCommand = new RelayCommand(DeleteNavisSearcher);
            DeleteNavisDataExportCommand = new RelayCommand(DeleteNavisDataExport);

            SaveCurrentConfigCommand = new RelayCommand(ConfigFile.SaveConfig);
            ImportConfigCommand = new RelayCommand(ImportConfig);
            ExportConfigCommand = new RelayCommand(ConfigFile.ExportConfigToFile);
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
                ConfigFile.SaveConfig();
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
                ConfigFile.SaveConfig();
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

        //Methods

        public RelayCommand AddNewCollectionCommand { get; set; }

        private void AddNewCollection()
        {
            var input = Dialogs.Dialogs.ShowInputDialog("New Collection", "Please, enter new collection's name");
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            if (ConfigFile.NavisExtractionTypeCollections == null)
            {
                ConfigFile.NavisExtractionTypeCollections = new ObservableCollection<NavisExtractionTypeCollection>();
            }
            ConfigFile.NavisExtractionTypeCollections.Add(new NavisExtractionTypeCollection(input));
            ConfigFile.ToFile();
        }

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            var input = Dialogs.Dialogs.ShowInputDialog("New ElementExportType", "Please, enter new extraction type's name");
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            if (SelectedCollection.Types == null)
            {
                SelectedCollection.Types = new ObservableCollection<NavisExtractionType>();
            }
            SelectedCollection.Types.Add(new NavisExtractionType(input));
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteCollectionCommad { get; set; }

        private void DeleteCollection()
        {
            ConfigFile.NavisExtractionTypeCollections.Remove(SelectedCollection);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteElementExportTypeCommad { get; set; }

        private void DeleteElementExportType()
        {
            SelectedCollection.Types.Remove(SelectedNavisExtractionType);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteNavisSearcherCommand { get; set; }

        private void DeleteNavisSearcher()
        {
            SelectedNavisExtractionType.Searchers.Remove(SelectedNavisSearcher);
            ConfigFile.ToFile();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }

        private void DeleteNavisDataExport()
        {
            SelectedNavisExtractionType.Datas.Remove(SelectedNavisData);
            ConfigFile.ToFile();
        }

        //Config commands

        public RelayCommand SaveCurrentConfigCommand { get; set; }

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
    }
}