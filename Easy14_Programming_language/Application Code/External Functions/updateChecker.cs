using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class UpdateChecker
    {
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string version = Path.Combine(executingAssemblyPath, "Application Code", "currentVersion.txt");
        public static void CheckLatestVersion()
        {
            string currentVersion = "V0 - Unknown";

            if (File.ReadAllLines(version).Length > 0) currentVersion = File.ReadAllLines(version)[1];

            string exeLocation = Assembly.GetExecutingAssembly().Location;
            string workingDirectory = Path.GetDirectoryName(exeLocation);

            // Replace with the URL of your version file hosted online
            string versionUrl = "https://pastebin.com/raw/nETTM1ih";

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string latestVersion = webClient.DownloadString(versionUrl);

                    if (latestVersion == "test")
                    { return; }
                    if (latestVersion != currentVersion)
                    {
                        Console.WriteLine("A new update is available!");
                        Console.WriteLine($"Current version: {currentVersion}");
                        Console.WriteLine($"Latest version: {latestVersion}");
                        ErrorReportor.ConsoleLineReporter.Message("Use LIM to install this update");
                    }
                    else
                    {
                        Console.WriteLine("You have the latest version.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for updates: {ex.Message}");
            }
        }
    }
}
