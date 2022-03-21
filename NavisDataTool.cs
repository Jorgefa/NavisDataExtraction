using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
    
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace NavisDataExtraction
{
    [Plugin("NavisDataExtraction.DataExport",
        "PMPK",
        ToolTip = "Export Data to Json",
        DisplayName ="Export")]
    public class NavisDataTool : AddInPlugin
    {
        public static int count = 0;
        public override int Execute(params string[] parameters)
        {
            var search = new Search();
            search.Selection.SelectAll();

            var condition = SearchCondition.HasPropertyByDisplayName("Revit Type", "UniclassSs");
            search.SearchConditions.Add(condition);

            var elements = search.FindAll(Application.ActiveDocument, true).ToList();
           
            var config = Config.FromFile();

            var navisDataTable = DataExportUtils.CreateNavisDatatable(elements, config.CustomPropertyList);
            navisDataTable.ToCSV(config.csvExportationFilePath);

            return 0;
        }       
    }
}
