using NavisDataExtraction.Wpf.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
                var currentCollection = new NavisExtractionTypeCollection();
                currentCollection.Types = CurrentElementExportTypes;
                NavisExtractionTypeCollections.Add(currentCollection);
            }
        }

        //Properties
        public static readonly string ConfigDefaultLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM Group", "Navis Data Exporter", "appConfig.json");

        public ObservableCollection<NavisExtractionType> CurrentElementExportTypes { get; set; }

        private ObservableCollection<NavisExtractionTypeCollection> _navisExtractionTypeCollections;

        public ObservableCollection<NavisExtractionTypeCollection> NavisExtractionTypeCollections
        {
            get { return _navisExtractionTypeCollections; }
            set
            {
                _navisExtractionTypeCollections = value; OnPropertyChanged();
            }
        }

        //Methods
        public static Config FromFile(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation)) fileLocation = ConfigDefaultLocation;
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

        public bool CollectionValidation()
        {
            if (NavisExtractionTypeCollections == null)
            {
                return true;
            }

            var collectionNames = NavisExtractionTypeCollections.ToList().Select(x => x.Name).ToList();

            if (collectionNames.Count != collectionNames.Distinct().Count())
            {
                return false;
            }

            return true;
        }

        public bool TypesValidation()
        {
            foreach (var collection in NavisExtractionTypeCollections)
            {
                if (collection.Types == null)
                {
                    return true;
                }

                var typeNames = collection.Types.ToList().Select(x => x.Name).ToList();

                if (typeNames.Count != typeNames.Distinct().Count())
                {
                    return false;
                }
            }
            return true;
        }

        public bool SearchersValidation()
        {
            foreach (var collection in NavisExtractionTypeCollections)
            {
                if (collection.Types != null)
                {
                    foreach (var type in collection.Types)
                    {
                        if (type.Searchers != null)
                        {
                            foreach (var searcher in type.Searchers)
                            {
                                if (string.IsNullOrEmpty(searcher.NavisCategoryName) ||
                                    string.IsNullOrEmpty(searcher.NavisPropertyName))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool DatasValidation()
        {
            foreach (var collection in NavisExtractionTypeCollections)
            {
                if (collection.Types != null)
                {
                    foreach (var type in collection.Types)
                    {
                        if (type.Datas != null)
                        {
                            var dataNames = type.Datas.ToList().Select(x => x.Name).ToList();

                            if (dataNames.Count != dataNames.Distinct().Count())
                            {
                                return false;
                            }

                            foreach (var data in type.Datas)
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
                }
            }
            return true;
        }

        public bool ConfigValidation()
        {
            return CollectionValidation() && TypesValidation() && SearchersValidation() && DatasValidation();
        }

        public void ToFile(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation)) fileLocation = ConfigDefaultLocation;
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
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

        public void SaveConfig()
        {
            if (ConfigValidation())
            {
                ToFile();
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter correct  config data. Check that: there are not duplicates in types or data and searchers and datas are fullfiled (category and property).");
            }
        }

        public static Config ImportConfigFromFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = dialog.FileName;
                var config = FromFile(filePath);
                return config;
            }
            return null;
        }

        public void ExportConfigToFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = dialog.FileName;
                ToFile(filePath);
            }
        }
    }
}