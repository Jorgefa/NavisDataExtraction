using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using NavisDataExtraction.Configuration;
using NavisDataExtraction.DataClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NavisDataExtraction.Utils
{
    public class NdeNavisCommands
    {
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
        public static void SelectElements(ObservableCollection<ModelItem> modelItems)
        {
            // current document (.NET)
            Document doc = Application.ActiveDocument;

            if (modelItems == null)
            {
                return;
            }
            doc.CurrentSelection.AddRange(modelItems);
        }
    }
}