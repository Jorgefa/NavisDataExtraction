using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.Commands;
using PM.Navisworks.DataExtraction.Extensions;
using PM.Navisworks.DataExtraction.Models.DataTransfer;
using PM.Navisworks.DataExtraction.Models.Navisworks;
using PM.Navisworks.DataExtraction.Utilities;
using Condition = PM.Navisworks.DataExtraction.Models.DataTransfer.Condition;

namespace PM.Navisworks.DataExtraction.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly Document _document;
        private readonly Dispatcher _dispatcher;

        public MainWindowViewModel(Document document)
        {
            _document = document;
            _document.CurrentSelection.Changed += (sender, args) => UpdateCommandVisibility();
            _dispatcher = Dispatcher.CurrentDispatcher;
            Searchers = new ObservableCollection<Searcher>();

            AddNewSearcherCommand = new DelegateCommand(AddNewSearch);
            AddNewConditionCommand = new DelegateCommand(AddNewCondition);
            AddNewPairCommand = new DelegateCommand(AddNewPair);
            SelectInNavisworksCommand = new DelegateCommand(SelectInNavisworks, CanSelectNavisworks);
            RefreshCategoriesCommand = new DelegateCommand(GetCategories, CanGetCategories);
            ImportConfigCommand = new DelegateCommand(() =>
                Searchers = new ObservableCollection<Searcher>(Configuration.Import()));
            ExportConfigCommand = new DelegateCommand(() => Configuration.Export(Searchers), () => Searchers.Any());
            DeleteSearcherCommand = new DelegateCommand(DeleteSearch);
            DeleteConditionCommand = new DelegateCommand(DeleteCondition);
            DeletePairCommand = new DelegateCommand(DeletePair);
            ExportSearchCsvCommand = new DelegateCommand(ExportSearchCsv);
            ExportSearchJsonCommand = new DelegateCommand(ExportSearchJson);
            ExportSearchAllCsvCommand =
                new DelegateCommand(() => Searchers.ExportCsv(_document), () => Searchers.Any());
            ExportSearchAllJsonCommand =
                new DelegateCommand(() => Searchers.ExportJson(_document), () => Searchers.Any());

            GetCategories();
        }

        private void GetCategories()
        {
            ProgressBarVisibility = true;
            ProgressBarMessage = "Loading available Navisworks Properties ....";

            var task = new Task(() =>
            {
                Categories = new ObservableCollection<Category>(_document.GetModelCategories());
                _dispatcher.Invoke(() =>
                {
                    ProgressBarVisibility = false;
                    ProgressBarMessage = string.Empty;
                });
            });
            task.Start();
        }

        private bool CanGetCategories()
        {
            return !_document.CurrentSelection.IsEmpty;
        }

        private void UpdateValues()
        {
            RaisePropertyChanged(nameof(Comparers));
            RaisePropertyChanged(nameof(BoolVisibility));
            RaisePropertyChanged(nameof(StringVisibility));
            RaisePropertyChanged(nameof(IntegerVisibility));
            RaisePropertyChanged(nameof(DoubleVisibility));
            RaisePropertyChanged(nameof(DateTimeVisibility));

            UpdateCommandVisibility();

            if (SelectedSearcher == null || !SelectedSearcher.Conditions.Any()) return;
            foreach (var selectedSearcherCondition in SelectedSearcher?.Conditions)
            {
                selectedSearcherCondition?.SetDisplayName(selectedSearcherCondition.Property?.ValueType);
            }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set => SetProperty(ref _progressBarVisibility, value);
        }

        private string _progressBarMessage;

        public string ProgressBarMessage
        {
            get => _progressBarMessage;
            private set => SetProperty(ref _progressBarMessage, value);
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            private set => SetProperty(ref _categories, value);
        }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                if(value == null) return;
                SelectedCondition.Category = value;
            }
        }

        private Property _selectedProperty;

        public Property SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                SetProperty(ref _selectedProperty, value);
                if(value == null) return;
                SelectedCondition.Property = value;
            }
        }

        private Category _selectedPairCategory;

        public Category SelectedPairCategory
        {
            get => _selectedPairCategory;
            set
            {
                SetProperty(ref _selectedPairCategory, value);
                if (value == null) return;
                SelectedPair.Category = value;
            }
        }

        private Property _selectedPairProperty;

        public Property SelectedPairProperty
        {
            get => _selectedPairProperty;
            set
            {
                SetProperty(ref _selectedPairProperty, value);
                if (value == null) return;
                SelectedPair.Property = value;
            }
        }

        private ObservableCollection<Searcher> _searchers;

        public ObservableCollection<Searcher> Searchers
        {
            get => _searchers;
            private set
            {
                SetProperty(ref _searchers, value);
                UpdateCommandVisibility();
            }
        }

        private Searcher _selectedSearcher;

        public Searcher SelectedSearcher
        {
            get => _selectedSearcher;
            set
            {
                SetProperty(ref _selectedSearcher, value);
                UpdateCommandVisibility();
            }
        }

        private Condition _selectedCondition;

        public Condition SelectedCondition
        {
            get => _selectedCondition;
            set
            {
                SetProperty(ref _selectedCondition, value);
                if (value == null) return;
                UpdateValues();
                value.PropertyChanged += (sender, args) => UpdateValues();
            }
        }

        private CategoryPropertyPair _selectedPair;

        public CategoryPropertyPair SelectedPair
        {
            get => _selectedPair;
            set => SetProperty(ref _selectedPair, value);
        }

        public ObservableCollection<ConditionComparer> Comparers
        {
            get
            {
                if (SelectedCondition?.Property?.ValueType == typeof(bool))
                {
                    return new ObservableCollection<ConditionComparer>
                    {
                        ConditionComparer.Exists,
                        ConditionComparer.Equal,
                        ConditionComparer.NotEqual,
                    };
                }

                if (SelectedCondition?.Property?.ValueType == typeof(string))
                {
                    return new ObservableCollection<ConditionComparer>
                    {
                        ConditionComparer.Exists,
                        ConditionComparer.Equal,
                        ConditionComparer.NotEqual,
                        ConditionComparer.StringContains,
                    };
                }

                if (SelectedCondition?.Property?.ValueType == typeof(int) ||
                    SelectedCondition?.Property?.ValueType == typeof(double) ||
                    SelectedCondition?.Property?.ValueType == typeof(DateTime))
                {
                    return new ObservableCollection<ConditionComparer>
                    {
                        ConditionComparer.Exists,
                        ConditionComparer.Equal,
                        ConditionComparer.NotEqual,
                        ConditionComparer.LessThan,
                        ConditionComparer.GreaterThan,
                        ConditionComparer.LessThanOrEqual,
                        ConditionComparer.GreaterThanOrEqual,
                    };
                }

                return new ObservableCollection<ConditionComparer>();
            }
        }

        public Visibility BoolVisibility => SelectedCondition?.Property?.ValueType == typeof(bool)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility StringVisibility => SelectedCondition?.Property?.ValueType == typeof(string)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IntegerVisibility => SelectedCondition?.Property?.ValueType == typeof(int)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility DoubleVisibility => SelectedCondition?.Property?.ValueType == typeof(double)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility DateTimeVisibility => SelectedCondition?.Property?.ValueType == typeof(DateTime)
            ? Visibility.Visible
            : Visibility.Collapsed;

        private void UpdateCommandVisibility()
        {
            SelectInNavisworksCommand?.RaiseCanExecuteChanged();
            RefreshCategoriesCommand?.RaiseCanExecuteChanged();
            ExportConfigCommand?.RaiseCanExecuteChanged();
            ExportSearchAllCsvCommand?.RaiseCanExecuteChanged();
            ExportSearchAllJsonCommand?.RaiseCanExecuteChanged();
        }

        public DelegateCommand AddNewSearcherCommand { get; }
        public DelegateCommand AddNewConditionCommand { get; }
        public DelegateCommand AddNewPairCommand { get; }
        public DelegateCommand SelectInNavisworksCommand { get; }
        public DelegateCommand RefreshCategoriesCommand { get; }
        public DelegateCommand ImportConfigCommand { get; }
        public DelegateCommand ExportConfigCommand { get; }
        public DelegateCommand DeleteSearcherCommand { get; }
        public DelegateCommand DeleteConditionCommand { get; }
        public DelegateCommand DeletePairCommand { get; }
        public DelegateCommand ExportSearchCsvCommand { get; }
        public DelegateCommand ExportSearchJsonCommand { get; }
        public DelegateCommand ExportSearchAllCsvCommand { get; }
        public DelegateCommand ExportSearchAllJsonCommand { get; }

        private void AddNewSearch()
        {
            Searchers.Add(new Searcher
            {
                Name = "New Searcher"
            });
        }

        private void DeleteSearch()
        {
            if (SelectedSearcher == null) return;
            Searchers.Remove(SelectedSearcher);
        }

        private void AddNewCondition()
        {
            if (SelectedSearcher == null) return;

            SelectedSearcher.Conditions.Add(new Condition
            {
                Category = Categories.Any() ? Categories.First() : null
            });
            SelectedCondition = SelectedSearcher.Conditions.Last();
            UpdateValues();
        }

        private void DeleteCondition()
        {
            if (SelectedSearcher == null) return;
            if (SelectedCondition == null) return;
            SelectedSearcher?.Conditions?.Remove(SelectedCondition);
        }

        private void AddNewPair()
        {
            if (SelectedSearcher == null) return;

            SelectedSearcher.Pairs.Add(new CategoryPropertyPair
            {
                Category = Categories.Any() ? Categories.First() : null
            });
        }

        private void DeletePair()
        {
            if (SelectedSearcher == null) return;
            if (SelectedPair == null) return;
            SelectedSearcher?.Pairs?.Remove(SelectedPair);
        }

        private void SelectInNavisworks()
        {
            if (SelectedSearcher == null) return;

            var search = NavisworksSearcher.FromDto(SelectedSearcher);

            _dispatcher.Invoke(() =>
            {
                ProgressBarVisibility = true;
                ProgressBarMessage = "Loading available Navisworks Properties ....";
            });

            var elements = search.FindAll(_document, false);
            _document.CurrentSelection.Clear();
            _document.CurrentSelection.CopyFrom(elements);

            _dispatcher.Invoke(() =>
            {
                ProgressBarVisibility = false;
                ProgressBarMessage = string.Empty;
            });
        }

        private bool CanSelectNavisworks()
        {
            if (SelectedSearcher == null) return false;
            if (!SelectedSearcher.Conditions.Any()) return false;
            if (SelectedSearcher.Conditions.Any(c => c.Category == null)) return false;

            return true;
        }

        private void ExportSearchCsv()
        {
            if (SelectedSearcher == null) return;
            var dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = SelectedSearcher.Name
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                SelectedSearcher.ExportCsv(_document, dialog.FileName);
        }

        private void ExportSearchJson()
        {
            if (SelectedSearcher == null) return;
            var dialog = new SaveFileDialog
            {
                Filter = "Json Files (*.json)|*.json",
                FileName = SelectedSearcher.Name
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                SelectedSearcher.ExportJson(_document, dialog.FileName);
        }
    }
}