using System.Collections.Generic;

namespace PM.Navisworks.DataExtraction.Models.Data
{
    public class FileData
    {
        public string FileName { get; set; }
        public string SearcherName { get; set; }
        public List<ElementData> ElementsData { get; set; }
    }
}