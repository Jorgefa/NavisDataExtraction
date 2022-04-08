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
        public static DataTable CreateNavisDatatable(ObservableCollection<NavisExtractionType> elementExportTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Guid", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            List<NavisExtractionElement> elementExportList = new List<NavisExtractionElement>();

            foreach (NavisExtractionType elementType in elementExportTypes)
            {
                foreach (NavisExtractionData data in elementType.Datas)
                {
                    string columnName = data.Name;
                    Type columnType = data.Type;
                    if (dt.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    dt.Columns.Add(columnName, columnType);
                }

                elementExportList.AddRange(NavisDataCollector.ElementCollectorByType(elementType));
            }
            foreach (NavisExtractionElement elementExport in elementExportList)
            {
                DataRow dataRow = dt.NewRow();
                var ele = elementExport.Element;
                var properties = elementExport.ExportType.Datas;
                string guid = ele.InstanceGuid.ToString();
                string name = ele.DisplayName.ToString();
                dataRow["Guid"] = guid;
                dataRow["Name"] = name;

                foreach (var property in properties)
                {
                    var dataName = property.Name;
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