using NavisDataExtraction.DataClasses;
using NavisDataExtraction.Others;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataExtraction
    {
        public static DataTable CreateNavisDatatable(ObservableCollection<ElementExportType> elementExportTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Guid", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            List<ElementExport> elementExportList = new List<ElementExport>();

            foreach (ElementExportType elementType in elementExportTypes)
            {
                foreach (NavisDataExport data in elementType.DataExportList)
                {
                    string columnName = data.DataName;
                    Type columnType = data.DataType;
                    if (dt.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    dt.Columns.Add(columnName, columnType);
                }

                elementExportList.AddRange(NavisDataCollector.ElementCollectorByType(elementType));
            }
            foreach (ElementExport elementExport in elementExportList)
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