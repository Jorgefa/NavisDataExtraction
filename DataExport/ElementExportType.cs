using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
{
    public class ElementExportType
    {
        public ElementExportType(string name, DataExportType searcher)
        {
            Name = name;
            Searcher = searcher;
            DataExportList = new List<DataExportType>();
        }
        public string Name { get; set; }
        public List<DataExportType> DataExportList { get; set; }
        public DataExportType Searcher { get; set; }

        public void AddDataExportType(DataExportType dataExportType)
        {
            if (dataExportType != null)
            {
                DataExportList.Add(dataExportType);
            }
        }
    }
}
