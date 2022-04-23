using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms.Integration;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using PM.Navisworks.DataExtraction.Views;

namespace PM.Navisworks.DataExtraction
{
    [Plugin("PM.Navisworks.DataExtraction.Export",
        "PMPK",
        ToolTip = "Creates a Configuration file for Automatic Data Export",
        DisplayName = "Export\nConfiguration")]
    public class Main : AddInPlugin
    {
        private static string _thisAssemblyPath;

        public override int Execute(params string[] parameters)
        {
            var activeDoc = Application.ActiveDocument;
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;

            var window = new MainWindow(activeDoc);
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