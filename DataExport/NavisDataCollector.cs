using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataExport;
using System.Collections.Generic;
using System.Linq;

namespace NavisDataExtraction.DataExport
{
    public class NavisDataCollector
    {
        public static List<ElementExport> ElementCollectorByListOfTypes(List<ElementExportType> elementExportTypes)
        {
            List<ElementExport> elements = new List<ElementExport>();
            foreach (ElementExportType exportType in elementExportTypes)
            {
                List<ElementExport> curElements = ElementCollectorByType(exportType);
                elements.AddRange(curElements);
            }

            return elements;
        }

        public static List<ElementExport> ElementCollectorByType(ElementExportType elementExportType)
        {
            List<ElementExport> elementExportList = new List<ElementExport>();
            List<NavisSearcher> navisSearcherList = elementExportType.SearcherList;

            Search search = new Search();
            search.Selection.SelectAll();

            foreach (NavisSearcher searcher in navisSearcherList)
            {
                string searcherCategory = searcher.NavisCategoryName;
                string searcherProperty = searcher.NavisPropertyName;
                string searcherType = searcher.SearchType;

                if (searcherCategory != null && searcherProperty != null)
                {
                    if (searcherType == "HasPropertyByDisplayName")
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

            return elementExportList;
        }
    }
}