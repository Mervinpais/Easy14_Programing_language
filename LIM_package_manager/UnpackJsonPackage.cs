using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LIM_package_manager
{
    public static class UnpackJsonPackage
    {
        public static void Unpack(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file specified.");
                return;
            }

            string jsonContent = File.ReadAllText(filePath);
            var packageData = JsonConvert.DeserializeObject<PackageData>(jsonContent);

            if (packageData == null)
            {
                Console.WriteLine("Failed to deserialize package data.");
                return;
            }

            string packageName = packageData.PackageName;
            List<FileData> files = packageData.Files;
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages", packageName);

            Directory.CreateDirectory(folderPath);

            foreach (var fileData in files)
            {
                string fileType = fileData.Type;
                string fileName = fileData.FileName;
                string[] fileContent = fileData.Content.Split(new[] { "\\r\\n" }, StringSplitOptions.None);

                // If the file type is "~", create a folder with the given name
                if (fileType == "~")
                {
                    string folderPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages", fileName);
                    Directory.CreateDirectory(folderPath2);
                    continue;
                }

                // Otherwise, save the file to the specified folder with the given name and content
                filePath = Path.Combine(folderPath, fileName);
                if (!filePath.EndsWith(fileType))
                {
                    filePath = filePath + "." + fileType;
                }

                File.WriteAllLines(filePath, fileContent);
            }
        }

        private class PackageData
        {
            public string PackageName { get; set; } = string.Empty;
            public List<FileData> Files { get; set; } = new List<FileData>();
        }

        private class FileData
        {
            public string FileName { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
    }
}
