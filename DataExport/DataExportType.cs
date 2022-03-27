namespace NavisDataExtraction.DataExport
{
    public class DataExportType
    {
        public DataExportType(string dataName, NavisDataType navisworkDataType)
        {
            DataName = dataName;
            NavisDataElement = navisworkDataType;
        }
        public string DataName { get; set; }
        public NavisDataType NavisDataElement { get; set; }
    }
}
