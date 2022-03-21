using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NavisDataExtraction
{
    public class Config
    {
        public static readonly string ConfigLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PM Group", "Navis Data Exporter", "appConfig.json");

        public List<string> PropertyList { get; set; }

        public Config(List<string> properties = null)
        {
            if (properties == null)
            {
                PropertyList = new List<string>() { "UniclassSs", "Zone", "ModuleNumber", "LineNumber" };
            } else
            {
                PropertyList = properties;
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
