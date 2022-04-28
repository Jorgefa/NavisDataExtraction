using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using PM.Navisworks.DataExtraction.Models.DataTransfer;

namespace PM.Navisworks.DataExtraction.Utilities
{
    public static class Configuration
    {
        public static List<Searcher> Import(string fileName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    var dialog = new OpenFileDialog()
                    {
                        Filter = "Json Files (*.json)|*.json",
                        Title = "Select a configuration file",
                        Multiselect = false,
                        CheckFileExists = true,
                        CheckPathExists = true
                    };
                    if (dialog.ShowDialog() != DialogResult.OK) return new List<Searcher>();

                    fileName = dialog.FileName;
                }
                var config = JsonConvert.DeserializeObject<List<Searcher>>(File.ReadAllText(fileName));
                return config;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<Searcher>();
            }
        }

        public static void Export(IEnumerable<Searcher> searchers)
        {
            try
            {
                var dialog = new SaveFileDialog()
                {
                    Filter = "Json Files (*.json)|*.json",
                    Title = "Select a configuration file",
                    OverwritePrompt = true,
                };
                if (dialog.ShowDialog() != DialogResult.OK) return;

                var fileName = dialog.FileName;
                var config = JsonConvert.SerializeObject(searchers, Formatting.Indented);
                File.WriteAllText(fileName, config);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}