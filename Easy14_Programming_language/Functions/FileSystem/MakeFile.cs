using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_MakeFile
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(int lineCount, string[] lines)
        {
            string line = lines[lineCount].Trim();

            if (!File.Exists(line))
            {
                File.Create(line).Close();
            }
        }
    }
}