using NavisDataExtraction.DataExport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace NavisDataExtraction
{
    public class Config
    {
        public static readonly string ConfigLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PM Group", "Navis Data Exporter", "appConfig.json");

        public string csvExportationFilePath { get; set; } = @"D:\02-GITHUB\NavisDataExtration\90-TEST\test.csv";

        public List<ElementExportType> CurrentElementExportTypes { get; set; }

        public Config(List<ElementExportType> elementExportTypes = null)
        {
            if (elementExportTypes == null)
            {
                DataExportType RevitUniclassSs = new DataExportType("UniclassSs", "Revit Type", "UniclassSs");
                DataExportType RevitZone = new DataExportType("Zone", "Revit Type", "Zone");
                DataExportType RevitModuleNumber = new DataExportType("ModuleNumber", "Revit Type", "ModuleNumber");

                DataExportType RevitIfcDataUniclassSs = new DataExportType("UniclassSs", "Data (IFC Type)", "UniclassSs");
                DataExportType RevitIfcDataZone = new DataExportType("Zone", "Data", "Zone");
                DataExportType RevitIfcDataModuleNumber = new DataExportType("ModuleNumber", "Data", "ModuleNumber");

                var ElementExportTypeA = new ElementExportType("RevitType01", RevitUniclassSs);
                ElementExportTypeA.AddDataExportType(RevitUniclassSs);
                ElementExportTypeA.AddDataExportType(RevitZone);
                ElementExportTypeA.AddDataExportType(RevitModuleNumber);

                var ElementExportTypeB = new ElementExportType("IfcType01", RevitIfcDataUniclassSs);
                ElementExportTypeB.AddDataExportType(RevitIfcDataUniclassSs);
                ElementExportTypeB.AddDataExportType(RevitIfcDataZone);
                ElementExportTypeB.AddDataExportType(RevitIfcDataModuleNumber);

                CurrentElementExportTypes = new List<ElementExportType>();

                CurrentElementExportTypes.Add(ElementExportTypeA);
                CurrentElementExportTypes.Add(ElementExportTypeB);
            }
            else
            {
                CurrentElementExportTypes = elementExportTypes;
            }
        }

        public static Config FromFile(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation)) fileLocation = ConfigLocation;
            if (!File.Exists(fileLocation))
            {
                var newConfig = new Config();
                newConfig.ToFile(fileLocation);
                return newConfig;
            }

            var configText = File.ReadAllText(fileLocation);
            var config = JsonConvert.DeserializeObject<Config>(configText);
            if (config == null)
            {
                var newConfig = new Config();
                newConfig.ToFile(fileLocation);
                return newConfig;
            }
            return config;
        }

        public void ToFile(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation)) fileLocation = ConfigLocation;
            var jsonString = JsonConvert.SerializeObject(this);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));
                File.WriteAllText(fileLocation, jsonString);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}