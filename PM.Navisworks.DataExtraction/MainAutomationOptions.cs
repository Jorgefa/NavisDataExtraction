using CommandLine;

namespace PM.Navisworks.DataExtraction
{
    public class MainAutomationOptions
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
    }
}