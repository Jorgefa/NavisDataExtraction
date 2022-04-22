using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.Models;

namespace PM.Navisworks.DataExtraction.Utilities
{
    public static class NavisworksCollector
    {
        public static List<Category> GetModelCategories(Document document)
        {
            var search = new Search();
            search.PruneBelowMatch = false;
            search.SearchConditions.Clear();
            search.Selection.SelectAll();

            var searchCondition = new SearchCondition(new NamedConstant("Item"), new NamedConstant(""),
                SearchConditionOptions.IgnoreNames, SearchConditionComparison.HasCategory, VariantData.FromNone());
            search.SearchConditions.Add(searchCondition);

            var searchResults = search.FindAll(document, true);


            var categoriesGrouped =
                searchResults.SelectMany(mi => mi.PropertyCategories)
                    .GroupBy(r => r.DisplayName);

            var categories = new List<Category>();

            foreach (var categoryGroup in categoriesGrouped)
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
                    Type type = null;
                    if (property.Value.IsBoolean) type = typeof(bool);
                    if (property.Value.IsAnyDouble) type = typeof(double);
                    if (property.Value.IsInt32) type = typeof(int);
                    if (property.Value.IsDateTime) type = typeof(DateTime);
                    if (property.Value.IsIdentifierString || property.Value.IsDisplayString) type = typeof(string);

                    if (type == null) continue;
                    var prop = new Property
                    {
                        Name = property.DisplayName,
                        ValueType = type
                    };
                    temporaryProperties.Add(prop);
                }

                var propertiesGrouped = temporaryProperties
                    .GroupBy(r => new { r.Name, r.ValueType })
                    .Select(r => r.First());

                category.Properties = new ObservableCollection<Property>(propertiesGrouped);
                categories.Add(category);
            }

            return categories;
        }
    }
}