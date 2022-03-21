﻿using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction
{
    public class NavisDataItemTable
    {
        public static DataTable CreateNavisDatatable(List<ModelItem> elements, List<string> properties)
        {
            var propertyList = new List<string>();
            propertyList.Add("Guid");
            propertyList.Add("Name");
            propertyList.AddRange(properties);
            propertyList.Distinct().ToList();

            var table = new DataTable();
            foreach (var property in propertyList)
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

                foreach (var property in propertyList)
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
