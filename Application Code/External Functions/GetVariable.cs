using System;
// Important Stuff/namespaces
using System.IO;

namespace Easy14_Programming_Language
{
    public static class GetVariable
    {
        /// <summary>
        /// It finds the variable name.
        /// </summary>
        /// <param name="variableName">The name of the variable you want to find.</param>
        public static string findVar(string variableName)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
            /* A default value for the variable. */
            string theVariable = "0xF000001";

            if (!Directory.Exists(dir))
            {
                return theVariable;
            }
            /* Getting the variable from the file. */
            foreach (string file in Directory.GetFiles(dir))
            {
                if (file.Substring(0, file.Length - 4).EndsWith(variableName))
                {
                    theVariable = File.ReadAllText(file);
                }
            }
            return theVariable;
        }
    }
}
