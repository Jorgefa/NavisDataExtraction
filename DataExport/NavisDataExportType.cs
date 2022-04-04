using System;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataExportType : NavisDataType
    {
        //Constructors
        public NavisDataExportType()
        {
        }

        public NavisDataExportType(string dataName, string navisCategoryName, string navisPropertyName)
        {
            DataName = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public NavisDataExportType(string dataName, string navisCategoryName, string navisPropertyName, string navisPropertyValue)
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
