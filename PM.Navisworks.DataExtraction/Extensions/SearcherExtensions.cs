using Autodesk.Navisworks.Api;
using Newtonsoft.Json;
using PM.Navisworks.DataExtraction.Models.Data;
using PM.Navisworks.DataExtraction.Models.DataTransfer;
using PM.Navisworks.DataExtraction.Models.Navisworks;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PM.Navisworks.DataExtraction.Extensions
{
    public static class SearcherExtensions
    {
        public static FileData GetData(this Searcher searcher, Document document)
        {
            if (searcher == null || document == null) return null;
            var search = NavisworksSearcher.FromDto(searcher);
            var elements = search.FindAll(document, false);

            return elements.GetData(searcher, document);
        }

        public static List<FileData> GetData(this IEnumerable<Searcher> searchers, Document document)
        {
            if (searchers == null || document == null) return new List<FileData>();
            var dtos = searchers.ToList();
            return !dtos.Any() ? new List<FileData>() : dtos.Select(r => r.GetData(document)).ToList();
        }

        public static void ExportCsv(this IEnumerable<Searcher> searchers, Document document, string folder = "")
        {
            try
            {
                if (string.IsNullOrEmpty(folder))
                {
                    var dialog = new FolderBrowserDialog()
                    {
                        ShowNewFolderButton = true,
                    };
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    folder = dialog.SelectedPath;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            foreach (var searcher in searchers)
            {
                //TODO: Flatten DataTable
                var file = Path.GetFileNameWithoutExtension(document.CurrentFileName);
                var fileName = $"{file}_{searcher.Name}.csv";
                var filePath = Path.Combine(folder, fileName);
                searcher.ExportCsv(document, filePath);
            }
        }

        public static void ExportCsv(this Searcher searcher, Document document, string filePath)
        {
            var table = new DataTable();
            table.Columns.Add("FileName");
            table.Columns.Add("SearcherName");
            table.Columns.Add("ElementName");
            table.Columns.Add("ElementGuid");
            foreach (var pair in searcher.Pairs)
            {
                table.Columns.Add($"{pair.Category.Name}_{pair.Property.Name}");
            }

            var data = searcher.GetData(document);
            foreach (var elementData in data.ElementsData)
            {
                var row = table.NewRow();
                row["FileName"] = document.CurrentFileName;
                row["SearcherName"] = searcher.Name;
                row["ElementName"] = elementData.ElementName;
                row["ElementGuid"] = elementData.ElementGuid;
                foreach (var property in elementData.Properties)
                {
                    row[$"{property.Category}_{property.Property}"] = property.Value;
                }

                table.Rows.Add(row);
            }

            try
            {
                table.ToCsv(filePath);
            }
            catch (Exception e)
            {
                //TODO: Handle Errors on Save
                // ignored
            }
        }

        public static void ExportCsvMapped(this Searcher searcher, Document document, string filePath)
        {
            var table = new DataTable();
            table.Columns.Add("FileName");
            table.Columns.Add("SearcherName");
            table.Columns.Add("ElementName");
            table.Columns.Add("ElementGuid");
            foreach (var pair in searcher.Pairs)
            {
                table.Columns.Add($"{pair.ColumnName}");
            }

            var data = searcher.GetData(document);
            foreach (var elementData in data.ElementsData)
            {
                var row = table.NewRow();
                row["FileName"] = document.CurrentFileName;
                row["SearcherName"] = searcher.Name;
                row["ElementName"] = elementData.ElementName;
                row["ElementGuid"] = elementData.ElementGuid;
                foreach (var property in elementData.Properties)
                {
                    row[property.ColumnName] = property.Value;
                }

                table.Rows.Add(row);
            }

            try
            {
                table.ToCsv(filePath);
            }
            catch (Exception e)
            {
                //TODO: Handle Errors on Save
                // ignored
            }
        }

        public static void ExportCsvMappedCombined(this IEnumerable<Searcher> searchers, Document document, string filePath = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    var dialog = new SaveFileDialog()
                    {
                        Filter = "CSV Files (*.csv)|*.csv",
                        FileName = ""
                    };

                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    filePath = dialog.FileName;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            var table = new DataTable();
            table.Columns.Add("FileName");
            table.Columns.Add("SearcherName");
            table.Columns.Add("ElementName");
            table.Columns.Add("ElementGuid");
            foreach (var searcher in searchers)
            {
                foreach (var pair in searcher.Pairs)
                {
                    if (table.Columns.Contains(pair.ColumnName))
                    {
                        continue;
                    }
                    table.Columns.Add($"{pair.ColumnName}");
                }
            }

            foreach (var searcher in searchers)
            {
                var data = searcher.GetData(document);
                foreach (var elementData in data.ElementsData)
                {
                    var row = table.NewRow();
                    row["FileName"] = document.CurrentFileName;
                    row["SearcherName"] = searcher.Name;
                    row["ElementName"] = elementData.ElementName;
                    row["ElementGuid"] = elementData.ElementGuid;
                    foreach (var property in elementData.Properties)
                    {
                        row[$"{property.ColumnName}"] = property.Value;
                    }

                    table.Rows.Add(row);
                }
            }

            try
            {
                table.ToCsv(filePath);
            }
            catch (Exception e)
            {
                //TODO: Handle Errors on Save
                // ignored
            }
        }

        public static void ExportJson(this IEnumerable<Searcher> searchers, Document document, string folder = "")
        {
            try
            {
                if (string.IsNullOrEmpty(folder))
                {
                    var dialog = new FolderBrowserDialog()
                    {
                        ShowNewFolderButton = true,
                    };
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    folder = dialog.SelectedPath;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            foreach (var searcher in searchers)
            {
                var file = Path.GetFileNameWithoutExtension(document.CurrentFileName);
                var fileName = $"{file}_{searcher.Name}.json";
                var filePath = Path.Combine(folder, fileName);
                searcher.ExportJson(document, filePath);
            }
        }

        public static void ExportJson(this Searcher searcher, Document document, string filePath)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(searcher.GetData(document), Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception e)
            {
                //TODO: Handle Errors on Save
                // ignored
            }
        }
    }
}