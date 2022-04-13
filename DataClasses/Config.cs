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

        public string CollectionsValidation()
        {
            if (NavisExtractionTypeCollections == null)
            {
                return null;
            }

            var collectionNames = NavisExtractionTypeCollections.ToList().Select(x => x.Name).ToList();

            if (collectionNames.Count != collectionNames.Distinct().Count())
            {
                return "duplicates";
            }

            return "ok";
        }

        public string ConfigValidation()
        {
            switch (CollectionsValidation())
            {
                case null:
                    return "ok-collections-null";

                case "duplicates":
                    return "fail-collections-duplicates";

                default:
                    foreach (var collection in NavisExtractionTypeCollections)
                    {
                        switch (collection.TypesValidation())
                        {
                            case "duplicates":
                                return "fail-types-duplicates";

                            case null:
                                break;

                            default:
                                foreach (var type in collection.Types)
                                {
                                    switch (type.SearchersValidation())
                                    {
                                        case "blankValue":
                                            return "fail-searchers-blankValue";

                                        default:
                                            break;
                                    }
                                    switch (type.DatasValidation())
                                    {
                                        case "duplicates":
                                            return "fail-data-duplicates";

                                        case "blankValue":
                                            return "fail-data-blankValue";

                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    return "ok";
            }
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
            switch (ConfigValidation())
            {
                case "ok":
                    ToFile();
                    break;

                case "ok-collections-null":
                    NavisExtractionTypeCollections = new ObservableCollection<NavisExtractionTypeCollection>();
                    ToFile();
                    break;

                default:
                    System.Windows.MessageBox.Show("Please enter correct  config data. Not duplicate names allowed and searchers and datas must be fullfiled (category and property).", "Error");
                    break;
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