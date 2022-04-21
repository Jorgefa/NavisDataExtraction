namespace NavisDataExtraction.Configuration
{
    public class NdeSearcher : NdeSelectableItem
    {
        public NdeSearcher()
        {
        }

        public NdeSearcher(SearchConditionType searchType, string navisCategoryName, string navisPropertyName)
        {
            SearchType = searchType;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }

        public SearchConditionType SearchType { get; set; }
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }
    }
}
