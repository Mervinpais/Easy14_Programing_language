using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;

namespace LIM_package_manager
{
    class Program_OLD
    {
        static void SendError(int errorType, string command) //
        {
            Console.WriteLine("<!> This is the old Program.cs file, please use the new one <!>");

            return;
            /*
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
            Console.WriteLine("<!> This is the old Program.cs file, please use the new one <!>");

            return;

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

        static void OldMain()
        {
            Console.ResetColor();
            Console.WriteLine(" = LIM Package Manager Console =\n");

            Console.WriteLine("<!> This is the old Program.cs file, please use the new one <!>");

            return;
            /*
            while (true)
            {
                Console.Write(">>>");
                string line = "";
                line = Console.ReadLine();

                if (line == null) { line = "<null>"; }

                string[] command_seperated = line.Split(" ");

                if (line != null)
                {
                    if (command_seperated[0].ToUpper() == "LIM")
                    {
                        if (command_seperated.Length > 1)
                        {
                            if (command_seperated[1].ToLower() == "install")
                            {
                                try
                                {
                                    line = line.Split(" ")[2];
                                    //Checks for HTTP/HTTPS string
                                    if (!line.StartsWith("http://") && !line.StartsWith("https://"))
                                    {
                                        throw new LIM_Exceptions.InvalidURL(line);
                                        //Console.WriteLine("\nIt seems that \"" + command + "\" is not a real URL (to download from) and the current operation shall be cancelled");
                                        continue;
                                    }

                                    string download_URL = line;
                                    // download zip file from http://mervin14.epizy.com/data/testDirForEasy14.zip 
                                    //DownloadFileAsync(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", "update.zip");
                                    string DownloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
                                    string? Easy14Directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                                    Easy14Directory = $"{Easy14Directory.Substring(6, Easy14Directory.Length - 43)}\\Easy14_Programming_language\\Functions\\";
                                    string downloadedZIP = download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 1);
                                    string file = Easy14Directory + $"{download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5)}";
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
                                                Directory.Delete(Easy14Directory + $"\\{download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5)}", true);
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

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write($"\nINSTALLING PACKAGE(S).");

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

                                    ZipFile.ExtractToDirectory(DownloadsFolder + downloadedZIP, Easy14Directory + download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5));
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
                                    string runAutoFile = Easy14Directory + download_URL.Substring(download_URL.LastIndexOf("/") + 1, download_URL.Length - download_URL.LastIndexOf("/") - 5) + "\\runAuto.config";
                                    if (File.Exists(runAutoFile))
                                    {
                                        Process.Start(runAutoFile);
                                    }

                                    Console.ResetColor();
                                }
                                catch (Exception)
                                {
                                    throw new LIM_Exceptions.UnknownOrInvalidPackage($"{line.Replace("LIM install ", "")}");
                                    //SendError(0x06, $"{command.Replace("LIM install ", "")}");
                                }
                            }
                            else if (command_seperated[1].ToLower() == "uninstall")
                            {
                                string easy14_dir = Directory.GetCurrentDirectory().Replace("\\LIM_package_manager\\bin\\Debug\\net6.0", "");
                                easy14_dir = $"{easy14_dir}\\Easy14_Programming_language\\Functions\\";
                                if (line.Split(" ").Length > 1)
                                {
                                    string TheitemToBeUninstalled = easy14_dir + "\\" + line.Split(" ")[2].ToLower();
                                    if (Directory.Exists(TheitemToBeUninstalled))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nAre you sure you want to uninstall " + line.Split(" ")[2].ToLower() + "? (y/n)");
                                        Console.Write(">");
                                        if (Console.ReadLine() == "y")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nAre you SURE SURE you want to uninstall " + line.Split(" ")[2].ToLower() + "? (y/n)");
                                            Console.Write(">");
                                            if (Console.ReadLine() == "y")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Directory.Delete(easy14_dir + "\\" + line.Split(" ")[2].ToLower(), true);
                                                Console.WriteLine("Successfully uninstalled " + line.Split(" ")[2].ToLower());
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
                                        Console.WriteLine("Can't Uninstall " + line.Split(" ")[2].ToLower() + " because it doesn't exist");
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
                                try
                                {
                                    string[] helpTextFile = File.ReadAllLines($"helpContent.txt");
                                    Console.WriteLine(string.Join(Environment.NewLine, helpTextFile) + "\n");
                                }
                                catch
                                {
                                    Console.WriteLine("\nUnable to get help file");
                                }
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
                                string[] command_Arr = line.Split(" ");
                                if (command_Arr.Length > 2)
                                {
                                    if (line.Split(" ")[2].ToLower() == "--full")
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
                                throw new LIM_Exceptions.LIM_UnknownError(line);
                                //SendError(0x01, command);
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
                            throw new LIM_Exceptions.LIM_UnknownError(line);
                            //SendError(0x01, command);
                        }
                    }
                    else
                    {

                    }
                }
            }*/
        }
    }
}