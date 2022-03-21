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
    public class DataExport : AddInPlugin
    {
        public static int count = 0;
        public override int Execute(params string[] parameters)
        {
            var search = new Search();
            search.Selection.SelectAll();

            var condition = SearchCondition.HasPropertyByDisplayName("Revit Type", "UniclassSs");
            search.SearchConditions.Add(condition);

            var elements = search.FindAll(Application.ActiveDocument, true).ToList();

            //var dataElements = elements.Select(e => new NavisDataItem(e));

            //var jsonString = JsonConvert.SerializeObject(dataElements, Formatting.Indented);
            var jsonFilePath = @"D:\02-GITHUB\NavisDataExtration\90-TEST\test.json";
            var csvFilePath = @"D:\02-GITHUB\NavisDataExtration\90-TEST\test.csv";

            //File.WriteAllText(file, jsonString);

            //var dataTable = CreateDatatable(elements);
           
            var config = Config.FromFile();

            var navisDataTable = NavisDataItemTable.CreateNavisDatatable(elements, config.PropertyList);
            navisDataTable.ToCSV(csvFilePath);

            return 0;
        }       
    }
}
