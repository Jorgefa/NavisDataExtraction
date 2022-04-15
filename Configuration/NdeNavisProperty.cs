using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;
using NavisDataExtraction.Configuration;

namespace NavisDataExtraction.Configuration
{
    public class NdeNavisProperty : NdeSelectableItem
    {
        public PropertyCategory Category { get; set; }
        public DataProperty Property { get; set; }

    }
}