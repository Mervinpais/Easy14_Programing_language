using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint //OLD
    {
        static readonly string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static readonly string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void Interperate(string line)
        {
            Console.WriteLine(line);

            //Anything else...
            /*
            Program prog = new Program();
            Console.WriteLine(prog.CompileCode_fromOtherFiles(textArray: new string[] { statement_line }));
            return; */
        }
    }
}