using System.Collections.ObjectModel;

namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionType : NavisObservableItem
    {
        //Constructors
        public NavisExtractionType()
        {
            Searchers = new ObservableCollection<NavisExtractionSearcher>();
            Datas = new ObservableCollection<NavisExtractionData>();
        }

        public NavisExtractionType(string name)
        {
            Name = name;
            Searchers = new ObservableCollection<NavisExtractionSearcher>();
            Datas = new ObservableCollection<NavisExtractionData>();
        }

        //Properties
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NavisExtractionData> _datas;

        public ObservableCollection<NavisExtractionData> Datas
        {
            get { return _datas; }
            set
            {
                _datas = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NavisExtractionSearcher> _searchers;

        public ObservableCollection<NavisExtractionSearcher> Searchers
        {
            get { return _searchers; }
            set
            {
                _searchers = value;
                OnPropertyChanged();
            }
        }

        //public ObservableCollection<NavisExtractionData> Datas { get; set; }
        //public ObservableCollection<NavisExtractionSearcher> Searchers { get; set; }
    }
}