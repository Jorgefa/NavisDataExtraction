namespace NavisDataExtraction.DataExport
{
    public class NavisDataType
    {
        public NavisDataType(string navisCategoryName, string navisPropertyName)
        {
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
    }
}
