using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;
using NavisDataExtraction.Configuration;

namespace NavisDataExtraction.DataClasses
{
    public class NavisworksProperty : NdeSelectableItem
    {
        public PropertyCategory Category { get; set; }
        public DataProperty Property { get; set; }

    }
}