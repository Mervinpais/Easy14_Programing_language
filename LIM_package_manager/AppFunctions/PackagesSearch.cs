namespace LIM_package_manager.AppFunctions
{
    public static class PackagesSearch
    {
        public static void Search()
        {
            // Get the list of packages in the specified directory
            string packagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
            List<string> packages = Directory.GetDirectories(packagesPath).ToList();

            // Filter packages based on the search query
            Console.Write("Enter the Name of the package you want to find >");
            string? searchQuery = Console.ReadLine();

            List<string> matchingPackages = new();
            foreach (var package in packages)
            {
                if (package.ToLower().Contains(searchQuery.ToLower()))
                {
                    matchingPackages.Add(package);
                }
            }
            Console.WriteLine();

            int yPosCursor = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(0, yPosCursor);

            // Display the matching packages
            Console.ForegroundColor = ConsoleColor.Cyan;
            List<string> packageNames = new();

            for (int i = 0; i < matchingPackages.Count; i++)
            {
                packageNames.Add($"{i + 1}. {matchingPackages[i].Substring(matchingPackages[i].LastIndexOf("\\")).Trim()}");
            }

            Console.WriteLine(string.Join(Environment.NewLine, packageNames));
            Console.ResetColor();
        }
    }
}
