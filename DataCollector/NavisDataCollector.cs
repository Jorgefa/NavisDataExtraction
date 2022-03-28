using Autodesk.Navisworks.Api;
using NavisDataExtraction.DataExport;
using System.Collections.Generic;
using System.Linq;

namespace NavisDataExtraction.DataCollector
{
    public class NavisDataCollector
    {
        public static List<ModelItem> ElementCollector(List<ElementExportType> elementExportTypes)
        {
            List<ModelItem> elements = new List<ModelItem>();
            foreach (ElementExportType exportType in elementExportTypes)
            {
                string searcherCategory = exportType.Searcher.NavisCategoryName;
                string searcherProperty = exportType.Searcher.NavisPropertyName;
                Search search = new Search();
                search.Selection.SelectAll();

                if (searcherCategory != null && searcherProperty != null)
                {
                    var searchCondition = SearchCondition.HasPropertyByDisplayName(searcherCategory, searcherProperty);
                    search.SearchConditions.Add(searchCondition);
                }
                List<ModelItem> curElements = search.FindAll(Application.ActiveDocument, true).ToList();
                elements.AddRange(curElements);
            }

            return elements;
        }
    }
}