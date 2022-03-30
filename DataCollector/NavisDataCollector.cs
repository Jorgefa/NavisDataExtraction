using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataExport;
using System.Collections.Generic;
using System.Linq;

namespace NavisDataExtraction.DataCollector
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
            string searcherCategory = elementExportType.Searcher.NavisCategoryName;
            string searcherProperty = elementExportType.Searcher.NavisPropertyName;
            Search search = new Search();
            search.Selection.SelectAll();

            if (searcherCategory != null && searcherProperty != null)
            {
                var searchCondition = SearchCondition.HasPropertyByDisplayName(searcherCategory, searcherProperty);
                search.SearchConditions.Add(searchCondition);
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