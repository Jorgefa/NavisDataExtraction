using NavisDataExtraction.Wpf.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

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
                CurrentElementExportTypes = new ObservableCollection<ElementExportType>();
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
        public bool ConfigValidation()
        {
            foreach (var eleExpTyp in CurrentElementExportTypes)
            {
                foreach (var searcher in eleExpTyp.SearcherList)
                {
                    if (string.IsNullOrEmpty(searcher.NavisCategoryName) ||
                        string.IsNullOrEmpty(searcher.NavisPropertyName))
                    {
                        return false;
                    }
                }
                foreach (var data in eleExpTyp.DataExportList)
                {
                    if (string.IsNullOrEmpty(data.DataName) ||
                        string.IsNullOrEmpty(data.NavisCategoryName) ||
                        string.IsNullOrEmpty(data.NavisPropertyName))
                    {
                        return false;
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