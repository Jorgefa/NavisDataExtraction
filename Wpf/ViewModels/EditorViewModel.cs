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

            SaveConfigCommand = new RelayCommand(ConfigFile.SaveConfig);
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

        private NavisExtractionType _selectedElementExportType;

        public NavisExtractionType SelectedElementExportType
        {
            get => _selectedElementExportType;
            set
            {
                _selectedElementExportType = value;
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
            SelectedCollection.Types.Remove(SelectedElementExportType);
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

    }
}