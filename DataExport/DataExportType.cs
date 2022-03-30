using System;

namespace NavisDataExtraction.DataExport
{
    public class DataExportType
    {
        //Constructors
        public DataExportType()
        {
        }
        public DataExportType(string dataName, string navisCategoryName, string navisPropertyName)
        {
            DataName = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public DataExportType(string dataName, string navisCategoryName, string navisPropertyName, string navisPropertyValue)
        {
            DataName = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
            NavisPropertyValue = navisPropertyValue;
        }

        //Properties
        public string DataName { get; set; }
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }
        public Type DataType { get; set; } = typeof(string);
    }
}
