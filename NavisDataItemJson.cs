using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction
{
    public class NavisDataItem
    {
        public NavisDataItem(ModelItem item)
        {
            NavisDataTool.count++;
            Guid = item.InstanceGuid.ToString();
            Name = item.DisplayName;
            Class = item.ClassDisplayName;
            PropertySets = new Dictionary<string, Dictionary<string, string>>();
            foreach (var propertySet in item.PropertyCategories)
            {
                try
                {
                    var name = propertySet.DisplayName;
                    var set = new Dictionary<string, string>();
                    foreach (var property in propertySet.Properties)
                    {
                        try
                        {
                            var propertyName = property.DisplayName;
                            var propertyValue = property.Value.ToString().Replace(
                                $"{property.Value.DataType.ToString()}:", "");
                            set.Add(propertyName, propertyValue);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    PropertySets.Add(name, set);
                }
                catch (Exception)
                {
                }
            }
        }

        public string Guid { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public Dictionary<string, Dictionary<string, string>> PropertySets { get; set; }
    }
}
