using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction
{
    public class DataExtraction
    {
        public static DataTable CreateNavisDatatable(List<ModelItem> elements, List<string> customProperties)
        {

            var table = new DataTable();
            table.Columns.Add("Guid", typeof(string));
            table.Columns.Add("Name", typeof(string));
            foreach (var property in customProperties)
            {
                table.Columns.Add(property, typeof(string));
            }

            foreach (var element in elements)
            {
                var dataRow = table.NewRow();

                var guid = element.InstanceGuid.ToString();
                var name = element.DisplayName.ToString();
                dataRow["Guid"] = guid;
                dataRow["Name"] = name;

                foreach (var property in customProperties)
                {
                    var propertyValue = element.GetParameterByName(property);
                    dataRow[property] = propertyValue;
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }
    }
}
