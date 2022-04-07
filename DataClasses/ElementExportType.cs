using System.Collections.ObjectModel;

namespace NavisDataExtraction.DataClasses
{
    public class ElementExportType
    {
        //Constructors
        public ElementExportType()
        {
        }

        public ElementExportType(string name)
        {
            Name = name;
            SearcherList = new ObservableCollection<NavisSearcher>();
            DataExportList = new ObservableCollection<NavisDataExport>();
        }

        public ElementExportType(string name, ObservableCollection<NavisSearcher> searcher)
        {
            Name = name;
            SearcherList = searcher;
            DataExportList = new ObservableCollection<NavisDataExport>();
        }

        public ElementExportType(string name, NavisSearcher searcher)
        {
            Name = name;
            SearcherList = new ObservableCollection<NavisSearcher>();
            SearcherList.Add(searcher);
            DataExportList = new ObservableCollection<NavisDataExport>();
        }

        //Properties
        public string Name { get; set; }

        public ObservableCollection<NavisDataExport> DataExportList { get; set; }
        public ObservableCollection<NavisSearcher> SearcherList { get; set; }

        //Methods
        public void AddSearcher(NavisSearcher searcher)
        {
            if (searcher != null)
            {
                SearcherList.Add(searcher);

            }
        }

        public void AddDataExportType(NavisDataExport dataExportType)
        {
            if (dataExportType != null)
            {
                DataExportList.Add(dataExportType);
            }
        }
    }
}