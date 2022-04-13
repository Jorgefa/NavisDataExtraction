using System.Collections.ObjectModel;
using System.Linq;

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

        //Methods
        public string SearchersValidation()
        {
            if (Searchers != null)
            {
                foreach (var searcher in Searchers)
                {
                    if (string.IsNullOrEmpty(searcher.NavisCategoryName) ||
                        string.IsNullOrEmpty(searcher.NavisPropertyName))
                    {
                        return "blankValue";
                    }
                }
            }
            return "ok";
        }

        public string DatasValidation()
        {
            if (Datas != null)
            {
                var dataNames = Datas.ToList().Select(x => x.Name).ToList();

                if (dataNames.Count != dataNames.Distinct().Count())
                {
                    return "duplicates";
                }

                foreach (var data in Datas)
                {
                    if (string.IsNullOrEmpty(data.Name) ||
                        string.IsNullOrEmpty(data.NavisCategoryName) ||
                        string.IsNullOrEmpty(data.NavisPropertyName))
                    {
                        return "blankValue";
                    }
                }
            }
            return "ok";
        }
    }
}