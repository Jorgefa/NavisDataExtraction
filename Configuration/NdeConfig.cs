using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NavisDataExtraction.Configuration
{
    public class NdeConfig : NdeObservableItem
    {
        //Constructors
        public NdeConfig()
        {
        }

        //Properties
        public static readonly string ConfigDefaultLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM Group", "Navis Data Exporter", "appConfig.json");

        private ObservableCollection<NdeCollection> _collections;

        public ObservableCollection<NdeCollection> Collections
        {
            get { return _collections; }
            set
            {
                _collections = value; OnPropertyChanged();
            }
        }

        //Methods
        public static NdeConfig FromFile(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation)) fileLocation = ConfigDefaultLocation;
            if (!File.Exists(fileLocation))
            {
                var newConfig = new NdeConfig();
                newConfig.ToFile(fileLocation);
                return newConfig;
            }

            var configText = File.ReadAllText(fileLocation);
            var config = JsonConvert.DeserializeObject<NdeConfig>(configText);
            if (config == null)
            {
                var newConfig = new NdeConfig();
                newConfig.ToFile(fileLocation);
                return newConfig;
            }
            return config;
        }

        public string CollectionsValidation()
        {
            if (Collections == null)
            {
                return null;
            }

            var collectionNames = Collections.ToList().Select(x => x.Name).ToList();

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
                    foreach (var collection in Collections)
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
                    Collections = new ObservableCollection<NdeCollection>();
                    ToFile();
                    break;

                default:
                    System.Windows.MessageBox.Show("Please enter correct  config data. Not duplicate names allowed and searchers and datas must be fullfiled (category and property).", "Error");
                    break;
            }
        }

        public static NdeConfig ImportConfigFromFile()
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