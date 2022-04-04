using System.Collections.Generic;

namespace NavisDataExtraction.DataExport
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
            SearcherList = new List<NavisSearcher>();
            DataExportList = new List<NavisDataExportType>();
        }

        public ElementExportType(string name, List<NavisSearcher> searcher)
        {
            Name = name;
            SearcherList = searcher;
            DataExportList = new List<NavisDataExportType>();
        }

        public ElementExportType(string name, NavisSearcher searcher)
        {
            Name = name;
            SearcherList = new List<NavisSearcher>();
            SearcherList.Add(searcher);
            DataExportList = new List<NavisDataExportType>();
        }

        //Properties
        public string Name { get; set; }

        public List<NavisDataExportType> DataExportList { get; set; }
        public List<NavisSearcher> SearcherList { get; set; }

        //Methods
        public void AddSearcher(NavisSearcher searcher)
        {
            if (searcher != null)
            {
                SearcherList.Add(searcher);
            }
        }

        public void AddDataExportType(NavisDataExportType dataExportType)
        {
            if (dataExportType != null)
            {
                DataExportList.Add(dataExportType);
            }
        }
    }
}