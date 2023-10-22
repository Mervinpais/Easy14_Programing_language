using LIM_package_manager.AppFunctions;
namespace LIM_package_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LIM Package Manager ===\r\n");
            DetectMissingPackages.Easy14StandardLibrary();

            while (true)
            {
                Console.ResetColor();
                Console.Write("\r\n>>> ");
                string command = args.Length > 0 ? string.Join(" ", args) : Console.ReadLine() ?? "";

                (List<string> classes, string method, List<string> parameters) = ParseCommand(command);

                if (classes.Count == 0) continue;

                if (classes[0].ToLower() == "lim")
                {
                    HandleLIMCommand(method, parameters);
                }
                else
                {
                    Console.WriteLine($"Unknown command class {string.Join(" ", classes)}");
                }
            }
        }

        static void HandleLIMCommand(string method, List<string> parameters)
        {
            switch (method.ToLower())
            {
                case "exit":
                    Environment.Exit(0);
                    break;

                case "install":
                case "update":
                    bool isLocal = parameters.Contains("--local");
                    bool isUpdate = method.ToLower() == "update";

                    _ = PackageInstall.Install(parameters, isLocal, isUpdate);
                    break;

                case "search":
                    PackagesSearch.Search();
                    break;

                case "uninstall":
                case "remove":
                    PackageUninstall.Uninstall(parameters);
                    break;

                case "list":
                    PackagesList.List();
                    break;

                case "make":
                    Tuple<string[], string> package = PackageMaker.Make();
                    List<string> packageContent = package.Item1.ToList();
                    string[] lines = packageContent.ToArray();
                    string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
                    File.WriteAllLines(Path.Combine(appDataPath, package.Item2 + "_Package_File.txt"), lines);
                    break;

                case "help":
                    Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
                    break;

                default:
                    Console.WriteLine($"Unknown command method {method}");
                    break;
            }
        }

        static (List<string> classes, string method, List<string> parameters) ParseCommand(string command)
        {
            string[] parts = command.Split(" ");
            List<string> classes = new List<string>();
            List<string> parameters = new List<string>();
            string method = "";

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.StartsWith("--"))
                {
                    parameters.Add(part);
                }
                else if (parts[i+1].StartsWith("--") && (!part.StartsWith("--")))
                {
                    method = part;
                }
                else
                {
                    classes.Add(part);
                }
            }

            return (classes, method, parameters);
        }
    }
}
