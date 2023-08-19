using System.Text.RegularExpressions;

namespace LIM_package_manager
{
    class Program
    {
        static string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");

        public static string SanitizeFileName(string fileName)
        {
            string invalidCharsRegex = string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
            string sanitizedFileName = Regex.Replace(fileName, invalidCharsRegex, "");
            sanitizedFileName = sanitizedFileName.Replace(":", "-").Replace("~", "-").Replace(".", "_");
            if (sanitizedFileName.EndsWith("_json")) sanitizedFileName = sanitizedFileName.Replace("_json", ".json");

            return sanitizedFileName;
        }
        public static async Task DownloadFile(string url, string saveFilePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Set the "Accept" header to request JSON content
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }
        }



        public static (List<string> classes, string method, List<string> params_) Parse(string command)
        {
            List<string> classes = new List<string>();
            string method = "";
            List<string> params_ = new List<string>();

            string[] array = command.Split(" ");

            // Find the index of the first element starting with "--"
                int firstParamIndex = Array.FindIndex(array, item => item.StartsWith("--"));

                if (firstParamIndex == -1)
                {
                    // Case: No parameters provided
                    method = array[array.Length - 1];
                    classes = new List<string>(array[..^1]);
                }
                else
                {
                    // Case: Parameters provided
                    method = array[firstParamIndex - 1];
                    params_ = new List<string>(array[firstParamIndex..]);
                    classes = new List<string>(array[..(firstParamIndex - 1)]);
                }
            if (command.Contains("--"))
            {
                return (classes, method, params_);
            }
            else
            {
                return (classes, method, new List<string>());
            }
        }

        static void Main()
        {
            Console.WriteLine("=== LIM Package Manager ===\r\n");
            while (true)
            {
                Console.ResetColor();
                Console.Write("\r\n>>> ");
                string command = Console.ReadLine() + "";
                List<string> classes = Parse(command.Trim()).classes;
                List<string> params_ = Parse(command.Trim()).params_;
                string method = Parse(command.Trim()).method;

                if (classes.Count == 0) { continue; }

                if (classes[0] == "LIM")
                {
                    if (method == "q" || method == "quick" || method == "quik" || method == "Kuwq") //lolz
                    {
                        int selected = 0;
                        string[] items = { "Install", "Update", "Remove" };
                        int topOptionInt = Console.GetCursorPosition().Top;

                        while (true)
                        {
                            // Print the menu options
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (i == selected)
                                {
                                    Console.Write("> ");
                                }
                                Console.WriteLine(items[i]);
                            }

                            // Wait for user input
                            ConsoleKeyInfo key = Console.ReadKey(true);

                            // Process the user input
                            if (key.Key == ConsoleKey.UpArrow)
                            {
                                selected--;
                                if (selected < 0)
                                {
                                    selected = items.Length - 1;
                                }
                            }
                            else if (key.Key == ConsoleKey.DownArrow)
                            {
                                selected++;
                                if (selected > items.Length - 1)
                                {
                                    selected = 0;
                                }
                            }
                            else if (key.Key == ConsoleKey.Enter)
                            {
                                if (selected == 0)
                                {
                                    
                                }
                                continue;
                            }

                            // Move the cursor back to the top of the options
                            Console.SetCursorPosition(0, topOptionInt);

                            // Clear the previous options and selector
                            for (int i = 0; i < items.Length; i++)
                            {
                                for (int j = 0; j < items[i].Length + 2; j++) // +2 to cover "> " prefix
                                {
                                    Console.Write(" ");
                                }
                                Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
                            }

                            // Move the cursor back to the top of the options again
                            Console.SetCursorPosition(0, topOptionInt);
                        }
                    }
                    else if (method == "install")
                    {
                        InstallCommand(params_);
                        continue;
                    }
                    else if (method == "uninstall" || method == "remove")
                    {
                        UnInstallCommand(params_);
                        continue;
                    }
                    else if (method == "help")
                    {
                        Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
                    }
                }
            }
        }

        static async Task InstallCommand(List<string> params_)
        {
            if (params_.Count == 0)
            {
                Console.WriteLine("No packages specified for installation.");
                return;
            }

            string downloadUrl = "";
            foreach (string param in params_)
            {
                if (param.StartsWith("--http"))
                {
                    downloadUrl = param.Substring(2);
                    Console.WriteLine($"PACKAGE: {Path.GetFileName(downloadUrl)}");

                    if (param.StartsWith("--https://"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("INFO: You are installing a package with a secured HTTPS connection");
                    }
                    else if (param.StartsWith("--http://"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("WARNING: You are installing a package over an insecure HTTP connection. Proceed with caution when downloading packages over HTTP.");
                    }

                    break;
                }
            }

            if (string.IsNullOrEmpty(downloadUrl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: No valid download URL provided.");
                return;
            }

            string zipFileName = Path.GetFileName(SanitizeFileName(downloadUrl));
            string zipFilePath_d = Path.Combine(appDataFolder, zipFileName);

            int success = 0;
            try
            {
                // Download the zip file
                await DownloadFile(downloadUrl, zipFilePath_d);
                string fp = zipFilePath_d;
                try
                {
                    // Unpack the JSON package
                    UnpackJsonPackage.file = fp;
                    UnpackJsonPackage.Unpack();

                    success = 1;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Package was not a .json, please check Downloaded folder...");
                    do { Console.WriteLine("Press Enter to continue"); } while (Console.ReadKey().Key != ConsoleKey.Enter);
                    success = 0;
                }

                // Cleanup: Delete the downloaded file
                // ... (Same as before)

            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during download, extraction, or unpacking
                success = -1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {ex}");
            }

            if (success == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Package installation finished but some issues occurred during install");
            }
            else if (success == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Package installation completed successfully :)\r\n");
            }
            else if (success == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Package installation failed :(");
            }

            Console.WriteLine("(Pres Enter to continue)");
            return;
        }

        static async Task UnInstallCommand(List<string> params_)
        {
            if (params_.Count == 0)
            {
                Console.WriteLine("No packages specified for removal.");
                return;
            }

            Console.Write($"\nAre you sure you want to remove the following; {Environment.NewLine + string.Join(" ", params_) + Environment.NewLine}(y/n) >");
            
            if (Console.ReadKey().Key == ConsoleKey.N) return;

            List<string> packagesSuccessfullyUninstalled = new List<string>();
            List<string> packagesFailedToUninstalled = new List<string>();
            foreach (string e in params_)
            {
                packagesFailedToUninstalled.Add(e.Trim().Substring(2));
            }

            params_ = new(packagesFailedToUninstalled.ToArray());
            foreach (string param in params_)
            {
                try
                {
                    string folderPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages", param);
                    Directory.Delete(folderPath2, true);
                    packagesSuccessfullyUninstalled.Add(param);
                    packagesFailedToUninstalled.Remove(param);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nERROR; Failed to remove package {param}");
                }
            }

            if (packagesSuccessfullyUninstalled.Count > 0)
            {
                Console.WriteLine($"\nThe following packages were removed successfully; {Environment.NewLine + string.Join(" ", packagesSuccessfullyUninstalled.ToArray()) + Environment.NewLine}");
            }
            if (packagesFailedToUninstalled.Count > 0)
            {
                Console.WriteLine($"\nWARNING; The following packages couldnt be uninstalled; {Environment.NewLine + string.Join(" ", packagesFailedToUninstalled.ToArray()) + Environment.NewLine}");
            }

            return;
        }
    }
}
