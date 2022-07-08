using System.IO.Compression;

namespace LIM_package_manager
{
    class Program
    {
        static void SendError(int errorType, string command)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (errorType == 1)
            {
                Console.WriteLine($"ERROR; Unknown command \"{command}\"");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        public static void DownloadFileAsync(string uri, string outputPath, string nameOfDownload = "<unknown>")
        {
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n===DOWNLOADING {nameOfDownload}===");
            Uri uriResult;
            Console.WriteLine($"    Finding URL to get update from...");
            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            {
                throw new InvalidOperationException("URI is invalid.");
            }
            Console.WriteLine($"    Checking if update.zip (empty) zip exists...");
            if (!File.Exists(outputPath))
            {
                throw new FileNotFoundException("File not found.", nameof(outputPath));
            }
            Console.WriteLine($"    Getting files...");
            byte[] fileBytes = _httpClient.GetByteArrayAsync(uri).Result;
            Console.WriteLine($"    Writing to zip file...");
            File.WriteAllBytes(outputPath, fileBytes);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"====={nameOfDownload} COMPLETE=====\n");
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("(LIM) >>>");
                string command = Console.ReadLine();
                if (command.Split(" ").Length > 1)
                {
                    if (command.Split(" ")[0].ToUpper() == "LIM")
                    {
                        if (command.Split(" ")[1].ToLower() == "install")
                        {
                            try
                            {
                                //Console.Write installing package
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"\nINSTALLING PACKAGE(S).");
                                string download_URL = command.Split(" ")[2];
                                Console.Write(".");
                                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update"))
                                {
                                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update", true);
                                }
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update");
                                Console.Write(".");
                                FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", FileMode.Create);
                                fs.Close();
                                DownloadFileAsync(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", "UPDATE");
                                Console.WriteLine("Please Wait a few seconds while we validate the package.");
                                Thread.Sleep(125);
                                //unzip the update.zip and place it in the Functions folder
                                Console.WriteLine("Unzipping the update.zip...");
                                ZipFile.ExtractToDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\Functions");
                                Console.WriteLine("Unzipping complete.");
                                //delete the update.zip
                                Console.WriteLine("Deleting the update.zip...");
                                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip");
                                Console.WriteLine("Deleting complete.");
                                //delete the update folder
                                Console.WriteLine("Deleting the update folder...");
                                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update", true);
                                Console.WriteLine("Deleting complete.");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nPACKAGE INSTALLATION COMPLETE.");
                                Console.ResetColor();
                            }
                            catch
                            {
                                SendError(1, command);
                            }
                        }
                        else if (command.Split(" ")[1].ToLower() == "uninstall")
                        {
                            string easy14_dir = Directory.GetCurrentDirectory().Replace("\\LIM_package_manager\\bin\\Debug\\net6.0", "");
                            easy14_dir = $"{easy14_dir}\\Easy14_Programming_language\\Functions\\";
                            if (command.Split(" ").Length > 1)
                            {
                                string TheitemToBeUninstalled = easy14_dir + "\\" + command.Split(" ")[2].ToLower();
                                if (Directory.Exists(TheitemToBeUninstalled))
                                {
                                    Console.WriteLine("\nAre you sure you want to uninstall " + command.Split(" ")[2].ToLower() + "? (y/n)");
                                    if (Console.ReadLine() == "y")
                                    {
                                        Console.WriteLine("\nAre you SURE SURE you want to uninstall " + command.Split(" ")[2].ToLower() + "? (y/n)");
                                        if (Console.ReadLine() == "y")
                                        {
                                            Directory.Delete(easy14_dir + "\\" + command.Split(" ")[2].ToLower(), true);
                                            Console.WriteLine("Successfully uninstalled " + command.Split(" ")[2].ToLower());
                                        }
                                        else
                                        {
                                            Console.WriteLine("Uninstallation cancelled");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Uninstallation cancelled");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Can't Uninstall " + command.Split(" ")[2].ToLower() + " because it doesn't exist");
                                }
                            }
                        }
                        else if (command.Split(" ")[1].ToLower() == "update")
                        {
                            if (command.Split(" ")[2].ToLower() == "--update")
                            {
                                string[] command_Arr = command.Split(" ");
                                if (command_Arr.Length > 3)
                                {
                                    if (command.Split(" ")[3].ToLower() == "--lim")
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
                                        DownloadFileAsync(download_URL, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_LIM_Update\\update.zip", "UPDATE");
                                        Console.WriteLine("Please Wait a few seconds while we validate the update.");
                                        Thread.Sleep(500);
                                    }
                                    else if (command.Split(" ")[3].ToLower() == "--easy14")
                                    {
                                        Console.WriteLine("LIM doesn't have the ablity to update the Easy14 language yet");
                                    }
                                }
                            }
                            //Console.WriteLine("LIM doesn't have the ablity to update yet");
                        }
                        else if (command.Split(" ")[1].ToLower() == "help" || command.Split(" ")[1].ToLower() == "?")
                        {
                            string helpTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                            string[] helpTextFile = File.ReadAllLines($"{helpTextFile_str}\\helpContent.txt");
                            Console.WriteLine(string.Join(Environment.NewLine, helpTextFile));
                            Console.WriteLine();
                        }
                        else if (command.Split(" ")[1].ToLower() == "version")
                        {
                            string versionTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                            string[] versionTextFile = File.ReadAllLines($"{versionTextFile_str}\\LIM_version.txt");
                            Console.WriteLine(string.Join(Environment.NewLine, versionTextFile));
                            Console.WriteLine();
                        }
                        else if (command.Split(" ")[1].ToLower() == "list")
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
                            SendError(1, command);
                        }
                    }
                    else if (command.Split(" ")[0].ToLower() == "help" || command.Split(" ")[0].ToLower() == "?")
                    {
                        string helpTextFile_str = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
                        string[] helpTextFile = File.ReadAllLines($"{helpTextFile_str}\\helpContent.txt");
                        Console.WriteLine(string.Join(Environment.NewLine, helpTextFile));
                        Console.WriteLine();
                    }
                    else
                    {
                        SendError(1, command);
                    }
                }
            }
        }
    }
}