using Autodesk.Navisworks.Api;


namespace NavisDataExtraction.DataClasses
{
    public class ElementExport
    {
        public ElementExport(ModelItem element, ElementExportType exportType)
        {
            Element = element;
            ExportType = exportType;
        }

        public ModelItem Element { get; set; }
        public ElementExportType ExportType { get; set; }
    }
}
