using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.Commands;
using PM.Navisworks.DataExtraction.Models;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly Document _document;
        private readonly Dispatcher _dispatcher;

        public MainWindowViewModel(Document document)
        {
            _document = document;
            _dispatcher = Dispatcher.CurrentDispatcher;
            Searchers = new ObservableCollection<SearcherDto>();

            AddNewSearcherCommand = new DelegateCommand(AddNewSearcher);
            AddNewConditionCommand = new DelegateCommand(AddNewCondition);
            AddNewPairCommand = new DelegateCommand(AddNewPair);
            SelectInNavisworksCommand = new DelegateCommand(SelectInNavisworks);
            RefreshCategoriesCommand = new DelegateCommand(GetCategories);

            GetCategories();
        }

        private void GetCategories()
        {
            ProgressBarVisibility = true;
            ProgressBarMessage = "Loading available Navisworks Properties ....";

            var task = new Task(() =>
            {
                Categories = new ObservableCollection<Category>(NavisworksCollector.GetModelCategories(_document));
                _dispatcher.Invoke(() =>
                {
                    ProgressBarVisibility = false;
                    ProgressBarMessage = string.Empty;
                });
            });
            task.Start();
        }

        private void UpdateValues()
        {
            RaisePropertyChanged(nameof(Comparers));
            RaisePropertyChanged(nameof(BoolVisibility));
            RaisePropertyChanged(nameof(StringVisibility));
            RaisePropertyChanged(nameof(IntegerVisibility));
            RaisePropertyChanged(nameof(DoubleVisibility));
            RaisePropertyChanged(nameof(DateTimeVisibility));

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
            set => SetProperty(ref _progressBarMessage, value);
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ObservableCollection<SearcherDto> _searchers;

        public ObservableCollection<SearcherDto> Searchers
        {
            get => _searchers;
            set => SetProperty(ref _searchers, value);
        }

        private SearcherDto _selectedSearcher;

        public SearcherDto SelectedSearcher
        {
            get => _selectedSearcher;
            set => SetProperty(ref _selectedSearcher, value);
        }

        private ConditionDto _selectedCondition;

        public ConditionDto SelectedCondition
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

        public DelegateCommand AddNewSearcherCommand { get; set; }
        public DelegateCommand AddNewConditionCommand { get; set; }
        public DelegateCommand AddNewPairCommand { get; set; }
        public DelegateCommand SelectInNavisworksCommand { get; set; }
        public DelegateCommand RefreshCategoriesCommand { get; set; }

        private void AddNewSearcher()
        {
            Searchers.Add(new SearcherDto
            {
                Name = "New Searcher"
            });
        }

        private void AddNewCondition()
        {
            if (SelectedSearcher == null) return;

            SelectedSearcher.Conditions.Add(new ConditionDto
            {
                Category = Categories.Any() ? Categories.First() : null
            });
            SelectedCondition = SelectedSearcher.Conditions.Last();
            UpdateValues();
        }

        private void AddNewPair()
        {
            if (SelectedSearcher == null) return;

            SelectedSearcher.Pairs.Add(new CategoryPropertyPair
            {
                Category = Categories.Any() ? Categories.First() : null
            });
        }

        private void SelectInNavisworks()
        {
            if (SelectedSearcher == null) return;

            var search = Searcher.FromDto(SelectedSearcher);

            ProgressBarVisibility = true;
            ProgressBarMessage = "Loading available Navisworks Properties ....";

            var elements = search.FindAll(_document, false);
            _document.CurrentSelection.Clear();
            _document.CurrentSelection.CopyFrom(elements);

            ProgressBarVisibility = false;
            ProgressBarMessage = string.Empty;
        }
    }
}