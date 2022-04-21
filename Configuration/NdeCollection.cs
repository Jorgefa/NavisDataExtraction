using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using NavisDataExtraction.NavisUtils;
using NavisDataExtraction.Utils;
using NavisDataExtraction.Utils.Progress;
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
                modelItemsGRoups.Add(new NdeModelItemGroup
                { ModelItemCollection = SearchModelItems(), Type = type, Collection = this });
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
            var doc = Application.ActiveDocument;

            // current document (COM)
            var cDoc = ComApiBridge.State;

            // testing
            var partition = cDoc.CurrentPartition;

            // new category name and displayName
            const string catDisplayName = "PMG";
            const string catName = catDisplayName + "_InternalName";

            // override
            var keepPreviousData = false;
            var updateProps = false;

            if (modelItemsGroups == null) return;

            var total = modelItemsGroups.SelectMany(r => r.ModelItemCollection).Count();
            var current = 0;

            try
            {
                ProgressUtilDefined.Start();

                foreach (var group in modelItemsGroups)
                {
                    if (group.Type == null || group.ModelItemCollection == null) return;

                    // loop trough each item
                    foreach (var modelItem in group.ModelItemCollection)
                    {
                        ProgressUtilDefined.Update($"{group.Type.Name} - {modelItem.DisplayName}", current, total);

                        // convert ModelItem to COM Path
                        InwOaPath cItem = (InwOaPath)ComApiBridge.ToInwOaPath(modelItem);

                        // declare Category (PropertyDataCollection)
                        InwOaPropertyVec newCat = (InwOaPropertyVec)cDoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                        // get item's PropertyCategoryCollection
                        InwGUIPropertyNode2 cItemCats = (InwGUIPropertyNode2)cDoc.GetGUIPropertyNode(cItem, true);

                        // set index
                        int index = 0;

                        // check if the element already has the category
                        var itemCategory = modelItem.PropertyCategories.FindCategoryByDisplayName(catDisplayName);

                        // create a new category and new properties if category doesn't exist
                        if (Equals(itemCategory, null))
                        {
                            // run through each data
                            foreach (var data in group.Type.Datas)
                            {
                                // create a new Property (PropertyData)
                                InwOaProperty newProp = (InwOaProperty)cDoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                // set PropertyName
                                newProp.name = data.Name + "_InternalName";
                                // set PropertyDisplayName
                                newProp.UserName = data.Name;
                                // set PropertyValue
                                newProp.value = modelItem.GetParameterByName(data.NavisCategoryName, data.NavisPropertyName);
                                // add PropertyData to Category
                                newCat.Properties().Add(newProp);
                            }

                            // add CategoryData to item's CategoryDataCollection
                            cItemCats.SetUserDefined(index, catDisplayName, catName, newCat);
                        }

                        // loop trough existing categories if exists
                        else
                        {
                            index = 1;

                            // category looping using COM
                            foreach (InwGUIAttribute2 atrib in cItemCats.GUIAttributes())
                            {
                                // checks if is user defined
                                if (!atrib.UserDefined)
                                {
                                    continue;
                                }

                                // checks if is same name. If not, increase index and continue
                                if (!Equals(atrib.ClassUserName, catDisplayName))
                                {
                                    index += 1;
                                    continue;
                                }

                                // if category is user defined and same name, add new parameters in new category (newCat)
                                foreach (var data in group.Type.Datas)
                                {
                                    var itemProperty = modelItem.PropertyCategories.FindPropertyByDisplayName(catDisplayName, data.Name);

                                    // check if the element already has the property
                                    if (Equals(itemProperty, null))
                                    {
                                        // create a new Property (PropertyData) and set name, display name and value. Add it to newCat.
                                        InwOaProperty newProp = (InwOaProperty)cDoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                        newProp.name = data.Name + "_InternalName";
                                        newProp.UserName = data.Name;
                                        newProp.value = modelItem.GetParameterByName(data.NavisCategoryName, data.NavisPropertyName);
                                        newCat.Properties().Add(newProp);
                                    }
                                    // if the property exists, add it to the newCat
                                    else
                                    {
                                        // create a new Property (PropertyData) and set name, display name and value. Add it to newCat.
                                        InwOaProperty newProp = (InwOaProperty)cDoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                        newProp.name = data.Name + "_InternalName";
                                        newProp.UserName = data.Name;
                                        var newValue = modelItem.GetParameterByName(data.NavisCategoryName, data.NavisPropertyName);
                                        if (Equals(newValue, null))
                                        {
                                            newProp.value = modelItem.GetParameterByName(catDisplayName, data.Name);
                                        }
                                        else
                                        {
                                            newProp.value = newValue;
                                        }
                                        newCat.Properties().Add(newProp);
                                    }
                                }

                                // if category is user defined and same name, and keepPreviousData is true, keep existing parameters in new category (newCat)
                                if (keepPreviousData)
                                {
                                    foreach (InwOaProperty prop in atrib.Properties())
                                    {
                                        var dataNames = group.Type.Datas.Select(x => x.Name).ToList();
                                        if (dataNames.Contains(prop.UserName))
                                        {
                                            continue;
                                        }
                                        // create a new Property (PropertyData)
                                        InwOaProperty newProp = (InwOaProperty)cDoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                        newProp.name = prop.name;
                                        newProp.UserName = prop.UserName;
                                        newProp.value = prop.value;
                                        newCat.Properties().Add(newProp);
                                    }
                                }
                                break;
                            }

                            // add CategoryData to item's CategoryDataCollection
                            cItemCats.SetUserDefined(index, catDisplayName, catName, newCat);
                        }

                        current++;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                ProgressUtilDefined.Finish();
            }
        }
    }
}