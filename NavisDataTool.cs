using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
    
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NavisDataExtraction.Wpf.Views;
using NavisDataExtraction.Wpf.ViewModels;


namespace NavisDataExtraction
{
    [Plugin("NavisDataExtraction.DataExport",
        "PMPK",
        ToolTip = "Export Data to Json",
        DisplayName ="Export")]
    public class NavisDataTool : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
           
            // var config = Config.FromFile();
            //
            // var elements = ElementSelector.ElementGathering("Revit Type", "UniclassSs");
            //
            // var navisDataTable = DataExtraction.CreateNavisDatatable(elements, config.CustomPropertyList);
            //
            // navisDataTable.ToCSV(config.csvExportationFilePath);

            var window = new MainWindow();
            window.Show();

            return 0;
        }       
    }
}
