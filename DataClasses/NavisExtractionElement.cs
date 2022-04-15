using Autodesk.Navisworks.Api;
using NavisDataExtraction.Configuration;

namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionElement : NdeSelectableItem
    {
        public NavisExtractionElement(ModelItem element, NdeType exportType)
        {
            Element = element;
            ExportType = exportType;
        }

        public ModelItem Element { get; set; }
        public NdeType ExportType { get; set; }
    }
}
