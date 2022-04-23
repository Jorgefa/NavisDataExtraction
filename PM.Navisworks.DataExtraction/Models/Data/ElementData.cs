using System;
using System.Collections.Generic;

namespace PM.Navisworks.DataExtraction.Models.Data
{
    public class ElementData
    {
        public string ElementName { get; set; }
        public Guid ElementGuid { get; set; }
        public List<DataPair> Properties { get; set; }
    }
}