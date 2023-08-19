using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIM_package_manager.AppFunctions
{
    public static class PackageUninstall
    {
        public static async Task Uninstall(List<string> params_)
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
                catch
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
