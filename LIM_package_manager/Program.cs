using LIM_package_manager.AppFunctions;

namespace LIM_package_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ResetColor();
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
            try
            {
                values.header = command.Split(' ')[0];
                values.command = command.Split(' ')[1];
                values.parameters = command.Split(' ')[2..];
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    string.Join(Environment.NewLine, new string[] {
                    "An Error occured while reading command, please type your command in this format",
                    "   class method --param1 --param2 ...",
                    "last Exception Details can be shown through the command LIM debug --lastException",
                    })
                );
                return;
            }

            if (values.command.ToLower() == "exit")
            {
                Console.WriteLine("!EXITING LIM!");
                Environment.Exit(0);
            }
            else if (values.command.ToLower() == "add")
            {
                bool isLocal = values.parameters.Contains("--local");
                bool isUpdate = values.command.ToLower() == "update";
                _ = PackageInstall.Install(values.parameters.ToList(), isLocal, isUpdate);
            }
            else if (values.command.ToLower() == "search")
            {
                PackagesSearch.Search();
            }
            else if (values.command.ToLower() == "remove")
            {
                PackageUninstall.Uninstall(values.parameters.ToList());
            }
            else if (values.command.ToLower() == "list")
            {
                PackagesList.List();
            }
            else if (values.command.ToLower() == "make")
            {
                CreateAndSavePackage();
            }
            else if (values.command.ToLower() == "help")
            {
                DisplayHelpContent();
            }
            else
            {
                Console.WriteLine($"Unknown command method {values.command}");
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
