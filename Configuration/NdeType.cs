using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace NavisDataExtraction.Configuration
{
    public class NdeType : NdeSelectableItem
    {
        //Constructors
        public NdeType()
        {
            Searchers = new ObservableCollection<NavisExtractionSearcher>();
            Datas = new ObservableCollection<NdeData>();
        }

        public NdeType(string name)
        {
            Name = name;
            Searchers = new ObservableCollection<NavisExtractionSearcher>();
            Datas = new ObservableCollection<NdeData>();
        }

        //Properties
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

        private ObservableCollection<NdeData> _datas;

        public ObservableCollection<NdeData> Datas
        {
            get { return _datas; }
            set
            {
                _datas = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NavisExtractionSearcher> _searchers;

        public ObservableCollection<NavisExtractionSearcher> Searchers
        {
            get { return _searchers; }
            set
            {
                _searchers = value;
                OnPropertyChanged();
            }
        }

        //Methods
        public string SearchersValidation()
        {
            if (Searchers != null)
            {
                foreach (var searcher in Searchers)
                {
                    if (string.IsNullOrEmpty(searcher.NavisCategoryName) ||
                        string.IsNullOrEmpty(searcher.NavisPropertyName))
                    {
                        return "blankValue";
                    }
                }
            }
            return "ok";
        }

        public string DatasValidation()
        {
            if (Datas != null)
            {
                var dataNames = Datas.ToList().Select(x => x.Name).ToList();

                if (dataNames.Count != dataNames.Distinct().Count())
                {
                    return "duplicates";
                }

                foreach (var data in Datas)
                {
                    if (string.IsNullOrEmpty(data.Name) ||
                        string.IsNullOrEmpty(data.NavisCategoryName) ||
                        string.IsNullOrEmpty(data.NavisPropertyName))
                    {
                        return "blankValue";
                    }
                }
            }
            return "ok";
        }

        public ObservableCollection<ModelItem> SearchModelItems()
        {
            // Get ActiveDocument
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            // Create Search
            Search search = new Search();
            search.Selection.SelectAll();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.PruneBelowMatch = false;

            // Create SearchConditions
            if (Searchers == null)
            {
                return null;
            }

            foreach (var searcher in Searchers)
            {
                switch (searcher.SearchType)
                {
                    case NavisSearchType.HasPropertyByDisplayName:
                        var sC = SearchCondition.HasPropertyByDisplayName(searcher.NavisCategoryName, searcher.NavisPropertyName);
                        search.SearchConditions.Add(sC);
                        break;
                    default:
                        break;
                }
            }

            // Collect ModelItems
            ObservableCollection<ModelItem> modelItems = new ObservableCollection<ModelItem>(search.FindAll(doc, true));

            return modelItems;
        }

        public NdeModelItemGroup SearchModelItemGroup()
        {
            return new NdeModelItemGroup { ModelItemCollection = SearchModelItems(), Type = this };
        }
    }
}