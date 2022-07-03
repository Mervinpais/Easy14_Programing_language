using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public class HelpCommandCode
    {
        readonly static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

        public void GetHelp(string function)
        {
            string helpFiles = ((Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName) + "\\Application Code\\HelpCodeContents"); ;
            try
            {
                string helpFile = helpFiles + "\\" + function.ToLower() + ".help";
                Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines(helpFile)));
            }
            catch (FileNotFoundException FileNoFound_Ex)
            {
                string[] helpMessage =
                {
                    "O",
                    " (",
                    "O\n",
                    "Can't Find the requested help file, make sure you typed it in correctly (if you did, dont add \".help\" at the end)",
                    $"Full Exception Message Below;\n\n{FileNoFound_Ex.Message}"
                };
                ExceptionSender exceptionSender = new ExceptionSender();
                exceptionSender.SendException("0xF00001", helpMessage);
            }

            catch (UnauthorizedAccessException UnauthAccessEexception_Ex)
            {
                string[] helpMessage =
                {
                    "O",
                    " (",
                    "O\n",
                    "Can't Acess the requested help file, make sure the file is not in use by another program",
                    $"Full Exception Message Below;\n\n{UnauthAccessEexception_Ex.Message}"
                };
                ExceptionSender exceptionSender = new ExceptionSender();
                exceptionSender.SendException("0xF00005", helpMessage);
            }
            catch (Exception e)
            {
                string[] helpMessage =
                {
                    "O",
                    " (",
                    "O\n",
                    "An Unknown error occurred while getting file info",
                    $"Full Exception Message Below;\n\n{e.Message}"
                };
                ExceptionSender exceptionSender = new ExceptionSender();
                exceptionSender.SendException("0xF00005", helpMessage);
            }
        }

        public void DisplayDefaultHelpOptions()
        {
            string[] defaultHelp = {
                "=== Easy14 Arguments/Commands ===",
                "   -help || /help = Show the list of arguments you can run with the Easy14 Language",
                "",
                "   -help <function> || /help <function> = lets you view help info about a function (you can make custom help guides too!)",
                "",
                "   -run || /run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)",
                "    |",
                "    └-→ show_cmds || /show_cmds = shows what command runs while running a file",
                "    └-→ preview_only || /preview_only = shows the file's content's",
                "",
                "   -intro || /intro = Introduction/Tutorial of Easy14",
                "\n",
                "     <More Commands Comming Soon>"
            };

            Console.WriteLine(string.Join(Environment.NewLine, defaultHelp));
        }
    }
}