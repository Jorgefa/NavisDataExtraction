using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;

namespace NavisDataExtraction.DataClasses
{
    public class NavisworksProperty : NavisObservableItem
    {
        public PropertyCategory Category { get; set; }
        public DataProperty Property { get; set; }

    }
}