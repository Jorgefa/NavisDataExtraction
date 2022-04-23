using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Autodesk.Navisworks.Api.Automation;
using Autodesk.Navisworks.Api.Resolver;

namespace PM.Navisworks.DataExtraction.Automation
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode);
        }

        private static void RunOptionsAndReturnExitCode(Options options)
        {
            var file = options.NavisworksFile;
            if (!File.Exists(file))
            {
                Console.WriteLine($"File {file} does not exist.");
                return;
            }
            if (!(!Path.GetExtension(file).Equals(".nwf", StringComparison.OrdinalIgnoreCase)
                || !Path.GetExtension(file).Equals(".nwd", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"File {file} is not a Navisworks file.");
                return;
            }

            var config = options.ConfigurationFile;
            if (!File.Exists(config))
            {
                Console.WriteLine($"Configuration file {config} does not exist.");
                return;
            }

            var folder = options.ExportFolder;
            if (!Directory.Exists(folder))
            {
                Console.WriteLine($"Export folder {folder} does not exist.");
                return;
            }
            if (!HasWriteAccessToFolder(folder))
            {
                Console.WriteLine($"Export folder {folder} is not writeable.");
                return;
            }
            
            if (!options.CsvExport || !options.JsonExport)
            {
                Console.WriteLine($"No export format specified.");
                return;
            }

            Export(options);
        }

        private static void Export(Options options)
        {
            
            var runtimeName = Resolver.TryBindToRuntime(RuntimeNames.Any);
            if (string.IsNullOrEmpty(runtimeName))
            {
                throw new Exception("Failed to bind to Navisworks runtime");
            }
            
            var configurationFile = $"configurationFile={options.ConfigurationFile}";
            var exportFolder = $"exportFolder={options.ConfigurationFile}";
            var csvExport = options.CsvExport ? "csvExport" : "";
            var jsonExport = options.JsonExport ? "jsonExport" : "";
            
            NavisworksApplication application = null;

            try
            {
                application = new NavisworksApplication();
                application.DisableProgress();
                application.Visible = true;
                application.OpenFile(options.NavisworksFile);
                var parameters = options.WriteToArray();
                var result = application.ExecuteAddInPlugin("PM.Navisworks.DataExtraction.ExportAutomated.PMPK", parameters);

                application.EnableProgress();
            }
            catch (AutomationException e)
            {
                Console.WriteLine($"Error:{e.Message}");
            }
            catch (AutomationDocumentFileException e)
            {
                Console.WriteLine($"Error:{e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e.Message}");
            }
            finally
            {
                application?.Dispose();
            }
        }


        public static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }

    public class Options
    {
        
        [Option('n', "navisworks", Required = true, HelpText = "Navisworks (.nwd/.nwf) file")]
        public string NavisworksFile { get; set; }
        [Option('c', "config", Required = true, HelpText = "Configuration .json file")]
        public string ConfigurationFile { get; set; }
        [Option('f', "exportFolder", Required = true, HelpText = "Export folder")]
        public string ExportFolder { get; set; }
        
        [Option("csv", Required = false, HelpText = "Whether to export to .csv files")]
        public bool CsvExport { get; set; }
        [Option("json", Required = false, HelpText = "Whether to export to .json files")]
        public bool JsonExport { get; set; }

        public string[] WriteToArray()
        {
            var options = new List<string>();
            
            options.Add("-n");
            options.Add(NavisworksFile);
            options.Add("-c");
            options.Add(ConfigurationFile);
            options.Add("-f");
            options.Add(ExportFolder);
            if(CsvExport) options.Add("--csv");
            if(JsonExport) options.Add("--json");
            
            return options.ToArray();
        }
    }
}