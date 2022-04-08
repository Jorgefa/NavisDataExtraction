using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataCollector
    {
        public static ObservableCollection<NavisExtractionElement> ElementCollectorByListOfTypes(ObservableCollection<NavisExtractionType> elementExportTypes)
        {
            List<NavisExtractionElement> elementExportList = new List<NavisExtractionElement>();
            foreach (NavisExtractionType exportType in elementExportTypes)
            {
                List<NavisExtractionElement> curElements = ElementCollectorByType(exportType).ToList();
                elementExportList.AddRange(curElements);
            }
            ObservableCollection<NavisExtractionElement> obsElementExportList = new ObservableCollection<NavisExtractionElement>(elementExportList);
            return obsElementExportList;
        }

        public static ObservableCollection<NavisExtractionElement> ElementCollectorByType(NavisExtractionType elementExportType)
        {
            List<NavisExtractionElement> elementExportList = new List<NavisExtractionElement>();

            Search search = new Search();
            search.Selection.SelectAll();

            if (elementExportType.Searchers == null)
            {
                return null;
            }

            foreach (NavisExtractionSearcher searcher in elementExportType.Searchers)
            {
                string searcherCategory = searcher.NavisCategoryName;
                string searcherProperty = searcher.NavisPropertyName;
                NavisSearchType searcherType = searcher.SearchType;

                if (searcherCategory != null && searcherProperty != null)
                {
                    if (searcherType == NavisSearchType.HasPropertyByDisplayName)
                    {
                        var searchCondition = SearchCondition.HasPropertyByDisplayName(searcherCategory, searcherProperty);
                        search.SearchConditions.Add(searchCondition);
                    }
                }
            }

            List<ModelItem> elements = search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, true).ToList();

            foreach (var element in elements)
            {
                NavisExtractionElement elementExport = new NavisExtractionElement(element, elementExportType);
                elementExportList.Add(elementExport);
            }

            ObservableCollection<NavisExtractionElement> obsElementExportList = new ObservableCollection<NavisExtractionElement>(elementExportList);
            return obsElementExportList;
        }
    }
}