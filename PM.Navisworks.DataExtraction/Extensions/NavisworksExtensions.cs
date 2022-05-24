using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using NavisDataExtraction.Utils.Progress;
using PM.Navisworks.DataExtraction.Models.Data;
using PM.Navisworks.DataExtraction.Models.DataTransfer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PM.Navisworks.DataExtraction.Extensions
{
    public static class NavisworksExtensions
    {
        public static List<Category> GetModelCategories(this Document document)
        {
            var search = new Search();
            search.PruneBelowMatch = false;
            search.SearchConditions.Clear();
            search.Selection.Clear();
            search.Selection.CopyFrom(document.CurrentSelection);

            var searchCondition = SearchCondition.HasPropertyByDisplayName("Item", "GUID");
            search.SearchConditions.Add(searchCondition);

            var searchResults = search.FindAll(document, true);

            var categoriesGrouped =
                searchResults.SelectMany(mi => mi.PropertyCategories)
                    .GroupBy(r => r.DisplayName);

            var categories = new List<Category>();

            foreach (var categoryGroup in categoriesGrouped)
            {
                ProcessCategoryGroup(categoryGroup, categories);
            }

            return categories;
        }

        private static void ProcessCategoryGroup(IGrouping<string, PropertyCategory> categoryGroup,
            ICollection<Category> categories)
        {
            var category = new Category
            {
                Name = categoryGroup.Key,
                Properties = new ObservableCollection<Property>()
            };

            var properties = categoryGroup.SelectMany(r => r.Properties).ToList();
            var temporaryProperties = new List<Property>();
            foreach (var property in properties)
            {
                ProcessProperty(property, temporaryProperties);
            }

            var propertiesGrouped = temporaryProperties
                .GroupBy(r => new { r.Name, r.ValueType })
                .Select(r => r.First());

            category.Properties = new ObservableCollection<Property>(propertiesGrouped);
            categories.Add(category);
        }

        private static void ProcessProperty(DataProperty property, ICollection<Property> temporaryProperties)
        {
            Type type = null;
            if (property.Value.IsBoolean) type = typeof(bool);
            if (property.Value.IsAnyDouble) type = typeof(double);
            if (property.Value.IsInt32) type = typeof(int);
            if (property.Value.IsDateTime) type = typeof(DateTime);
            if (property.Value.IsIdentifierString || property.Value.IsDisplayString) type = typeof(string);

            if (type == null) return;
            var prop = new Property
            {
                Name = property.DisplayName,
                ValueType = type
            };
            temporaryProperties.Add(prop);
        }

        public static ElementData GetData(this ModelItem modelItem, Searcher searcher)
        {
            var elementData = new ElementData()
            {
                ElementGuid = modelItem.InstanceGuid,
                ElementName = modelItem.DisplayName,
                Properties = new List<DataPair>()
            };

            if (searcher.DefaultData.ModelSource)
            {
                elementData.ElementModelSource = modelItem.Ancestors.Last().DisplayName;
            }
            if (searcher.DefaultData.Coordinates)
            {
                elementData.ElementCoordinates = new double[3];
                elementData.ElementCoordinates[0] = ConvertUnitsToMeters(modelItem.BoundingBox().Center.X);
                elementData.ElementCoordinates[1] = ConvertUnitsToMeters(modelItem.BoundingBox().Center.Y);
                elementData.ElementCoordinates[2] = ConvertUnitsToMeters(modelItem.BoundingBox().Center.Z);
            }

            foreach (var propertyPair in searcher.Pairs)
            {
                var property = modelItem.PropertyCategories.FindPropertyByDisplayName(propertyPair.Category.Name,
                    propertyPair.Property.Name);
                if (property == null) continue;
                var value = property.Value.ToString().Replace($"{property.Value.DataType.ToString()}:", "");
                var dataPair = new DataPair()
                {
                    Category = propertyPair.Category.Name,
                    Property = propertyPair.Property.Name,
                    Value = value,
                    ColumnName = propertyPair.ColumnName
                };
                elementData.Properties.Add(dataPair);
            }

            return elementData;
        }

        public static void AddDataToNavis(this ModelItem modelItem, Searcher searcher, Document document, InwOpState10 cDocument, string catDisplayName, bool keepPreviousData)
        {
            var catName = catDisplayName + "_InternalName";
            // get element data
            var elementData = modelItem.GetData(searcher);

            // convert ModelItem to COM Path
            InwOaPath cItem = (InwOaPath)ComApiBridge.ToInwOaPath(modelItem);

            // declare Category (PropertyDataCollection)
            InwOaPropertyVec newCat = (InwOaPropertyVec)cDocument.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

            // get item's PropertyCategoryCollection
            InwGUIPropertyNode2 cItemCats = (InwGUIPropertyNode2)cDocument.GetGUIPropertyNode(cItem, true);

            // set index
            int index = 0;

            // check if the element already has the category
            var itemCategory = modelItem.PropertyCategories.FindCategoryByDisplayName(catDisplayName);

            // create a new category and new properties if category doesn't exist
            if (Equals(itemCategory, null))
            {
                // run through each data
                foreach (var property in elementData.Properties)
                {
                    // create a new Property (PropertyData), set PropertyName, set PropertyDisplayName, set PropertyValue and addd Porperty yo Category
                    InwOaProperty newProp = (InwOaProperty)cDocument.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                    newProp.name = property.ColumnName + "_InternalName";
                    newProp.UserName = property.ColumnName;
                    newProp.value = property.Value;
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
                    foreach (var property in elementData.Properties)
                    {
                        // create a new Property (PropertyData), set PropertyName, set PropertyDisplayName, set PropertyValue and addd Porperty yo Category
                        InwOaProperty newProp = (InwOaProperty)cDocument.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                        newProp.name = property.ColumnName + "_InternalName";
                        newProp.UserName = property.ColumnName;
                        newProp.value = property.Value;
                        newCat.Properties().Add(newProp);
                    }

                    // if category is user defined and same name, and keepPreviousData is true, keep existing parameters in new category (newCat)
                    if (keepPreviousData)
                    {
                        foreach (InwOaProperty prop in atrib.Properties())
                        {
                            var dataNames = searcher.Pairs.Select(x => x.ColumnName).ToList();
                            if (dataNames.Contains(prop.UserName))
                            {
                                continue;
                            }
                            // create a new Property (PropertyData)
                            InwOaProperty newProp = (InwOaProperty)cDocument.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                            newProp.name = prop.name;
                            newProp.UserName = prop.UserName;
                            newProp.value = prop.value;
                            newCat.Properties().Add(newProp);
                        }
                    }
                }

                // add CategoryData to item's CategoryDataCollection
                cItemCats.SetUserDefined(index, catDisplayName, catName, newCat);
            }
        }

        public static FileData GetData(this ModelItemCollection collection, Searcher searcher, Document document)
        {
            var fileInfo = new FileData()
            {
                FileName = document.CurrentFileName,
                SearcherName = searcher.Name,
                ElementsData = new List<ElementData>()
            };
            foreach (var modelItem in collection)
            {
                fileInfo.ElementsData.Add(modelItem.GetData(searcher));
            }

            return fileInfo;
        }

        public static double ConvertUnitsToMeters(double dim)
        {
            // Get current active document.
            Document doc = Application.ActiveDocument;

            // Get units of the document
            Units units = doc.Units;

            // Return converted value to meters
            switch (units)
            {
                case (Units.Centimeters): return dim * 0.01d;
                case (Units.Feet): return dim * 0.3048d;
                case (Units.Inches): return dim * 0.0254d;
                case (Units.Kilometers): return dim * 1000d;
                case (Units.Meters): return dim * 1f;
                case (Units.Microinches): return dim * 0.0000000254d;
                case (Units.Micrometers): return dim * 0.000001d;
                case (Units.Miles): return dim * 1609.43d;
                case (Units.Millimeters): return dim * 0.001d;
                case (Units.Mils): return dim * 0.0000254d;
                case (Units.Yards): return dim * 0.9144d;
                default: return dim * 1;
            }
        }

        public static void AddDataToNavis(this ModelItemCollection collection, Searcher searcher, bool keepPreviousData = true)
        {
            // current document (.NET)
            var document = Application.ActiveDocument;

            // current document (COM)
            var cDocument = ComApiBridge.State;

            // new category name and displayName
            const string catDisplayName = "PMG";

            if (collection == null || collection.Count == 0) return;
            if (searcher == null) return;


            // to do progress bar
            var total = collection.Count();
            var current = 0;

            try
            {
                ProgressUtilDefined.Start();


                foreach (var modelItem in collection)
                {
                    ProgressUtilDefined.Update($"{searcher.Name} - {modelItem.DisplayName}", current, total);

                    modelItem.AddDataToNavis(searcher, document, cDocument, catDisplayName, keepPreviousData);
                    current++;
                }
            }
            catch (Exception)
            {
                // ignore
            }
            finally
            {
                ProgressUtilDefined.Finish();
            }
        }
    }
}