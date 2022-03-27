using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction
{
    public class NavisDataCollector
    {
        public static List<ModelItem> ElementCollector(string categoryName = null, string propertyName = null)
        {
            var search = new Search();
            search.Selection.SelectAll();

            if (categoryName != null && propertyName != null )
            {
                var searchCondition = SearchCondition.HasPropertyByDisplayName(categoryName, propertyName);
                search.SearchConditions.Add(searchCondition);
            }

            var elements = search.FindAll(Application.ActiveDocument, true).ToList();

            return elements;
        } 

    }
}
