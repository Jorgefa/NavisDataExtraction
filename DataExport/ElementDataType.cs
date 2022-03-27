using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
{
    public class ElementExportType
    {
        public ElementExportType()
        {
            DataExportList = null;
        }
        public List<DataExportType> DataExportList { get; set; }

        public void AddDataExportType(DataExportType dataExportType)
        {
            if (dataExportType != null)
            {
                DataExportList.Add(dataExportType);
            }
        }
    }
}
