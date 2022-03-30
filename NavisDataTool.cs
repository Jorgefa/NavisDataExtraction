using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
    
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NavisDataExtraction.Wpf.Views;
using NavisDataExtraction.Wpf.ViewModels;
using NavisDataExtraction.Wpf;

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
            var window = new MainWindow();
            window.Show();

            return 0;
        }       
    }
}
