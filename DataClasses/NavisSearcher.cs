namespace NavisDataExtraction.DataClasses
{
    public class NavisSearcher : NavisData
    {
        public NavisSearcher()
        {
        }

        public NavisSearcher(NavisSearchType searchType, string navisCategoryName, string navisPropertyName)
        {
            SearchType = searchType;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }

        public NavisSearchType SearchType { get; set; }
    }
    }
