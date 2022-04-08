using Autodesk.Navisworks.Api;


namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionElement : NavisObservableItem
    {
        public NavisExtractionElement(ModelItem element, NavisExtractionType exportType)
        {
            Element = element;
            ExportType = exportType;
        }

        public ModelItem Element { get; set; }
        public NavisExtractionType ExportType { get; set; }
    }
}
