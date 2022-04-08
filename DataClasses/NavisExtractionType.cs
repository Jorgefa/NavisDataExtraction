using System.Collections.ObjectModel;

namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionType
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
        public string Name { get; set; }

        public ObservableCollection<NavisExtractionData> Datas { get; set; }
        public ObservableCollection<NavisExtractionSearcher> Searchers { get; set; }

    }
}