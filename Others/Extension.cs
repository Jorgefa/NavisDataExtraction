using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.Others
{
    public static class Extension
    {
        public static void ToCSV(this DataTable dtDataTable, string strFilePath, string separator = ";")
        {

            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(separator);
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(separator))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(separator);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
         
        public static string GetParameterByName(this ModelItem item,string propertyName)
        {
            foreach (var category in item.PropertyCategories)
            {
                foreach (var property in category.Properties)
                {
                    if (property.DisplayName == propertyName)
                    {
                        return property.Value.ToString().Replace($"{property.Value.DataType.ToString()}:", "");
                    }
                }
            }
            return "empty";
        }

        public static string GetParameterByName(this ModelItem item, string categoryName, string propertyName)
        {
            foreach (var category in item.PropertyCategories)
            {
                if (category.DisplayName != categoryName) continue;
                foreach (var property in category.Properties)
                {
                    if (property.DisplayName == propertyName)
                    {
                        return property.Value.ToString().Replace($"{property.Value.DataType.ToString()}:", "");
                    }
                }
            }
            return "empty";
        }
    }
}
