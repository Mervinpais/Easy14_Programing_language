using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIM_package_manager.AppFunctions
{
    public static class PackageInstall
    {
        public static async Task Install(List<string> params_, bool islocalPackage = false)
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
            string folderPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
            string zipFileName = Path.GetFileName(Base.SanitizeFileName(downloadUrl));
            string zipFilePath_d = Path.Combine(folderPath2, zipFileName);

            int success = 0;
            string fp = "";
            try
            {
                // Download the zip file
                await Base.DownloadFile(downloadUrl, zipFilePath_d);
                fp = zipFilePath_d;

            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during download, extraction, or unpacking
                success = -1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {ex}");
            }

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

            Console.WriteLine("(Press Enter to continue)");
            return;
        }
    }
}
