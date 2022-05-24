using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using PM.Navisworks.DataExtraction.Views;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms.Integration;

namespace PM.Navisworks.DataExtraction
{
    [Plugin("PM.Navisworks.DataExtraction.Export",
        "PMPK",
        ToolTip = "Creates a Configuration file for Automatic Data Export",
        DisplayName = "Export\nConfiguration")]
    public class Main : AddInPlugin
    {
        private static string _thisAssemblyPath;
        private static MainWindow _window;

        public override int Execute(params string[] parameters)
        {
            var activeDoc = Application.ActiveDocument;
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;

            if (_window != null)
            {
                _window.Activate();
            }
            else
            {
                _window = new MainWindow(activeDoc);
                ElementHost.EnableModelessKeyboardInterop(_window);
                _window.Closed += WindowClosed;
                _window.Show();
            }

            return 0;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _window = null;
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