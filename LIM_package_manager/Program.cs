using LIM_package_manager.AppFunctions;

namespace LIM_package_manager
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== LIM Package Manager ===\r\n");
            while (true)
            {
                Console.ResetColor();
                Console.Write("\r\n>>> ");
                string command = Console.ReadLine() + "";
                List<string> classes = Parse(command.Trim()).classes;
                List<string> params_ = Parse(command.Trim()).params_;
                string method = Parse(command.Trim()).method.ToLower();

                if (classes.Count == 0) { continue; }

                if (classes[0].ToLower() == "lim")
                {
                    if (method == "install")
                    {
                        if (params_[0] == "--local")
                        {
                            _ = PackageInstall.Install(params_);
                            continue;
                        }
                        else
                        {
                            _ = PackageInstall.Install(params_);
                            continue;
                        }

                    }
                    else if (method == "uninstall" || method == "remove")
                    {
                        _ = PackageUninstall.Uninstall(params_);
                        continue;
                    }
                    else if (method == "list")
                    {
                        _ = PackagesList.List();
                        continue;
                    }
                    else if (method == "make")
                    {
                        Tuple<string[],string> package = PackageMaker.Make();
                        List<string> packageContent = package.Item1.ToList();
                        string[] lines = packageContent.ToArray();
                        string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");

                        File.WriteAllLines(Path.Combine(appDataPath, package.Item2 + "_Package_File.txt"), lines);
                    }
                    else if (method == "help")
                    {
                        Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
                    }
                }
            }
        }

        public static (List<string> classes, string method, List<string> params_) Parse(string command)
        {
            List<string> classes = new List<string>();
            string method = "";
            List<string> params_ = new List<string>();

            string[] array = command.Split(" ");

            // Find the index of the first element starting with "--"
            int firstParamIndex = Array.FindIndex(array, item => item.StartsWith("--"));

            if (firstParamIndex == -1)
            {
                // Case: No parameters provided
                method = array[array.Length - 1];
                classes = new List<string>(array[..^1]);
            }
            else
            {
                // Case: Parameters provided
                method = array[firstParamIndex - 1];
                params_ = new List<string>(array[firstParamIndex..]);
                classes = new List<string>(array[..(firstParamIndex - 1)]);
            }
            if (command.Contains("--"))
            {
                return (classes, method, params_);
            }
            else
            {
                return (classes, method, new List<string>());
            }
        }
    }
}
