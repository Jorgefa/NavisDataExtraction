using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
    
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NavisDataExtraction.Wpf.Views;
using NavisDataExtraction.Wpf.ViewModels;
using NavisDataExtraction.Wpf;
using System.Windows.Forms.Integration;

namespace NavisDataExtraction
{
    [Plugin("NavisDataExtraction.DataExport",
        "PMPK",
        ToolTip = "Export Data to Json",
        DisplayName ="DataExtraction")]
    [RibbonTab("PMG_CustomTab_01", DisplayName = "PMG Tools")]
    [Command("ID_BUtton_01",ToolTip ="Interface")]
    public class Main : AddInPlugin
    {
        private static string _thisAssemblyPath;
        public override int Execute(params string[] parameters)
        {
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;
            
            var window = new MainWindow();
            ElementHost.EnableModelessKeyboardInterop(window);
            window.Show();
            return 0;
        }

        private Assembly ResolveAssemblies(object sender, ResolveEventArgs args)
        {
            
            var path = Path.GetDirectoryName(_thisAssemblyPath);
            if (path == null) return null;
            var dll = $"{new Regex(",.*").Replace(args.Name, string.Empty)}.dll";
            var file = Path.Combine(path, dll);

            if (!File.Exists(file)) return null;

            return Assembly.LoadFrom(file);
        }
    }
}
