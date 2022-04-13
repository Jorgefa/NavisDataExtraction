using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using NavisDataExtraction.DataClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

            search.Locations = SearchLocations.DescendantsAndSelf;

            search.PruneBelowMatch = false;

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


            var curSelect = Application.ActiveDocument.CurrentSelection;
            curSelect.Clear();
            curSelect.AddRange(elements);


            ObservableCollection<NavisExtractionElement> obsElementExportList = new ObservableCollection<NavisExtractionElement>(elementExportList);
            return obsElementExportList;


        }

        public static void IsolateElements(ObservableCollection<ModelItem> modelItems)
        {
            //Create hidden collection

            List<ModelItem> hidden = new List<ModelItem>();

            //create a store for the visible items

            List<ModelItem> visible = new List<ModelItem>();

            //Add all the items that are visible to the visible list

            foreach (ModelItem item in Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems)

            {
                if (item.AncestorsAndSelf != null)

                    visible.AddRange(item.AncestorsAndSelf);

                if (item.Descendants != null)

                    visible.AddRange(item.Descendants);
            }

            //mark as invisible all the siblings of the visible items

            foreach (ModelItem toShow in visible)

            {
                if (toShow.Parent != null)

                {
                    hidden.AddRange(toShow.Parent.Children);
                }
            }

            //remove the visible items from the list

            foreach (ModelItem toShow in visible)

            {
                hidden.Remove(toShow);
            }

            //hide the remaining items

            Autodesk.Navisworks.Api.Application.ActiveDocument.Models.SetHidden(hidden, true);
        }
    }
}