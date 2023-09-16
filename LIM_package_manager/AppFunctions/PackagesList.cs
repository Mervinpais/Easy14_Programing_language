namespace LIM_package_manager.AppFunctions
{
    public static class PackagesList
    {
        public static void List()
        {
            List<string> packages = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages")).ToList();
            Console.WriteLine("List of packages installed;");
            Console.ForegroundColor = ConsoleColor.Cyan;
            List<string> packageNames = new();

            for (int i = 0; i < packages.Count; i++)
            {
                packageNames.Add($"{i+1}. {packages[i].Substring(packages[i].LastIndexOf("\\")).Trim()}");
            }

            Console.WriteLine(string.Join(Environment.NewLine, packageNames));
            Console.ResetColor();
        }
    }
}
