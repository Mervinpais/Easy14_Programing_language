using System.Text.RegularExpressions;

namespace LIM_package_manager.AppFunctions
{
    public static class Base
    {
        //public static string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");

        public static string SanitizeFileName(string fileName)
        {
            string invalidCharsRegex = string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
            string sanitizedFileName = Regex.Replace(fileName, invalidCharsRegex, "");
            sanitizedFileName = sanitizedFileName.Replace(":", "-").Replace("~", "-").Replace(".", "_");
            if (sanitizedFileName.EndsWith("_json")) sanitizedFileName = sanitizedFileName.Replace("_json", ".json");

            return sanitizedFileName;
        }
        public static async Task DownloadFile(string url, string saveFilePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Set the "Accept" header to request JSON content
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }
        }
    }
}
