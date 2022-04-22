using System;
using Autodesk.Navisworks.Api;

namespace PM.Navisworks.DataExtraction.Models
{
    public class Searcher
    {
        public Searcher()
        {
            Search = new Search();
            Search.Selection.SelectAll();
            Search.Locations = SearchLocations.DescendantsAndSelf;
            Search.PruneBelowMatch = false;
        }
        
        public static Search FromDto(SearcherDto searcherDto)
        {
            var searcher = new Searcher();
                
            searcher.PruneBelow(searcherDto.PruneBelow);
            
            foreach (var condition in searcherDto.Conditions)
            {
                if(condition.Category == null) continue;
                
                var newCondition = new Condition(condition.Category.Name);
                
                if (condition.Property == null)
                {
                    searcher.AddCondition(newCondition);
                    continue;
                }
                newCondition.AddProperty(condition.Property.Name);

                if (condition.Comparer == ConditionComparer.Exists)
                {
                    searcher.AddCondition(newCondition);
                    continue;
                }

                newCondition.Compare(condition.Comparer);

                var type = condition.Property.ValueType;

                if (type == typeof(bool)) newCondition.With(condition.BoolValue);
                if (type == typeof(string)) newCondition.With(condition.StringValue);
                if (type == typeof(double)) newCondition.With(condition.DoubleValue);
                if (type == typeof(int)) newCondition.With(condition.IntegerValue);
                if (type == typeof(DateTime)) newCondition.With(condition.DateTimeValue);

                searcher.AddCondition(newCondition);
            }

            return searcher.GetSearch();
        }

        private Search Search { get; set; }
        
        public Searcher PruneBelow(bool prune = true)
        {
            Search.PruneBelowMatch = prune;
            return this;
        }

        public Searcher AddCondition(Condition localSearchCondition)
        {
            var condition = localSearchCondition.GetCondition();
            Search.SearchConditions.Add(condition);
            return this;
        }

        public Search GetSearch()
        {
            return Search;
        }
    }
}