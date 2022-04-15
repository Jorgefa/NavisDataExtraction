using Autodesk.Navisworks.Api;
using NavisDataExtraction.Configuration;
using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Utils;
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
            ConfigFile = NdeConfig.FromFile();

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
                ConfigFile.SaveConfig();
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

        private NdeSearcher _selectedNavisSearcher;

        public NdeSearcher SelectedNavisSearcher
        {
            get => _selectedNavisSearcher;
            set
            {
                _selectedNavisSearcher = value;
                OnPropertyChanged();
            }
        }

        private NdeData _selectedNavisData;

        public NdeData SelectedNavisData
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
            if (ConfigFile.Collections == null)
            {
                ConfigFile.Collections = new ObservableCollection<NdeCollection>();
            }

            var newCollection = new NdeCollection(input);
            ConfigFile.Collections.Add(newCollection);

            switch (ConfigFile.CollectionsValidation())
            {
                case "ok":
                    ConfigFile.SaveConfig();
                    break;

                case "duplicates":
                    ConfigFile.Collections.Remove(newCollection);
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
            var newCollection = new NdeCollection
            {
                Name = SelectedCollection.Name + "_(Copy)",
                Types = SelectedCollection.Types
            };
            ConfigFile.Collections.Add(newCollection);
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
                SelectedCollection.Types = new ObservableCollection<NdeType>();
            }

            var newType = new NdeType(input);
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
            var newType = new NdeType
            {
                Name = SelectedType.Name + "_(Copy)",
                Searchers = SelectedType.Searchers,
                Datas = SelectedType.Datas
            };
            SelectedCollection.Types.Add(newType);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteCollectionCommad { get; set; }

        private void DeleteCollection()
        {
            ConfigFile.Collections.Remove(SelectedCollection);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteElementExportTypeCommad { get; set; }

        private void DeleteElementExportType()
        {
            SelectedCollection.Types.Remove(SelectedType);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteNavisSearcherCommand { get; set; }

        private void DeleteNavisSearcher()
        {
            SelectedType.Searchers.Remove(SelectedNavisSearcher);
            ConfigFile.SaveConfig();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }

        private void DeleteNavisData()
        {
            SelectedType.Datas.Remove(SelectedNavisData);
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

            var collectionNames = ConfigFile.Collections.ToList().Select(x => x.Name).ToList();

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
            SelectedType.Name = input;
            ConfigFile.SaveConfig();
        }

        //Config methods

        public RelayCommand ClearConfigCommand { get; set; }

        private void ClearConfig()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure that you want to reset the configuration and clear all collections?", "Reset configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ConfigFile = new NdeConfig();
                ConfigFile.SaveConfig();
            }
        }

        public RelayCommand SaveCurrentConfigCommand { get; set; }

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

    }
}