using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Easy14_Programming_Language.Functions
{
    public static class ImportFileStatement
    {
        public static List<string> Interpret(string code)
        {
            code = code.Substring("import".Length).Trim();
            code = code.Substring(0, code.Length - 1).Trim();
            if (File.Exists(code))
            {
                return File.ReadLines(code).ToList();
            }
            else
            {
                Debugger.Error("Importing File Error", $"Error; Importing file \'{code}\' does not exist!");
            }
            return new();
        }
    }
}
