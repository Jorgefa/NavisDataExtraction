using System;
using Autodesk.Navisworks.Api;

namespace PM.Navisworks.DataExtraction.Models
{
    public class Condition
    {
        public Condition(string categoryName)
        {
            Category = categoryName;
        }

        public Condition AddProperty(string propertyName)
        {
            Property = propertyName;
            return this;
        }

        public Condition Compare(ConditionComparer comparison)
        {
            Comparison = comparison;
            return this;
        }

        public Condition With(bool value)
        {
            Value = value;
            return this;
        }

        public Condition With(string value)
        {
            Value = value;
            return this;
        }

        public Condition With(double value)
        {
            Value = value;
            return this;
        }

        public Condition With(int value)
        {
            Value = value;
            return this;
        }

        public Condition With(DateTime value)
        {
            Value = value;
            return this;
        }

        public SearchCondition GetCondition()
        {
            if (string.IsNullOrEmpty(Category))
                throw new Exception("Category must be set");

            SearchCondition condition;
            if (string.IsNullOrEmpty(Property))
            {
                condition = SearchCondition.HasCategoryByDisplayName(Category);
                return condition;
            }

            condition = SearchCondition.HasPropertyByDisplayName(Category, Property);


            if (Value == null || Comparison == ConditionComparer.Exists) return condition;

            var variantData = CreateVariantData();

            CreateComparer(condition, variantData);

            return condition;
        }
        
        private VariantData CreateVariantData()
        {
            VariantData variantData = null;

            switch (Value)
            {
                case bool value:
                    variantData = new VariantData(value);
                    break;
                case string value:
                    variantData = new VariantData(value);
                    break;
                case double value:
                    variantData = new VariantData(value);
                    break;
                case int value:
                    variantData = new VariantData(value);
                    break;
                case DateTime value:
                    variantData = new VariantData(value);
                    break;
            }

            if (variantData == null)
            {
                throw new Exception("Value provided is not of right type");
            }

            return variantData;
        }

        private void CreateComparer(SearchCondition condition, VariantData variantData)
        {
            switch (Comparison)
            {
                case ConditionComparer.Equal:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.NotEqual:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.StringContains:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.GreaterThan:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.LessThan:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.LessThanOrEqual:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                case ConditionComparer.GreaterThanOrEqual:
                    condition.CompareWith((SearchConditionComparison)(int)Comparison, variantData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Comparer provided is not supported");
            }
        }

        private string Category { get; set; }
        private string Property { get; set; }
        private ConditionComparer Comparison { get; set; } = ConditionComparer.Equal;
        private object Value { get; set; }
    }
}