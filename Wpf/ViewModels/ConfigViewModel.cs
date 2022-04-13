using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Others;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace NavisDataExtraction.Wpf.ViewModels
{
    internal class ConfigViewModel : BaseViewModel
    {
        //Constructor
        public ConfigViewModel()
        {
            ConfigFile = Config.FromFile();

            AddNewElementExportTypeCommand = new RelayCommand(AddNewExportType);
            DuplicateSelectedCollectionCommand = new RelayCommand(DuplicateSelectedCollection);
            AddNewCollectionCommand = new RelayCommand(AddNewCollection);
            DuplicateSelectedTypeCommand = new RelayCommand(DuplicateSelectedType);
            DeleteCollectionCommad = new RelayCommand(DeleteCollection);
            DeleteElementExportTypeCommad = new RelayCommand(DeleteElementExportType);
            DeleteNavisSearcherCommand = new RelayCommand(DeleteNavisSearcher);
            DeleteNavisDataExportCommand = new RelayCommand(DeleteNavisData);

            RenameCollectionCommand = new RelayCommand(RenameCollection);
            RenameTypeCommand = new RelayCommand(RenameType);

            SaveCurrentConfigCommand = new RelayCommand(ConfigFile.SaveConfig);
            ImportConfigCommand = new RelayCommand(ImportConfig);
            ExportConfigCommand = new RelayCommand(ConfigFile.ExportConfigToFile);
            ClearConfigCommand = new RelayCommand(ClearConfig);
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
                System.Windows.Forms.MessageBox.Show("Please enter a valid name");
                return;
            }
            if (ConfigFile.NavisExtractionTypeCollections == null)
            {
                ConfigFile.NavisExtractionTypeCollections = new ObservableCollection<NavisExtractionTypeCollection>();
            }

            var newCollection = new NavisExtractionTypeCollection(input);
            ConfigFile.NavisExtractionTypeCollections.Add(newCollection);

            switch (ConfigFile.CollectionsValidation())
            {
                case "ok":
                    ConfigFile.SaveConfig();
                    break;

                case "duplicates":
                    ConfigFile.NavisExtractionTypeCollections.Remove(newCollection);
                    System.Windows.Forms.MessageBox.Show("Name already exists");
                    break;

                default:
                    System.Windows.Forms.MessageBox.Show("Unknow error");
                    break;
            }
        }

        public RelayCommand DuplicateSelectedCollectionCommand { get; set; }

        private void DuplicateSelectedCollection()
        {
            var newCollection = new NavisExtractionTypeCollection
            {
                Name = SelectedCollection.Name + "_(Copy)",
                Types = SelectedCollection.Types
            };
            ConfigFile.NavisExtractionTypeCollections.Add(newCollection);
            ConfigFile.SaveConfig();
        }

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            if (SelectedCollection == null)
            {
                System.Windows.Forms.MessageBox.Show("Please, select a collection.", "Error");
                return;
            }
            var input = Dialogs.Dialogs.ShowInputDialog("New ElementExportType", "Please, enter new extraction type's name.");
            if (string.IsNullOrEmpty(input))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid name.", "Error");
                return;
            }
            if (SelectedCollection.Types == null)
            {
                SelectedCollection.Types = new ObservableCollection<NavisExtractionType>();
            }

            var newType = new NavisExtractionType(input);
            SelectedCollection.Types.Add(newType);

            switch (SelectedCollection.TypesValidation())
            {
                case "ok":
                    ConfigFile.SaveConfig();
                    break;

                case "duplicates":
                    SelectedCollection.Types.Remove(newType);
                    System.Windows.Forms.MessageBox.Show("Name already exists");
                    break;

                default:
                    System.Windows.Forms.MessageBox.Show("Unknow error");
                    break;
            }
        }

        public RelayCommand DuplicateSelectedTypeCommand { get; set; }

        private void DuplicateSelectedType()
        {
            var newType = new NavisExtractionType
            {
                Name = SelectedNavisExtractionType.Name + "_(Copy)",
                Searchers = SelectedNavisExtractionType.Searchers,
                Datas = SelectedNavisExtractionType.Datas
            };
            SelectedCollection.Types.Add(newType);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteCollectionCommad { get; set; }

        private void DeleteCollection()
        {
            ConfigFile.NavisExtractionTypeCollections.Remove(SelectedCollection);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteElementExportTypeCommad { get; set; }

        private void DeleteElementExportType()
        {
            SelectedCollection.Types.Remove(SelectedNavisExtractionType);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteNavisSearcherCommand { get; set; }

        private void DeleteNavisSearcher()
        {
            SelectedNavisExtractionType.Searchers.Remove(SelectedNavisSearcher);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }

        private void DeleteNavisData()
        {
            SelectedNavisExtractionType.Datas.Remove(SelectedNavisData);
            ConfigFile.SaveConfig();
        }

        public RelayCommand RenameCollectionCommand { get; set; }

        private void RenameCollection()
        {
            var input = Dialogs.Dialogs.ShowInputDialog("Change Collection Name", "Please, enter new collection's name");
            if (string.IsNullOrEmpty(input))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid name");
                return;
            }

            var collectionNames = ConfigFile.NavisExtractionTypeCollections.ToList().Select(x => x.Name).ToList();

            if (collectionNames.Contains(input))
            {
                System.Windows.Forms.MessageBox.Show("This collection name already exists.");
                return;
            }
            SelectedCollection.Name = input;
            ConfigFile.SaveConfig();
        }

        public RelayCommand RenameTypeCommand { get; set; }

        private void RenameType()
        {
            var input = Dialogs.Dialogs.ShowInputDialog("Change Type Name", "Please, enter new collection's name.");
            if (string.IsNullOrEmpty(input))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid name.", "Error");
                return;
            }
            var typeNames = SelectedCollection.Types.ToList().Select(x => x.Name).ToList();

            if (typeNames.Contains(input))
            {
                System.Windows.Forms.MessageBox.Show("This type name already exists.", "Error");
                return;
            }
            SelectedNavisExtractionType.Name = input;
            ConfigFile.SaveConfig();
        }

        //Config methods

        public RelayCommand ClearConfigCommand { get; set; }

        private void ClearConfig()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure that you want to reset the configuration and clear all collections?", "Reset configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ConfigFile = new Config();
                ConfigFile.SaveConfig();
            }
        }

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