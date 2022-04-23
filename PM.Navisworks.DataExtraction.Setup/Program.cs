using System;
using System.Collections.Generic;
using WixSharp;
using WixSharp.CommonTasks;
using File = WixSharp.File;

namespace PM.Navisworks.DataExtraction.Setup
{
    internal class Program
    {
        private static readonly DateTime ProjectStartedDate = new DateTime(year: 2022, month: 4, day: 23);

        const string Guid = "2A95EAEE-B7A7-47FC-BDCF-B1874DF6B21A";
        const string ProjectName = "Navisworks Data Exporter";
        const string PackageName = "PM.Navisworks.DataExtraction";
        const string ProjectDescription = "Navisworks Data Exporter";

        //Change This when Building from different PC
        private const string ProjectLocation =
            @"C:\Users\piotr.kulicki\RiderProjects\NavisDataExtraction\PM.Navisworks.DataExtraction";

        private const string AutomationProjectLocation =
            @"C:\Users\piotr.kulicki\RiderProjects\NavisDataExtraction\PM.Navisworks.DataExtraction.Automation";

        public static void Main(string[] args)
        {
            var folders = new Dictionary<string, string>
            {
                { "2018", $@"{ProjectLocation}\bin\x64\Release_2018\net452" },
                { "2020", $@"{ProjectLocation}\bin\x64\Release_2020\net47" },
                { "2021", $@"{ProjectLocation}\bin\x64\Release_2021\net47" },
                { "2022", $@"{ProjectLocation}\bin\x64\Release_2022\net47" }
            };
            var automationFolders = new Dictionary<string, string>
            {
                { "2018", $@"{AutomationProjectLocation}\bin\x64\Release_2018\net452" },
                { "2020", $@"{AutomationProjectLocation}\bin\x64\Release_2020\net47" },
                { "2021", $@"{AutomationProjectLocation}\bin\x64\Release_2021\net47" },
                { "2022", $@"{AutomationProjectLocation}\bin\x64\Release_2022\net47" }
            };

            AutoElements.DisableAutoKeyPath = true;
            var feature = new Feature(ProjectName, true, false);
            var directories = CreateDirectories(feature, folders);
            var automationDirectories = CreateDirectories(feature, automationFolders);
            var dir = new Dir(feature, $@"%AppData%/Autodesk/ApplicationPlugins/{PackageName}.bundle",
                new File(feature, "./PackageContents.xml"),
                new Dir(feature, "Contents")
                {
                    Dirs = directories
                },
                new Dir(feature, "Automation")
                {
                    Dirs = automationDirectories
                });

            var project = new Project(ProjectName, dir)
            {
                Name = ProjectName,
                Description = ProjectDescription,
                OutFileName = ProjectName,
                OutDir = "output",
                Platform = Platform.x64,
                UI = WUI.WixUI_Minimal,
                Version = GetVersion(),
                InstallScope = InstallScope.perUser,
                MajorUpgrade = MajorUpgrade.Default,
                GUID = new Guid(Guid),
                LicenceFile = "./Resources/EULA.rtf",
                ControlPanelInfo =
                {
                    ProductIcon = "./Resources/PM.ico",
                },
                BannerImage = "./Resources/Banner.bmp",
                BackgroundImage = "./Resources/Main.bmp",
            };

            project.AddRegValues(new RegValue(RegistryHive.CurrentUser, $"Software\\PM Group\\{ProjectName}", "Version",
                GetVersion().ToString()));
            project.AddRegValues(new RegValue(RegistryHive.CurrentUser, $"Software\\PM Group\\{ProjectName}", "Guid",
                Guid));
            project.BuildMsi();
        }

        private static Dir[] CreateDirectories(Feature feature, Dictionary<string, string> folders)
        {
            var dirs = new List<Dir>();
            foreach (var folder in folders)
            {
                var dir = new Dir(folder.Key,
                    new Files(feature,
                        $@"{folder.Value}\*.*"));
                dirs.Add(dir);
            }

            return dirs.ToArray();
        }

        private static Version GetVersion()
        {
            const int majorVersion = 0;
            const int minorVersion = 3;
            var daysSinceProjectStarted = (int)((DateTime.UtcNow - ProjectStartedDate).TotalDays);
            var minutesSinceMidnight = (int)DateTime.UtcNow.TimeOfDay.TotalMinutes;
            var version = $"{majorVersion}.{minorVersion}.{daysSinceProjectStarted}.{minutesSinceMidnight}";
            return new Version(version);
        }
    }
}