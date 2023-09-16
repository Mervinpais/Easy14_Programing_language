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

            foreach (string requiredPackage in requiredPackages)
            {
                if (!packages.Any(package => package.EndsWith(requiredPackage)))
                {
                    Console.WriteLine($"Required Easy14 Base package '{requiredPackage}' is missing.");
                    // You can take appropriate action here, such as downloading the missing package.
                }
            }

            // Check internet availability
            bool isInternetAvailable = NetworkInterface.GetIsNetworkAvailable();

            if (isInternetAvailable)
            {
                Console.WriteLine("Internet is available.");

                // Get the path to the local Console/Print.cs file
                string localFilePath = Path.Combine(packages.FirstOrDefault(package => package.EndsWith("Console")), "Print.cs");

                // Compare local Console/Print.cs with the GitHub version
                bool areFilesIdentical = await AreFilesIdenticalAsync(localFilePath, "https://raw.githubusercontent.com/Mervinpais/Easy14-BasePackages/main/Console/Print.cs");

                if (areFilesIdentical)
                {
                    Console.WriteLine("The local Console/Print.cs file is identical to the one on GitHub.");
                }
                else
                {
                    Console.WriteLine("The local Console/Print.cs file differs from the one on GitHub.");
                    // You can take appropriate action here, such as downloading the updated file.
                }
            }
            else
            {
                Console.WriteLine("Internet is not available. Cannot perform comparison with GitHub.");
            }

        }
    }
}
