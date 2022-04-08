using NavisDataExtraction.Wpf.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace NavisDataExtraction.DataClasses
{
    public class Config : BaseViewModel
    {
        //Constructors
        public Config()
        {
        }

        public Config(ObservableCollection<NavisExtractionType> elementExportTypes = null)
        {
            if (elementExportTypes == null)
            {
                CurrentElementExportTypes = new ObservableCollection<NavisExtractionType>();
            }
            else
            {
                CurrentElementExportTypes = elementExportTypes;
            }
        }

        //Properties
        public static readonly string ConfigLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM Group", "Navis Data Exporter", "appConfig.json");

        public ObservableCollection<NavisExtractionType> CurrentElementExportTypes { get; set; }

        public ObservableCollection<NavisExtractionTypeCollection> NavisExtractionTypeCollections { get; set; }

        //Methods
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

        public bool ConfigValidation()
        {
            foreach (var navisExtractionType in CurrentElementExportTypes)
            {
                if (navisExtractionType.Searchers == null)
                {
                    return true;
                }
                else
                {
                    foreach (var searcher in navisExtractionType.Searchers)
                    {
                        if (string.IsNullOrEmpty(searcher.NavisCategoryName) ||
                            string.IsNullOrEmpty(searcher.NavisPropertyName))
                        {
                            return false;
                        }
                    }
                }
                if (navisExtractionType.Datas == null)
                {
                    return true;
                }
                else
                {
                    foreach (var data in navisExtractionType.Datas)
                    {
                        if (string.IsNullOrEmpty(data.Name) ||
                            string.IsNullOrEmpty(data.NavisCategoryName) ||
                            string.IsNullOrEmpty(data.NavisPropertyName))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
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