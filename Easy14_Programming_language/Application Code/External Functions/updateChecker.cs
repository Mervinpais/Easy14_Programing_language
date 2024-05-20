using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class UpdateChecker
    {
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string version = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executingAssemblyPath).FullName).FullName).FullName).FullName, ".git", "HEAD");

        public static void CheckLatestVersion()
        {
            return;
            string currentVersion = "v0";

            if (File.ReadAllLines(version).Length > 0) currentVersion = File.ReadAllLines(version)[0].Split("/")[2]; // Read the first line

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

                    // Extract the main version (vX) and patch version (pX) from both versions
                    string[] currentVersionParts = currentVersion.Split('-');
                    string[] latestVersionParts = latestVersion.Split('-');

                    string currentMainVersion = currentVersionParts[0].Trim();
                    string latestMainVersion = latestVersionParts[0].Trim();

                    string currentPatchVersion = "";
                    string latestPatchVersion = "";

                    if (currentVersionParts.Length > 1)  currentPatchVersion = currentVersionParts[1].Trim();

                    if (latestVersionParts.Length > 1)  latestPatchVersion = latestVersionParts[1].Trim();

                    // Compare the main version
                    int mainVersionComparison = string.Compare(currentMainVersion, latestMainVersion);

                    if (mainVersionComparison < 0)
                    {
                        Console.WriteLine($"""
                            A new update is available!
                            Current version: {currentVersion}
                            Latest version: {latestVersion}
                        """);
                        Debugger.Warning("EASY14 Update Message", "Use LIM to install this update");
                    }
                    else if (mainVersionComparison == 0)
                    {
                        // Compare the patch version if it's present in both versions
                        if (!string.IsNullOrEmpty(currentPatchVersion) && !string.IsNullOrEmpty(latestPatchVersion))
                        {
                            int patchVersionComparison = string.Compare(currentPatchVersion, latestPatchVersion);
                            if (patchVersionComparison < 0)
                            {
                                Console.WriteLine("A new patch update is available!");
                                Console.WriteLine($"Current version: {currentVersion}");
                                Console.WriteLine($"Latest version: {latestVersion}");
                                Debugger.Warning("EASY14 Update Message", "Use LIM to install this patch update");
                            }
                            else if (patchVersionComparison == 0)
                            {
                                Console.WriteLine("You have the latest version.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You have the latest version.");
                        }
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
