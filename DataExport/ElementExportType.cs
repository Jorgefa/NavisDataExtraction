using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataExport
{
    public class ElementExportType
    {
        //Constructors
        public ElementExportType(string name, DataExportType searcher)
        {
            Name = name;
            Searcher = searcher;
            DataExportList = new List<DataExportType>();
        }

        //Properties
        public string Name { get; set; }
        public List<DataExportType> DataExportList { get; set; }
        public DataExportType Searcher { get; set; }

        //Methods
        public void AddDataExportType(DataExportType dataExportType)
        {
            if (dataExportType != null)
            {
                DataExportList.Add(dataExportType);
            }
        }
    }
}
