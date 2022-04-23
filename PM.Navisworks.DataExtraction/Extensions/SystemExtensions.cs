using System;
using System.IO;

namespace PM.Navisworks.DataExtraction.Extensions
{
    public static class SystemExtensions
    {
        public static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                var ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}