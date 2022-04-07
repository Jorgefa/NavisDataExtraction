using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataCollector
    {
        public static ObservableCollection<ElementExport> ElementCollectorByListOfTypes(ObservableCollection<ElementExportType> elementExportTypes)
        {
            List<ElementExport> elementExportList = new List<ElementExport>();
            foreach (ElementExportType exportType in elementExportTypes)
            {
                List<ElementExport> curElements = ElementCollectorByType(exportType).ToList();
                elementExportList.AddRange(curElements);
            }
            ObservableCollection<ElementExport> obsElementExportList = new ObservableCollection<ElementExport>(elementExportList);
            return obsElementExportList;
        }

        public static ObservableCollection<ElementExport> ElementCollectorByType(ElementExportType elementExportType)
        {
            List<ElementExport> elementExportList = new List<ElementExport>();

            Search search = new Search();
            search.Selection.SelectAll();

            foreach (NavisSearcher searcher in elementExportType.SearcherList)
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

            List<ModelItem> elements = search.FindAll(Application.ActiveDocument, true).ToList();

            foreach (var element in elements)
            {
                ElementExport elementExport = new ElementExport(element, elementExportType);
                elementExportList.Add(elementExport);
            }

            ObservableCollection<ElementExport> obsElementExportList = new ObservableCollection<ElementExport>(elementExportList);
            return obsElementExportList;
        }
    }
}