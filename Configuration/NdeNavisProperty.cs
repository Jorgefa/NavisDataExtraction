
using Autodesk.Navisworks.Api;


namespace NavisDataExtraction.Configuration
{
    public class NdeNavisProperty : NdeSelectableItem
    {
        public PropertyCategory Category { get; set; }
        public DataProperty Property { get; set; }

    }
}