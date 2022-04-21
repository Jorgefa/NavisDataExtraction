using System;

namespace NavisDataExtraction.Configuration
{
    public class NdeData : NdeSelectableItem
    {
        //Constructors
        public NdeData()
        {
        }

        public NdeData(string name, string navisCategoryName, string navisPropertyName)
        {
            Name = name;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public NdeData(string dataName, string navisCategoryName, string navisPropertyName, string navisPropertyValue)
        {
            Name = dataName;
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
            NavisPropertyValue = navisPropertyValue;
        }

        //Properties
        public string Name { get; set; }    
        public Type Type { get; set; } = typeof(string);
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }

    }
}
