using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LIM_package_manager.AppFunctions
{
    public static class PackageInstall
    {
        public static async Task Install(List<string> params_, bool isLocalPackage = false, bool isUpdate = false)
        {
            if (params_.Count == 0)
            {
                Console.WriteLine("No packages specified for installation.");
                return;
            }

            int success = 0;
            string packageFilePath = "";

            if (isLocalPackage)
            {
                params_.RemoveAt(0);
                packageFilePath = string.Join(" ", params_);
            }
            else
            {
                string downloadUrl = GetDownloadUrl(params_);
                if (string.IsNullOrEmpty(downloadUrl))
                {
                    return;
                }

                string packagesFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
                string mainPackageFile = Base.SanitizeFileName(Path.GetFileName(downloadUrl));
                string mainPackageFileLocation = Path.Combine(packagesFolderPath, mainPackageFile);

                try
                {
                    // Download the zip file
                    await Base.DownloadFileProgress(downloadUrl, mainPackageFileLocation);
                    packageFilePath = mainPackageFileLocation;
                    success = 1;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during download, extraction, or unpacking
                    success = -1;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex}");
                }
            }

            try
            {
                if (packageFilePath.StartsWith("--"))
                {
                    packageFilePath = packageFilePath.Substring(2);
                }

                UnpackJsonPackage.file = packageFilePath;
                UnpackJsonPackage.Unpack();
                success = 1;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Package was not a .json or was unable to be unpacked, please check the Downloaded folder...");
                Console.WriteLine("Press Enter to continue");
                Console.ReadKey();
                success = 0;
            }

            PrintInstallationStatus(success);
        }

        private static string GetDownloadUrl(List<string> params_)
        {
            foreach (string param in params_)
            {
                if (param.StartsWith("--http"))
                {
                    string downloadUrl = param.Substring(2);
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

                    return downloadUrl;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: No valid download URL provided.");
            return string.Empty;
        }

        private static void PrintInstallationStatus(int success)
        {
            if (success == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("! Package installation finished but some issues occurred during install !");
            }
            else if (success == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Package installation completed successfully :)\r\n");
            }
            else if (success == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("X Package installation failed :(");
            }

            Console.WriteLine("(Press Enter to continue)");
            Console.ReadKey();
        }
    }
}
