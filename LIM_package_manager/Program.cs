using LIM_package_manager.AppFunctions;
using System.Text.RegularExpressions;

namespace LIM_package_manager
{
    class Program
    {
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
                string method = Parse(command.Trim()).method;

                if (classes.Count == 0) { continue; }

                if (classes[0] == "LIM")
                {
                    if (method == "q" || method == "quick" || method == "quik" || method == "Kuwq") //lolz
                    {
                        int selected = 0;
                        string[] items = { "Install", "Update", "Remove" };
                        int topOptionInt = Console.GetCursorPosition().Top;

                        while (true)
                        {
                            // Print the menu options
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (i == selected)
                                {
                                    Console.Write("> ");
                                }
                                Console.WriteLine(items[i]);
                            }

                            // Wait for user input
                            ConsoleKeyInfo key = Console.ReadKey(true);

                            // Process the user input
                            if (key.Key == ConsoleKey.UpArrow)
                            {
                                selected--;
                                if (selected < 0)
                                {
                                    selected = items.Length - 1;
                                }
                            }
                            else if (key.Key == ConsoleKey.DownArrow)
                            {
                                selected++;
                                if (selected > items.Length - 1)
                                {
                                    selected = 0;
                                }
                            }
                            else if (key.Key == ConsoleKey.Enter)
                            {
                                if (selected == 0)
                                {
                                    
                                }
                                continue;
                            }

                            // Move the cursor back to the top of the options
                            Console.SetCursorPosition(0, topOptionInt);

                            // Clear the previous options and selector
                            for (int i = 0; i < items.Length; i++)
                            {
                                for (int j = 0; j < items[i].Length + 2; j++) // +2 to cover "> " prefix
                                {
                                    Console.Write(" ");
                                }
                                Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
                            }

                            // Move the cursor back to the top of the options again
                            Console.SetCursorPosition(0, topOptionInt);
                        }
                    }
                    else if (method == "install")
                    {
                        _ = PackageInstall.Install(params_);
                        continue;
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
                    else if (method == "help")
                    {
                        Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines("helpContent.txt")));
                    }
                }
            }
        }
    }
}
