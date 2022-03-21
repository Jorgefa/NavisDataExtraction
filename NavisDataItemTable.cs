using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction
{
    class NavisDataItemTable
    {
        public DataTable CreateNavisDatatable(List<ModelItem> elements, List<string> propertyList)
        {
            var table = new DataTable();
            foreach (var property in propertyList)
            {
                table.Columns.Add(property, typeof(string));
            }

            foreach (var element in elements)
            {
                var guid = element.InstanceGuid.ToString();
                var name = element.DisplayName.ToString();
                var uniclassSs = element.GetParameterByName("UniclassSs");
                //var moduleNumber = element.GetParameterByName("ModuleNumber");
                //var lineNumber = element.GetParameterByName("LineNumber");

                table.Rows.Add(guid, name, uniclassSs);
            }

            return table;
        }
    }
}
