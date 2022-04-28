using System;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.Models.DataTransfer;

namespace PM.Navisworks.DataExtraction.Models.Navisworks
{
    public class NavisworksSearcher
    {
        public NavisworksSearcher()
        {
            Search = new Search();
            Search.Selection.SelectAll();
            Search.Locations = SearchLocations.DescendantsAndSelf;
            Search.PruneBelowMatch = false;
        }
        
        public static Search FromDto(Searcher searcherDto)
        {
            var searcher = new NavisworksSearcher();
            
            searcher.PruneBelow(searcherDto.PruneBelow);
            searcher.AddCondition(new NavisworksCondition("Item")
                .AddProperty("GUID"));
            
            foreach (var condition in searcherDto.Conditions)
            {
                if(condition.Category == null) continue;
                
                var newCondition = new NavisworksCondition(condition.Category.Name);
                
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
        
        public NavisworksSearcher PruneBelow(bool prune = true)
        {
            Search.PruneBelowMatch = prune;
            return this;
        }

        public NavisworksSearcher AddCondition(NavisworksCondition localSearchCondition)
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