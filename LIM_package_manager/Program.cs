using LIM_package_manager.AppFunctions;
using System.Reflection;

namespace LIM_package_manager
{
    class Program
    {
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string versionFile = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executingAssemblyPath).FullName).FullName).FullName).FullName, ".git", "HEAD");
        private static readonly string version = File.ReadAllLines(versionFile)[0].Split("/")[2];

        static void Main(string[] args)
        {
            Console.ResetColor();
            Console.WriteLine("LIM Package Manager ---\n");

            while (true)
            {
                Console.Write($"{Environment.NewLine}>>> ");
                string command;
                if (args.Length > 0) { command = string.Join(" ", args); }
                else { command = Console.ReadLine(); }
                HandleCommand(command);
            }
        }

        static void HandleCommand(string command)
        {
            (string command, string[] parameters) values = new("", new string[] { });

            try
            {
                values.command = command.Split(' ')[0];
                values.parameters = command.Split(' ')[1..];
            }
            catch (Exception ex)
            {
                if (!(values.parameters.Length > 0))
                {
                    values.parameters = ["--"];
                }    
            }

            switch (values.command.ToLower())
            {
                case "exit" or "q":
                    Environment.Exit(0);
                    break;

                case "add" or "install":
                    {
                        bool isLocal = values.parameters.Contains("--local");
                        bool isUpdate = values.command.ToLower() == "update";
                        _ = PackageInstall.Install(values.parameters.ToList(), isLocal, isUpdate);
                        break;
                    }
                case "search" or "find" or "fd":
                    PackagesSearch.Search();
                    break;
                case "remove" or "rm":
                    PackageUninstall.Uninstall(values.parameters.ToList());
                    break;
                case "list" or "ls":
                    PackagesList.List();
                    break;
                case "make" or "mk":
                    CreateAndSavePackage();
                    break;
                case "help" or "?":
                    DisplayHelpContent();
                    break;
                default:
                    Console.WriteLine($"Unknown command \'{values.command}\'");
                    break;
            }
        }

        static void CreateAndSavePackage()
        {
            Tuple<string[], string> package = PackageMaker.Make();
            List<string> lines = [$"?PackageVersion = {version}"];
            lines.AddRange(package.Item1.ToList());
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages"
                );

            File.WriteAllLines(Path.Combine(appDataPath, $"{package.Item2}_Package_File.txt"), lines.ToArray());
        }

        static void DisplayHelpContent()
        {
            Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
        }
    }
}
