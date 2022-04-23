using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using PM.Navisworks.DataExtraction.Extensions;
using PM.Navisworks.DataExtraction.Models.DataTransfer;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction
{
    [Plugin("PM.Navisworks.DataExtraction.ExportAutomated",
        "PMPK")]
    public class MainAutomation : AddInPlugin
    {
        private static string _thisAssemblyPath;

        public override int Execute(params string[] parameters)
        {
            var activeDoc = Application.ActiveDocument;
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;

            var settings = ParseParameters(parameters);
            var configurationFile = settings.FirstOrDefault(r => r.Key == "configurationFile").Value;
            if (string.IsNullOrEmpty(configurationFile))
            {
                Console.WriteLine("Configuration file not provided");
                return 0;
            }
            
            var searchers = new List<SearcherDto>();
            try
            {
                searchers = Configuration.Import(configurationFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            if (!searchers.Any())
            {
                Console.WriteLine("No searchers found in configuration file");
                return 0;
            }

            var exportFolder = settings.FirstOrDefault(r => r.Key == "exportFolder").Value;
            if (string.IsNullOrEmpty(configurationFile))
            {
                Console.WriteLine("Export folder not provided");
                return 0;
            }
            if (!Directory.Exists(exportFolder))
            {
                Console.WriteLine("Export folder does not exist");
                return 0;
            }
            if (SystemExtensions.HasWriteAccessToFolder(exportFolder))
            {
                Console.WriteLine("No write access to export folder");
                return 0;
            }

            var csvExport = settings.FirstOrDefault(r => r.Key == "csvExport").Key;
            var jsonExport = settings.FirstOrDefault(r => r.Key == "jsonExport").Key;

            try
            {
                if(csvExport != null) searchers.ExportCsv(activeDoc, exportFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if(jsonExport != null) searchers.ExportJson(activeDoc, exportFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return 1;
        }

        private Dictionary<string, string> ParseParameters(string[] parameters)
        {
            var settings = new Dictionary<string, string>();
            foreach (var parameter in parameters)
            {
                var split = parameter.Split('=');
                settings.Add(split[0], split[1]);
            }

            return settings;
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