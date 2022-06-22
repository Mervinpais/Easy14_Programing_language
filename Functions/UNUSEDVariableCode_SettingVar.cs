using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class UNUSED_VariableCode_SettingVar
    {
        /*

            ████████╗██╗░░██╗██╗░██████╗  ██╗░██████╗  ██╗░░░██╗███╗░░██╗██╗░░░██╗░██████╗███████╗██████╗░██╗
            ╚══██╔══╝██║░░██║██║██╔════╝  ██║██╔════╝  ██║░░░██║████╗░██║██║░░░██║██╔════╝██╔════╝██╔══██╗██║
            ░░░██║░░░███████║██║╚█████╗░  ██║╚█████╗░  ██║░░░██║██╔██╗██║██║░░░██║╚█████╗░█████╗░░██║░░██║██║
            ░░░██║░░░██╔══██║██║░╚═══██╗  ██║░╚═══██╗  ██║░░░██║██║╚████║██║░░░██║░╚═══██╗██╔══╝░░██║░░██║╚═╝
            ░░░██║░░░██║░░██║██║██████╔╝  ██║██████╔╝  ╚██████╔╝██║░╚███║╚██████╔╝██████╔╝███████╗██████╔╝██╗
            ░░░╚═╝░░░╚═╝░░╚═╝╚═╝╚═════╝░  ╚═╝╚═════╝░  ░╚═════╝░╚═╝░░╚══╝░╚═════╝░╚═════╝░╚══════╝╚═════╝░╚═╝

            Sorry but this function is unused till i can fix bugs for it!

        */
        Program prog = new Program();
        private string endOfStatementCode_;
        public string endOfStatementCode
        {
            get { return endOfStatementCode_; }
            set { endOfStatementCode_ = value; }
        }

        public void interperate(string code_part, string[] lines, string varName)
        {
            string code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(3);
            code_part = code_part.TrimStart();
            //code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.StartsWith("Console.input(")) {
                code_part.Replace("Console.input(", "");
                code_part.Substring(0, code_part.Length - 1);
                Console.Write(">");
                string ohGodmyBrainIsHurting = Console.ReadLine();

                string[] files = Directory.GetFiles(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                                + @$"\EASY14_Variables_TEMP"
                        );
                string var_file = null;
                foreach (string file in files)
                {
                    if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == varName)
                    {
                        var_file = file;
                    }
                }
                File.WriteAllText(var_file, ohGodmyBrainIsHurting);
            }
        }
    }
}