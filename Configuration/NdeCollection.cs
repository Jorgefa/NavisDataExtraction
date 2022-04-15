using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using NavisDataExtraction.NavisUtils;
using NavisDataExtraction.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace NavisDataExtraction.Configuration
{
    public class NdeCollection : NdeSelectableItem
    {
        //Constructors
        public NdeCollection()
        {
        }

        public NdeCollection(string name)
        {
            Name = name;
        }

        public NdeCollection(string name, ObservableCollection<NdeType> types)
        {
            Name = name;
            Types = types;
        }

        //Properties
        private ObservableCollection<NdeType> _types;

        public ObservableCollection<NdeType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        //Methods
        public string TypesValidation()
        {
            if (Types == null)
            {
                return null;
            }

            var typeNames = Types.ToList().Select(x => x.Name).ToList();

            if (typeNames.Count != typeNames.Distinct().Count())
            {
                return "duplicates";
            }
            return "ok";
        }

        public ObservableCollection<ModelItem> SearchModelItems()
        {
            if (Types == null)
            {
                return null;
            }
            var modelItems = new List<ModelItem>();
            foreach (var type in Types)
            {
                var modelItemGroup = type.SearchModelItems();
                modelItems.AddRange(modelItemGroup);
            }
            return new ObservableCollection<ModelItem>(modelItems);
        }

        public ObservableCollection<NdeModelItemGroup> SearchModelItemsGroups()
        {
            var modelItemsGRoups = new ObservableCollection<NdeModelItemGroup>();
            foreach (var type in Types)
            {
                modelItemsGRoups.Add(new NdeModelItemGroup { ModelItemCollection = SearchModelItems(), Type = type, Collection = this });
            }
            return modelItemsGRoups;
        }

        public DataTable GetDataTable()
        {
            //var dataNames = group.Type.Datas.ToList().Select(x => x.Name).ToList();

            // Get ModelItemsGroups
            var modelItems = SearchModelItems();
            var datas = new List<NdeData>();
            // Create table
            DataTable dt = new DataTable();

            // Create default columns
            dt.Columns.Add("object", typeof(ModelItem));
            dt.Columns.Add("Guid", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("CC-X", typeof(string));
            dt.Columns.Add("CC-Y", typeof(string));
            dt.Columns.Add("CC-Z", typeof(string));

            // Create custom data colums
            foreach (var type in Types)
            {
                foreach (var data in type.Datas)
                {
                    string columnName = data.Name;
                    Type columnType = data.Type;
                    if (dt.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    datas.Add(data);
                    dt.Columns.Add(columnName, columnType);
                }
            }

            // Create rows
            foreach (var modelItem in modelItems)
            {
                DataRow dataRow = dt.NewRow();
                var ele = modelItem;
                string guid = ele.InstanceGuid.ToString();
                string name = ele.DisplayName.ToString();
                string coordX = NdeUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.X).ToString();
                string coordY = NdeUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.Y).ToString();
                string coordZ = NdeUnits.ConvertUnitsToMeters((float)ele.BoundingBox().Center.Z).ToString();
                dataRow["Object"] = modelItem;
                dataRow["Guid"] = guid;
                dataRow["Name"] = name;
                dataRow["CC-X"] = coordX;
                dataRow["CC-Y"] = coordY;
                dataRow["CC-Z"] = coordZ;

                foreach (var data in datas)
                {
                    var dataName = data.Name;
                    var categoryName = data.NavisCategoryName;
                    var propertyName = data.NavisPropertyName;
                    var propertyValue = modelItem.GetParameterByName(categoryName, propertyName);
                    dataRow[dataName] = propertyValue;
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public void AddDataToNaviswork(ObservableCollection<NdeModelItemGroup> modelItemsGroups)
        {
            // current document (.NET)
            Document doc = Application.ActiveDocument;
            // current document (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            // new category name and displayName
            string catName = "PMG";
            string catDisplayName = "PMG_InternalName";

            // override
            bool overrideProps = false;

            if (modelItemsGroups == null)
            {
                return;
            }
            foreach (var group in modelItemsGroups)
            {
                if (group.Type == null || group.ModelItemCollection == null)
                {
                    return;
                }
                foreach (var item in group.ModelItemCollection)
                {
                    // check if the element already has the category
                    var itemCategory = item.PropertyCategories.FindCategoryByDisplayName(catDisplayName);
                    if (!Equals(itemCategory,null))
                    {
                        // do some code to use the same category (search PMG category and use it in COM)
                        continue;
                    }
                    // create new Category (PropertyDataCollection)
                    InwOaPropertyVec newCat = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                    // convert ModelItem to COM Path
                    InwOaPath cItem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
                    // get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cPropCats = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(cItem, true);
                    foreach (var data in group.Type.Datas)
                    {
                        var itemProperty = item.PropertyCategories.FindPropertyByDisplayName(catDisplayName, data.Name);
                        // check if the element already has the property
                        if (!Equals(itemProperty, null))
                        {
                            // do some code if the property already exist. Update property and check for override
                            continue;
                        }
                        // create a new Property (PropertyData)
                        InwOaProperty newProp = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                        // set PropertyName
                        newProp.name = data.Name + "_InternalName";
                        // set PropertyDisplayName
                        newProp.UserName = data.Name;
                        // set PropertyValue
                        newProp.value = item.GetParameterByName(data.NavisCategoryName, data.NavisPropertyName);
                        // add PropertyData to Category
                        newCat.Properties().Add(newProp);
                    }
                    // add CategoryData to item's CategoryDataCollection
                    cPropCats.SetUserDefined(0, catName, catDisplayName, newCat);
                }
            }
        }
    }
}