using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
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
