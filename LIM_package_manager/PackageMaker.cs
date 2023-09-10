using Newtonsoft.Json;

namespace LIM_package_manager
{
    public class Package
    {
        public string PackageName = "YourPackageName";
        public List<FileEntry> Files = new List<FileEntry>();

        public class FileEntry
        {
            public string Type { get; set; }
            public string FileName { get; set; }
            public string Content { get; set; }
        }
    }

    public static class PackageMaker
    {
        public static Tuple<string[], string> Make()
        {
            Package package = new Package();

            Console.Write("\n(1/2) Enter a name for your package/library> ");
            string userPackageName = Console.ReadLine();
            package.PackageName = userPackageName;

            Console.Write("\n(2/2) Enter the package directory, Where all the package files are> ");
            string userPackageDir = Console.ReadLine();

            if (userPackageDir == "" && userPackageName == "")
            {
                Console.WriteLine("No package was created");
            }
            if (!Directory.Exists(userPackageDir))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{userPackageDir} is not a valid directory!");
                Console.ResetColor();
                return new(new string[] { }, "");
            }

            List<string> jsonStrings = new List<string>
            {
                $"{{\r\n  \"packageName\": \"{userPackageName}\",\r\n  \"files\": ["
            };
            foreach (string filePath in Directory.GetFiles(userPackageDir))
            {
                string fileName = Path.GetFileName(filePath);
                string fileContent = File.ReadAllText(filePath);

                string extension = Path.GetExtension(filePath).TrimStart('.');
                string fileType = extension.ToLower();

                Package.FileEntry fileEntry = new Package.FileEntry
                {
                    Type = fileType,
                    FileName = fileName,
                    Content = fileContent
                };

                string jsonString = JsonConvert.SerializeObject(fileEntry, Formatting.Indented);
                jsonStrings.Add(jsonString);
            }

            jsonStrings.Add("]\r\n}");
            return new Tuple<string[], string>(jsonStrings.ToArray(), userPackageName);
        }
    }
}
