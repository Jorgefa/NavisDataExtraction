using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataType
    {
        //Constructors
        public NavisDataType()
        {
        }
        public NavisDataType(string navisCategoryName, string navisPropertyName)
        {
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public NavisDataType(string navisCategoryName, string navisPropertyName, string navisPropertyValue)
        {
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
            NavisPropertyValue = navisPropertyValue;
        }

        //Properties
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }
    }
}
