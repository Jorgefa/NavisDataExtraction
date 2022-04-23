using System;
using System.Data;
using System.IO;

namespace PM.Navisworks.DataExtraction.Extensions
{
    public static class DataTableExtensions
    {
        public static void ToCsv(this DataTable dtDataTable, string strFilePath, string separator = "\t")
        {

            var sw = new StreamWriter(strFilePath, false);
            WriteHeaders(dtDataTable, separator, sw);
            sw.Write(sw.NewLine);
            WriteRows(dtDataTable, separator, sw);
            sw.Close();
        }

        private static void WriteRows(DataTable dtDataTable, string separator, StreamWriter sw)
        {
            foreach (DataRow dr in dtDataTable.Rows)
            {
                WriteRow(dtDataTable, separator, sw, dr);

                sw.Write(sw.NewLine);
            }
        }

        private static void WriteRow(DataTable dtDataTable, string separator, StreamWriter sw, DataRow dr)
        {
            for (var i = 0; i < dtDataTable.Columns.Count; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    var value = dr[i].ToString();
                    if (value.Contains(separator))
                    {
                        value = $"\"{value}\"";
                        sw.Write(value);
                    }
                    else
                    {
                        sw.Write(dr[i].ToString());
                    }
                }

                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(separator);
                }
            }
        }

        private static void WriteHeaders(DataTable dtDataTable, string separator, StreamWriter sw)
        {
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(separator);
                }
            }
        }
    }
}