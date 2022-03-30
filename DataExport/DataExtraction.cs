using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataCollector;
using NavisDataExtraction.DataExport;
using System;
using System.Collections.Generic;
using System.Data;

namespace NavisDataExtraction.DataExport
{
    public class DataExtraction
    {
        public static DataTable CreateNavisDatatable(List<ElementExportType> elementExportTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Guid", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            List<ElementExport> elements = new List<ElementExport>();

            foreach (ElementExportType elementType in elementExportTypes)
            {
                List<DataExportType> dataList = elementType.DataExportList;
                foreach (DataExportType data in dataList)
                {
                    string columnName = data.DataName;
                    Type columnType = data.DataType;
                    if (dt.Columns.Contains(columnName)) continue;
                    dt.Columns.Add(columnName, columnType);
                }

                List<ElementExport> curElements = NavisDataCollector.ElementCollectorByType(elementType);
                elements.AddRange(curElements);
            }
            foreach (ElementExport elementExport in elements)
            {
                DataRow dataRow = dt.NewRow();
                var ele = elementExport.Element;
                var properties = elementExport.ExportType.DataExportList;
                string guid = ele.InstanceGuid.ToString();
                string name = ele.DisplayName.ToString();
                dataRow["Guid"] = guid;
                dataRow["Name"] = name;

                foreach (var property in properties)
                {
                    var dataName = property.DataName;
                    var categoryName = property.NavisCategoryName;
                    var propertyName = property.NavisPropertyName;
                    var propertyValue = ele.GetParameterByName(categoryName, propertyName);
                    dataRow[dataName] = propertyValue;
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}