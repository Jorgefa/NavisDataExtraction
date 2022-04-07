using NavisDataExtraction.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NavisDataExtraction.DataClasses
{
    public class NavisData : INotifyPropertyChanged
    {
        //Constructors
        public NavisData()
        {
        }
        public NavisData(string navisCategoryName, string navisPropertyName)
        {
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
        }
        public NavisData(string navisCategoryName, string navisPropertyName, string navisPropertyValue)
        {
            NavisCategoryName = navisCategoryName;
            NavisPropertyName = navisPropertyName;
            NavisPropertyValue = navisPropertyValue;
        }

        //Properties
        public string NavisCategoryName { get; set; }
        public string NavisPropertyName { get; set; }
        public string NavisPropertyValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
