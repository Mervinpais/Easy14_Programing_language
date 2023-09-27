using Newtonsoft.Json;

namespace LIM_package_manager
{
    public static class UnpackJsonPackage
    {
        public static string file = "";

        public static void Unpack()
        {
            if (file == "")
            {
                Console.WriteLine("No File specified");
                return;
            }

            string jsonContent = File.ReadAllText(file);
            PackageData? packageData = JsonConvert.DeserializeObject<PackageData>(jsonContent);
            string packageName = packageData.PackageName;
            List<FileData> files = packageData.Files;
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
            folderPath = Path.Combine(folderPath, packageName);
            Directory.CreateDirectory(folderPath);
            foreach (FileData fileData in files)
            {
                string fileType = fileData.Type;
                string fileName = fileData.FileName;
                string[] fileContent = fileData.Content.Split(@"\r\n");

                // If the file type is "~", create a folder with the given name
                if (fileType == "~")
                {
                    string folderPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages", fileName);
                    Directory.CreateDirectory(folderPath);
                    continue;
                }

                // Otherwise, save the file to the specified folder with the given name and content
                string filePath = Path.Combine(folderPath, fileName);

                if (!filePath.EndsWith(fileType)) filePath = filePath + "." + fileType;
                File.WriteAllLines(filePath, fileContent);
            }
        }

        private class PackageData
        {
            public required string PackageName { get; set; }
            public required List<FileData> Files { get; set; }
        }

        private class FileData
        {
            public required string FileName { get; set; }
            public required string Type { get; set; }
            public required string Content { get; set; }
        }
    }
}
