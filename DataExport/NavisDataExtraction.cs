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
        public static DataTable CreateNavisDatatable(ObservableCollection<NavisExtractionType> navisExtractionTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Guid", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("CC-X", typeof(string));
            dt.Columns.Add("CC-Y", typeof(string));
            dt.Columns.Add("CC-Z", typeof(string));


            List<NavisExtractionElement> elementExportList = new List<NavisExtractionElement>();

            foreach (NavisExtractionType type in navisExtractionTypes)
            {
                foreach (NavisExtractionData data in type.Datas)
                {
                    string columnName = data.Name;
                    Type columnType = data.Type;
                    if (dt.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    dt.Columns.Add(columnName, columnType);
                }

                elementExportList.AddRange(NavisDataCollector.ElementCollectorByType(type));
            }
            foreach (NavisExtractionElement elementExport in elementExportList)
            {
                DataRow dataRow = dt.NewRow();
                var ele = elementExport.Element;
                var properties = elementExport.ExportType.Datas;
                string guid = ele.InstanceGuid.ToString();
                string name = ele.DisplayName.ToString();
                string coordX = NavisUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.X).ToString();
                string coordY = NavisUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.Y).ToString();
                string coordZ = NavisUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.Z).ToString();
                dataRow["Guid"] = guid;
                dataRow["Name"] = name;
                dataRow["CC-X"] = coordX;
                dataRow["CC-Y"] = coordY;
                dataRow["CC-Z"] = coordZ;


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