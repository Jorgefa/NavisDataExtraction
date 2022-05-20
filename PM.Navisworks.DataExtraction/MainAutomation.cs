using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using CommandLine;
using PM.Navisworks.DataExtraction.Extensions;
using PM.Navisworks.DataExtraction.Models.DataTransfer;
using PM.Navisworks.DataExtraction.Utilities;
using Application = Autodesk.Navisworks.Api.Application;

namespace PM.Navisworks.DataExtraction
{
    [AddInPlugin(AddInLocation.None)]
    [Plugin("PM.Navisworks.DataExtraction.ExportAutomated",
        "PMPK" )]
    public class MainAutomation : AddInPlugin
    {
        private static string _thisAssemblyPath;
        private static Document _document;

        public override int Execute(params string[] parameters)
        {
            _document = Application.ActiveDocument;
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;

            try
            {
                var parserResult = Parser.Default.ParseArguments<MainAutomationOptions>(parameters);
                if (parserResult.Tag == ParserResultType.Parsed)
                {
                    var options = ((Parsed<MainAutomationOptions>)parserResult).Value;
                    ExportData(options);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return 0;
        }

        private void ExportData(MainAutomationOptions options)
        {
            var searchers = new List<Searcher>();
            try
            {
                searchers = Configuration.Import(options.ConfigurationFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            if (!searchers.Any())
            {
                Console.WriteLine("No searchers found in configuration file");
                return;
            }

            try
            {
                if (options.CsvExport) searchers.ExportCsv(_document, options.ExportFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                var exportName = $"{Path.GetFileNameWithoutExtension(options.NavisworksFile)}.csv";
                if (options.CsvCombinedExport) searchers.ExportCsvCombined(_document, Path.Combine(options.ExportFolder, exportName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (options.JsonExport) searchers.ExportJson(_document, options.ExportFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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