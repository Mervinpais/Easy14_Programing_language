using LIM_package_manager.AppFunctions;
using System.Net.NetworkInformation;

namespace LIM_package_manager
{
    public static class DetectMissingPackages
    {
        static List<string> packages = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages")).ToList();

        public static async Task<bool> AreFilesIdenticalAsync(string localFilePath, string remoteFileUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Fetch the remote file content
                    string remoteFileContent = await client.GetStringAsync(remoteFileUrl);

                    // Read the local file's content
                    string localFileContent = File.ReadAllText(localFilePath);

                    // Compare the two content strings
                    return string.Equals(localFileContent, remoteFileContent, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., network error, file not found)
                Console.WriteLine($"Error comparing files: {ex.Message}");
                return false;
            }

        }
        public static async Task Easy14StandardLibraryAsync()
        {
            List<string> requiredPackages = new List<string>
            {
                "Audio",
                "Console",
                "FileSystem",
                "Network",
                "Time"
            };

            ProgressBar.Show(0);

            for (int i = 0; i < requiredPackages.Count; i++)
            {
                ProgressBar.Update(i * 20);
                string requiredPackage = requiredPackages[i];
                if (!packages.Any(package => package.EndsWith(requiredPackage)))
                {
                    Console.WriteLine($"Required Easy14 Base package '{requiredPackage}' is missing.");
                    // You can take appropriate action here, such as downloading the missing package.
                }
            }

            ProgressBar.Update(100);
            ProgressBar.Clear();
            // Check internet availability
            bool isInternetAvailable = NetworkInterface.GetIsNetworkAvailable();

            if (isInternetAvailable)
            {
                Console.WriteLine("Internet is available.");

                // List of local file paths
                List<string> localFilePaths = new List<string>
                {
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Print.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Input.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Beep.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Clear.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "New.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Exec.cs"),
                    Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "GetKeyPress.cs"),
                };

                // List of GitHub raw links
                List<string> githubRawLinks = new List<string>
                {
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Print.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Input.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Beep.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Clear.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/New.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Exec.cs",
                    "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/GetKeyPress.cs",
                };

                // List to store files with differences or errors
                List<string> differingFiles = new List<string>();

                // Compare each pair of local files and GitHub links
                for (int i = 0; i < localFilePaths.Count; i++)
                {
                    string localFilePath = localFilePaths[i];
                    string githubRawLink = githubRawLinks[i];

                    bool areFilesIdentical = await AreFilesIdenticalAsync(localFilePath, githubRawLink);

                    if (!areFilesIdentical)
                    {
                        Console.WriteLine($"The local file '{localFilePath}' differs from the one on GitHub.");
                        differingFiles.Add(localFilePath);
                        // You can take appropriate action here, such as downloading the updated file.
                    }
                }

                if (differingFiles.Count == 0)
                {
                    Console.WriteLine("All Base Packages for Easy14 are alright");
                }
                else
                {
                    Console.WriteLine("Differences or errors were found in the following files:");
                    foreach (string differingFile in differingFiles)
                    {
                        Console.WriteLine(differingFile);
                    }
                }
            }
            else
            {
                Console.WriteLine("Internet is not available. Cannot perform comparison with GitHub.");
            }


        }
    }
}
