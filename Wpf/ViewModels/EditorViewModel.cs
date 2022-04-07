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
            //ElementExportTypes = new ObservableCollection<ElementExportType>(ConfigFile.CurrentElementExportTypes);
            AddNewElementExportTypeCommand = new RelayCommand(AddNewExportType);
            DeleteElementExportTypeCommad = new RelayCommand(DeleteElementExportType);
            AddNewNavisSearcherCommand = new RelayCommand(AddNewNavisNavisSearcher);
            DeleteNavisSearcherCommand = new RelayCommand(DeleteNavisSearcher);
            AddNewElementExportTypeCommand = new RelayCommand(AddNewNavisDataExport);
            DeleteNavisDataExportCommand = new RelayCommand(DeleteNavisDataExport);
            NewNavisSearcher = new NavisSearcher();
            NewElementExportType = new ElementExportType();
            NewNavisDataExport = new NavisDataExport();
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
                ConfigFile.ToFile();
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

        private NavisSearcher _newNavisSearcher;

        public NavisSearcher NewNavisSearcher
        {
            get { return _newNavisSearcher; }
            set
            {
                _newNavisSearcher = value;
                OnPropertyChanged();
            }
        }

        private NavisDataExport _newNavisDataExport;

        public NavisDataExport NewNavisDataExport
        {
            get { return _newNavisDataExport; }
            set
            {
                _newNavisDataExport = value;
                OnPropertyChanged();
            }
        }

        //Methods

        public RelayCommand AddNewElementExportTypeCommand { get; set; }

        private void AddNewExportType()
        {
            if (string.IsNullOrEmpty(NewElementExportType.Name))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            ConfigFile.CurrentElementExportTypes.Add(NewElementExportType);
            ConfigFile.ToFile();
            NewElementExportType = new ElementExportType();
        }

        public RelayCommand DeleteElementExportTypeCommad { get; set; }

        private void DeleteElementExportType()
        {
            ConfigFile.CurrentElementExportTypes.Remove(SelectedElementExportType);
            ConfigFile.ToFile();
        }

        public RelayCommand AddNewNavisSearcherCommand { get; set; }

        private void AddNewNavisNavisSearcher()
        {
            if (string.IsNullOrEmpty(NewNavisSearcher.NavisCategoryName) || string.IsNullOrEmpty(NewNavisSearcher.NavisPropertyName))
            {
                MessageBox.Show("Please enter a valid category and property names");
                return;
            }
            SelectedElementExportType.AddSearcher(NewNavisSearcher);
            ConfigFile.ToFile();
            NewNavisSearcher = new NavisSearcher();
        }

        public RelayCommand DeleteNavisSearcherCommand { get; set; }

        private void DeleteNavisSearcher()
        {
            SelectedElementExportType.SearcherList.Remove(SelectedNavisSearcher);
            ConfigFile.ToFile();
        }

        public RelayCommand AddNewNavisDataExportCommand { get; set; }

        private void AddNewNavisDataExport()
        {
            if (string.IsNullOrEmpty(NewNavisDataExport.DataName) || string.IsNullOrEmpty(NewNavisDataExport.NavisCategoryName) || string.IsNullOrEmpty(NewNavisDataExport.NavisPropertyName))
            {
                MessageBox.Show("Please enter a valid data, category and property names");
                return;
            }
            SelectedElementExportType.DataExportList.Add(NewNavisDataExport);
            ConfigFile.ToFile();
            NewNavisDataExport = new NavisDataExport();
        }

        public RelayCommand DeleteNavisDataExportCommand { get; set; }

        private void DeleteNavisDataExport()
        {
            SelectedElementExportType.DataExportList.Remove(SelectedNavisDataExport);
            ConfigFile.ToFile();
        }
    }
}