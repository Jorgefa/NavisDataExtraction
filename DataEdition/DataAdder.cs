using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.DataEdition
{
    public class DataAdder 
    {
        public static void AddCoordinates(List<ModelItem> elements)
        {
            // current document (.NET)
            Document doc = Application.ActiveDocument;
            // current document (COM)
            InwOpState10 cdoc = ComApiBridge.State;

            foreach (var element in elements)
            {
                // convert ModelItem to COM Path
                InwOaPath cItem = (InwOaPath)ComApiBridge.ToInwOaPath(element);
                // get item's PropertyCategoryCollection
                InwGUIPropertyNode2 cPropCats = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(cItem, true);
                // create new Category (PropertyDataCollection)
                InwOaPropertyVec newCat = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                // create a new Property (PropertyData)
                InwOaProperty newProp = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                // set PropertyName
                newProp.name = "PMG_Property_InternalName";
                // set PropertyDisplayName
                newProp.UserName = "PMG_Property";
                // set PropertyValue
                newProp.value = "MyProperty";
                // add PropertyData to Category
                newCat.Properties().Add(newProp);
                // add CategoryData to item's CategoryDataCollection
                cPropCats.SetUserDefined(0, "PMG", "PMG_InternalName", newCat);
            }
        }
    }
}
