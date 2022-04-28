using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.Models.Data;
using PM.Navisworks.DataExtraction.Models.DataTransfer;

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
    }
}