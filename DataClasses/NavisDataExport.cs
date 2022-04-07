using System;

namespace NavisDataExtraction.DataClasses
{
    public class NavisDataExport : NavisData
    {
        //Constructors
        public NavisDataExport()
        {
        }

        public NavisDataExport(string dataName, string navisCategoryName, string navisPropertyName)
        {
            DataName = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public NavisDataExport(string dataName, string navisCategoryName, string navisPropertyName, string navisPropertyValue)
        {
            DataName = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
            NavisPropertyValue = navisPropertyValue;
        }

        //Properties
        public string DataName { get; set; }    
        public Type DataType { get; set; } = typeof(string);
    }
}
