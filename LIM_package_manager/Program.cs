using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using static LIM_package_manager.LIM_error_codes;

namespace LIM_package_manager
{
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // Helper method to extract files from a zip archive
        public static void ExtractZip(string zipPath, string extractPath)
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        public static void DownloadFile(string uri, string outputPath, string nameOfDownload = "<unknown>")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n===DOWNLOADING {nameOfDownload}===");

            if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? uriResult))
            {
                throw new InvalidOperationException("URI is invalid.");
            }

            Console.WriteLine($"    Getting files...");
            byte[] fileBytes = _httpClient.GetByteArrayAsync(uriResult).Result;

            Console.WriteLine($"    Writing to zip file...");
            File.WriteAllBytes(outputPath, fileBytes);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"====={nameOfDownload} COMPLETE=====\n");
            Console.ResetColor();
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
                classes = new List<string>(array.Take(array.Length - 1));
            }
            else
            {
                // Case: Parameters provided
                method = array[firstParamIndex - 1];
                params_ = new List<string>(array.Skip(firstParamIndex));
                classes = new List<string>(array.Take(firstParamIndex - 1));
            }

            return (classes, method, params_);
        }

        static void Main()
        {
            Console.WriteLine("=== LIM Package Manager ===\r\n");

            while (true)
            {
                Console.ResetColor();
                Console.Write("\r\n>>> ");
                string command = Console.ReadLine() + "";
                List<string> classes = Parse(command).classes;
                List<string> params_ = Parse(command).params_;
                string method = Parse(command).method;
                /*Console.WriteLine("Classes: " + string.Join(" ", classes));
                Console.WriteLine("Method: " + method);
                Console.WriteLine("Params: " + string.Join(" ", params_));*/

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

                    if (method == "install")
                    {
                        
                    }
                }    
            }

            void InstallCommand(List<string> params_)
            {
                if (params_.Count == 0) { return; }
                string downloadUrl = NULL_URL;
                foreach (string Param in params_)
                {
                    Console.ResetColor();
                    if (Param.StartsWith("--http"))
                    {
                        if (Param.StartsWith("--http://"))
                        {
                            Console.WriteLine($"PACKAGE: {Param.Substring(9)}");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("WARNING: You are installing a package over a HTTP connection. Proceed with caution when downloading packages over HTTP.");
                        }
                        else if (Param.StartsWith("--https://"))
                        {
                            Console.WriteLine($"PACKAGE: {Param.Substring(10)}");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("INFO: You are installing a package with a secured HTTPS connection");
                        }
                    }
                }
                string zipFilePath = "archive.zip";
                string extractionPath = "Packages";

                try
                {
                    // Download the zip file
                    DownloadFile(downloadUrl, zipFilePath, "Example Archive");

                    // Extract the files from the zip archive
                    ExtractZip(zipFilePath, extractionPath);

                    // Cleanup: Delete the downloaded zip file
                    File.Delete(zipFilePath);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during download or extraction
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.ResetColor();
                }

            }
        }
    }
}
