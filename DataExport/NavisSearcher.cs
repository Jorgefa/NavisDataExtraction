using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
{
    public class NavisSearcher : NavisDataType
    {
        public NavisSearcher()
        {
        }

        public NavisSearcher(string searchType, string navisCategoryName, string navisPropertyName)
        {
            SearchType = searchType;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }

        public string SearchType { get; set; }
    }
}
