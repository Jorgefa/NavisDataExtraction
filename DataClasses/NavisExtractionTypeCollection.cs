using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionTypeCollection
    {
        //Constructors
        NavisExtractionTypeCollection()
        {

        }

        public NavisExtractionTypeCollection(string name)
        {
            Name = name;
        }
        NavisExtractionTypeCollection(string name, ObservableCollection<NavisExtractionType> types)
        {
            Name = name;
            Types = types;
        }

        //Properties
        public ObservableCollection<NavisExtractionType> Types { get; set; }
        public string Name { get; set; }
    }
}
