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

        public List<ElementExportType> CurrentElementExportTypes { get; set; }

        public Config(List<ElementExportType> elementExportTypes = null)
        {
            if (elementExportTypes == null)
            {
                NavisDataExportType RevitUniclassSs = new NavisDataExportType("UniclassSs", "Revit Type", "UniclassSs");
                NavisDataExportType RevitZone = new NavisDataExportType("Zone", "Revit Type", "Zone");
                NavisDataExportType RevitModuleNumber = new NavisDataExportType("ModuleNumber", "Revit Type", "ModuleNumber");
                NavisSearcher RevitUniclassSsSearcher = new NavisSearcher("HasPropertyByDisplayName","Revit Type", "UniclassSs");
                List<NavisSearcher> RevitNavisSearcherList = new List<NavisSearcher>
                {
                    RevitUniclassSsSearcher
                };

                NavisDataExportType RevitIfcDataUniclassSs = new NavisDataExportType("UniclassSs", "Data (IFC Type)", "UniclassSs");
                NavisDataExportType RevitIfcDataZone = new NavisDataExportType("Zone", "Data", "Zone");
                NavisDataExportType RevitIfcDataModuleNumber = new NavisDataExportType("ModuleNumber", "Data", "ModuleNumber");
                NavisSearcher RevitIfcDataUniclassSsSearcher = new NavisSearcher("HasPropertyByDisplayName", "Revit Type", "UniclassSs");
                List<NavisSearcher> RevitIfcDataSearcherList = new List<NavisSearcher>
                {
                    RevitIfcDataUniclassSsSearcher
                };

                var ElementExportTypeA = new ElementExportType("RevitType01", RevitNavisSearcherList);
                ElementExportTypeA.AddDataExportType(RevitUniclassSs);
                ElementExportTypeA.AddDataExportType(RevitZone);
                ElementExportTypeA.AddDataExportType(RevitModuleNumber);

                var ElementExportTypeB = new ElementExportType("IfcType01", RevitIfcDataSearcherList);
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