namespace LIM_package_manager.AppFunctions
{
    public static class PackagesList
    {
        public static async Task List()
        {
            List<string> packages = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages")).ToList();
            Console.WriteLine("List of packages installed;");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(string.Join(Environment.NewLine, packages));
            Console.ResetColor();
            return;
        }
    }
}
