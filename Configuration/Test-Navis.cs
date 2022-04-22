using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;



namespace CScript
{
    public class CScript
    {
        static public void Main()
        {
            // Get ActiveDocument
            var doc = Application.ActiveDocument;

            // Create Search
            Search search = new Search();
            search.Selection.SelectAll();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.PruneBelowMatch = false;

            List<SearchCondition> sCGroup = new List<SearchCondition>();

            var sCCat = SearchCondition.HasCategoryByDisplayName("Data");
            var sCProp = SearchCondition.HasPropertyByDisplayName("Data", "FPC");

            sCProp = sCProp.CompareWith(SearchConditionComparison.Equal, VariantData.FromDisplayString("FPC1"));
            sCProp = sCProp.CompareWith(SearchConditionComparison.HasCategory, VariantData.FromNone());





            sCGroup.Add(sCProp);

            search.SearchConditions.Add(sCProp);
            //search.SearchConditions.AddGroup(sCGroup);

            // Collect ModelItems
            ObservableCollection<ModelItem> modelItems = new ObservableCollection<ModelItem>(search.FindAll(doc, true));

            doc.CurrentSelection.Clear();
            doc.CurrentSelection.AddRange(modelItems);

            Console.WriteLine(sCProp.Comparison);
            Console.WriteLine(sCProp.Options);
            Console.WriteLine(sCProp.CategoryCombinedName);
            Console.WriteLine(sCProp.PropertyCombinedName);
            Console.WriteLine(sCProp.Value);

        }
    }
}