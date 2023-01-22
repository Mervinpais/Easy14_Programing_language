using System.Diagnostics;
using System.IO.Compression;

namespace LIM_package_manager
{
    class Program_OLD
    {
        static void SendError(int errorType, string command)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            if (errorType == 0x01)
            {
                Console.WriteLine($"ERROR; Unknown command \"{command}\"");
            }
            else if (errorType == 0x02)
            {
                Console.WriteLine($"ERROR; Can't Download Package; Internet Error \n \"{command}\"");
            }
            else if (errorType == 0x03)
            {
                Console.WriteLine($"ERROR; Can't Download Update; Internet Error \n \"{command}\"");
            }
            else if (errorType == 0x04)
            {
                Console.WriteLine($"ERROR; Can't Download Package; File IO Error \n \"{command}\"");
            }
            else if (errorType == 0x05)
            {
                Console.WriteLine($"ERROR; Can't Download Update; File IO Error \n \"{command}\"");
            }
            else if (errorType == 0x06)
            {
                Console.WriteLine($"ERROR; Can't Download Package Unknown or Invalid Package \n \"{command}\"");
            }
            else if (errorType == 0x08)
            {
                Console.WriteLine($"ERROR; Download Error \n \"{command}\"");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        public static void DownloadFile_Async(string uri, string outputPath, string nameOfDownload = "<unknown>")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n===DOWNLOADING {nameOfDownload}===");
            Console.WriteLine($"    Finding URL to get files from...");
            if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? uriResult))
            {
                throw new InvalidOperationException("URI is invalid.");
            }
            Console.WriteLine($"    Checking if the empty zip exists...");
            if (!File.Exists(outputPath))
            {
                throw new FileNotFoundException("File not found.", nameof(outputPath));
            }
            Console.WriteLine($"    Getting files...");
            byte[] fileBytes = _httpClient.GetByteArrayAsync(uriResult).Result;
            Console.WriteLine($"    Writing to zip file...");
            File.WriteAllBytes(outputPath, fileBytes);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"====={nameOfDownload} COMPLETE=====\n");
            Console.ResetColor();
        }

        static void Main_OLD()
        {
            Console.ResetColor();
            Console.WriteLine(" = LIM Package Manager Console =\n");

            while (true)
            {
                Console.Write("(LIM) >>>");
                string? command = Console.ReadLine();
                if (command != null)
                {
                    string[] command_seperated = command.Split(" ");
                    if (command_seperated[0].ToUpper() == "LIM")
                    {
                        if (command_seperated.Length > 1)
                        {
                            if (command_seperated[1].ToLower() == "install")
                            {
                                try
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write($"\nINSTALLING PACKAGE(S).");
                                    string download_URL = command.Split(" ")[2];
                                    Console.Write(".");
                                    Console.Write(".");
                                    // download zip file from http://mervin14.epizy.com/data/testDirForEasy14.zip 
                                    //DownloadFileAsync(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", "update.zip");
                                    string DownloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
                                    string easy14_dir = Directory.GetCurrentDirectory().Replace("\\LIM_package_manager\\bin\\Debug\\net6.0", "");
                                    easy14_dir = $"{easy14_dir}\\Easy14_Programming_language\\Functions\\";
                                    string downloadedZIP = download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 1);
                                    string file = easy14_dir + $"{download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5)}";
                                    if (Directory.Exists(file))
                                    {
                                        Console.WriteLine("\nIt seems you have already installed \"" +
                                                            download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5) +
                                                            "\".\nDo you want to update(or reinstall) this package? (Enter Y/N)");
                                        Console.Write(">");
                                        Console.ResetColor();
                                        string? userInput = Console.ReadLine();
                                        if (userInput != null)
                                        {
                                            if (userInput.ToLower() == "y")
                                            {
                                                Directory.Delete(easy14_dir + $"\\{download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5)}", true);
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine($"Installation of package {download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5)} CANCELED");
                                                Console.ResetColor();
                                                continue;
                                            }
                                        }
                                    }

                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = download_URL,
                                        UseShellExecute = true
                                    });

                                    while (!File.Exists(DownloadsFolder + downloadedZIP))
                                    {
                                        Console.WriteLine("Waiting for file to finish downloading");
                                        Thread.Sleep(100);
                                    }
                                    //DownloadFileAsync(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip");
                                    Console.WriteLine("Please Wait a few seconds while we validate the package.");
                                    Thread.Sleep(150);
                                    //unzip the update.zip and place it in the Functions folder
                                    Console.WriteLine("Unzipping the " + downloadedZIP + "...");

                                    ZipFile.ExtractToDirectory(DownloadsFolder + downloadedZIP, easy14_dir + download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5));
                                    File.Delete(DownloadsFolder + downloadedZIP);
                                    Console.WriteLine("Unzipping complete.");
                                    //delete the update.zip
                                    Console.WriteLine("Deleting the update.zip...");
                                    Console.WriteLine("Deleting complete.");
                                    //delete the update folder
                                    Console.WriteLine("Deleting the update folder...");
                                    Console.WriteLine("Deleting complete.");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nPACKAGE {download_URL.Substring(download_URL.LastIndexOf(" / ") + 1, download_URL.Length - download_URL.LastIndexOf(" / ") - 5)} INSTALLATION COMPLETE.");
                                    Console.WriteLine($"Checking for runAuto.config...");
                                    string runAutoFile = easy14_dir + download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5) + "\\runAuto.config";
                                    if (File.Exists(runAutoFile))
                                    {
                                        Process.Start(runAutoFile);
                                    }

                                    Console.ResetColor();
                                }
                                catch (Exception e)
                                {
                                    SendError(0x06, command);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(e.Message);
                                    Console.ResetColor();
                                }
                            }
                            else if (command_seperated[1].ToLower() == "uninstall")
                            {
                                string easy14_dir = Directory.GetCurrentDirectory().Replace("\\LIM_package_manager\\bin\\Debug\\net6.0", "");
                                easy14_dir = $"{easy14_dir}\\Easy14_Programming_language\\Functions\\";
                                if (command.Split(" ").Length > 1)
                                {
                                    string TheitemToBeUninstalled = easy14_dir + "\\" + command.Split(" ")[2].ToLower();
                                    if (Directory.Exists(TheitemToBeUninstalled))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nAre you sure you want to uninstall " + command.Split(" ")[2].ToLower() + "? (y/n)");
                                        Console.Write(">");
                                        if (Console.ReadLine() == "y")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nAre you SURE SURE you want to uninstall " + command.Split(" ")[2].ToLower() + "? (y/n)");
                                            Console.Write(">");
                                            if (Console.ReadLine() == "y")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Directory.Delete(easy14_dir + "\\" + command.Split(" ")[2].ToLower(), true);
                                                Console.WriteLine("Successfully uninstalled " + command.Split(" ")[2].ToLower());
                                                Console.ResetColor();
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("Uninstallation cancelled");
                                                Console.ResetColor();
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Uninstallation cancelled");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Can't Uninstall " + command.Split(" ")[2].ToLower() + " because it doesn't exist");
                                        Console.ResetColor();
                                    }
                                }
                            }
                            else if (command_seperated[1].ToLower() == "update")
                            {
                                if (command_seperated.Length > 2)
                                {
                                    if (command_seperated[2].ToLower() == "--lim")
                                    {
                                        //Console.WriteLine("LIM doesn't have the ablity to update yet");
                                        Console.Write("Downloading update.");
                                        string download_URL = "https://github.com/Mervinpais/Easy14_Programing_language/archive/refs/heads/Latest_version.zip";
                                        Console.Write(".");
                                        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update"))
                                        {
                                            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update", true);
                                        }
                                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update");
                                        Console.Write(".");
                                        FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", FileMode.Create);
                                        fs.Close();
                                        DownloadFile_Async(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", "UPDATE");
                                        Console.WriteLine("Please Wait a few seconds while we validate the update.");
                                        Thread.Sleep(500);
                                    }
                                    else if (command_seperated[2].ToLower() == "--easy14")
                                    {
                                        Console.WriteLine("LIM doesn't have the ablity to update the Easy14 language yet");
                                    }
                                }
                                //Console.WriteLine("LIM doesn't have the ablity to update yet");
                            }
                            else if (command_seperated[1].ToLower() == "help" || command_seperated[1].ToLower() == "?")
                            {
                                string helpTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                                string[] helpTextFile = File.ReadAllLines($"{helpTextFile_str}\\helpContent.txt");
                                Console.WriteLine(string.Join(Environment.NewLine, helpTextFile));
                                Console.WriteLine();
                            }
                            else if (command_seperated[1].ToLower() == "version")
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                string versionTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                                if (File.Exists($"{versionTextFile_str}\\LIM_version.txt"))
                                {
                                    string[] versionTextFile = File.ReadAllLines($"{versionTextFile_str}\\LIM_version.txt");
                                    Console.WriteLine(string.Join(Environment.NewLine, versionTextFile));
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Version ?.?.?");
                                }
                                Console.WriteLine();
                                Console.ResetColor();
                            }
                            else if (command_seperated[1].ToLower() == "list")
                            {
                                //Console.WriteLine(Directory.GetCurrentDirectory());
                                string easy14_dir = Directory.GetCurrentDirectory().Replace("\\LIM_package_manager\\bin\\Debug\\net6.0", "");
                                easy14_dir = $"{easy14_dir}\\Easy14_Programming_language\\Functions\\";
                                string[] command_Arr = command.Split(" ");
                                if (command_Arr.Length > 2)
                                {
                                    if (command.Split(" ")[2].ToLower() == "--full")
                                    {
                                        Console.WriteLine(string.Join(Environment.NewLine, Directory.GetDirectories(easy14_dir)));
                                    }
                                }
                                else
                                {
                                    List<string> old_Data = new List<string>(Directory.GetDirectories(easy14_dir));
                                    List<string> new_Data = new List<string>();
                                    foreach (string item in old_Data)
                                    {
                                        new_Data.Add(item.Substring(item.LastIndexOf("\\") + 1));
                                    }
                                    Console.Write("\n===PACKAGES INSTALLED===\n\n");
                                    Console.WriteLine(string.Join(Environment.NewLine, new_Data.ToArray()));
                                    Console.Write("\n========================\n");
                                }
                            }
                            else
                            {
                                SendError(0x01, command);
                            }
                        }
                        else if (command_seperated[0].ToLower() == "help" || command_seperated[0].ToLower() == "?")
                        {
                            string helpTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                            string[] helpTextFile = File.ReadAllLines($"{helpTextFile_str}\\helpContent.txt");
                            Console.WriteLine(string.Join(Environment.NewLine, helpTextFile));
                            Console.WriteLine();
                        }
                        else
                        {
                            SendError(0x01, command);
                        }
                    }
                }
            }
        }
    }
}