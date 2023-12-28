using LIM_package_manager.AppFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LIM_package_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LIM Package Manager ---\n");

            while (true)
            {
                Console.Write($"{Environment.NewLine}>>> ");
                string command = "";
                if (args.Length > 0) { command = string.Join(" ", args); }
                else { command = Console.ReadLine(); }
                HandleCommand(command);
            }
        }

        static void HandleCommand(string command)
        {
            (string header, string command, string[] parameters) values = new("", "", new string[] { });
            values.header = command.Split(' ')[0];
            values.command = command.Split(' ')[1];
            values.parameters = command.Split(' ')[2..];

            switch (values.command.ToLower())
            {
                case "exit":
                    Environment.Exit(0);
                    break;

                case "install":
                case "update":
                    bool isLocal = values.parameters.Contains("--local");
                    bool isUpdate = values.command.ToLower() == "update";
                    _ = PackageInstall.Install(values.parameters.ToList(), isLocal, isUpdate);
                    break;

                case "search":
                    PackagesSearch.Search();
                    break;

                case "uninstall":
                case "remove":
                    PackageUninstall.Uninstall(values.parameters.ToList());
                    break;

                case "list":
                    PackagesList.List();
                    break;

                case "make":
                    CreateAndSavePackage();
                    break;

                case "help":
                    DisplayHelpContent();
                    break;

                default:
                    Console.WriteLine($"Unknown command method {values.command}");
                    break;
            }
        }

        static void CreateAndSavePackage()
        {
            Tuple<string[], string> package = PackageMaker.Make();
            List<string> packageContent = package.Item1.ToList();
            string[] lines = packageContent.ToArray();
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
            File.WriteAllLines(Path.Combine(appDataPath, $"{package.Item2}_Package_File.txt"), lines);
        }

        static void DisplayHelpContent()
        {
            Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
        }
    }
}
