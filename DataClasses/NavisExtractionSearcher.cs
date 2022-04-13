namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionSearcher : NavisObservableItem
    {
        public NavisExtractionSearcher()
        {
        }

        public NavisExtractionSearcher(NavisSearchType searchType, string navisCategoryName, string navisPropertyName)
        {
            SearchType = searchType;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }

        public NavisSearchType SearchType { get; set; }
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }
    }
    }
