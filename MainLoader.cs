using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisDataExtraction
{
    [Plugin("NavisDataExtraction.MainLoader",
        "PMPK",
        DisplayName ="Export Loader")]
    public class MainLoader : EventWatcherPlugin
    {
        private static string _thisAssemblyPath;

        private Assembly ResolveAssemblies(object sender, ResolveEventArgs args)
        {
            
            var path = Path.GetDirectoryName(_thisAssemblyPath);
            if (path == null) return null;
            var dll = $"{new Regex(",.*").Replace(args.Name, string.Empty)}.dll";
            var file = Path.Combine(path, dll);

            if (!File.Exists(file)) return null;

            return Assembly.LoadFrom(file);
        }

        public override void OnLoaded()
        {
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;
        }

        public override void OnUnloading()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssemblies;
        }
    }
}