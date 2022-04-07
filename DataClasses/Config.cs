using NavisDataExtraction.Wpf.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace NavisDataExtraction.DataClasses
{
    public class Config : BaseViewModel
    {
        public static readonly string ConfigLocation =  
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PM Group", "Navis Data Exporter", "appConfig.json");

        public ObservableCollection<ElementExportType> CurrentElementExportTypes { get; set; }

        public Config(ObservableCollection<ElementExportType> elementExportTypes = null)
        {
            if (elementExportTypes == null)
            {
                NavisDataExport RevitUniclassSs = new NavisDataExport("UniclassSs", "Revit Type", "UniclassSs");
                NavisDataExport RevitZone = new NavisDataExport("Zone", "Revit Type", "Zone");
                NavisDataExport RevitModuleNumber = new NavisDataExport("ModuleNumber", "Revit Type", "ModuleNumber");
                NavisSearcher RevitUniclassSsSearcher = new NavisSearcher(NavisSearchType.HasPropertyByDisplayName,"Revit Type", "UniclassSs");
                ObservableCollection<NavisSearcher> RevitNavisSearcherList = new ObservableCollection<NavisSearcher>
                {
                    RevitUniclassSsSearcher
                };

                NavisDataExport RevitIfcDataUniclassSs = new NavisDataExport("UniclassSs", "Data (IFC Type)", "UniclassSs");
                NavisDataExport RevitIfcDataZone = new NavisDataExport("Zone", "Data", "Zone");
                NavisDataExport RevitIfcDataModuleNumber = new NavisDataExport("ModuleNumber", "Data", "ModuleNumber");
                NavisSearcher RevitIfcDataUniclassSsSearcher = new NavisSearcher(NavisSearchType.HasPropertyByDisplayName, "Revit Type", "UniclassSs");
                ObservableCollection<NavisSearcher> RevitIfcDataSearcherList = new ObservableCollection<NavisSearcher>
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

                CurrentElementExportTypes = new ObservableCollection<ElementExportType>();

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