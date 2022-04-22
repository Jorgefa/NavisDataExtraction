using Autodesk.Navisworks.Api;

namespace NavisDataExtraction.Configuration
{
    public class NdeSearcher : NdeSelectableItem
    {
        public NdeSearcher()
        {
        }

        public NdeSearcher(NdeSearchConditionType searchType, string navisCategoryName, string navisPropertyName)
        {
            SearchType = searchType;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }

        public NdeSearchConditionType SearchType { get; set; }

        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }

        public SearchConditionComparison Comparison { get; set; }
        public SearchConditionOptions Options { get; set; }
    }
}
