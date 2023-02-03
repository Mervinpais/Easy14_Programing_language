using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public static class ErrorReportor
    {
        public static readonly int EASY14_PROGRAM_ERROR = 0x01;
        public static readonly int EASY14_CSHARP_ERROR = 0x02;
        public static readonly int EASY14_LOADING_ERROR = 0x01;
        public static readonly int EASY14_IO_FILE_ERROR = 0xF1;
        public static readonly int EASY14_UNKNOWN_ERROR = -0x01;
        public static readonly int EASY14_MAIN_PROGRAM_FILE_ERROR = 0x10;

        public static class Logger
        {
            /// <summary>
            /// From The Logger Class, Used to Report errors to the Debug Log
            /// </summary>
            /// <param name="exception">An Exception to get info about the exception</param>
            /// <param name="message">A Custom Message to give extra info (If Needed) about an error</param>
            /// <param name="errCode">The Main Error Code for Report specific errors (1 = Program, 2 = C#, -1 = Unknown, 1x = Error in the program.cs file)</param>
            /// <param name="posErrCode">place where the error occured (1 = Top of file, 2 = In Between, 3 = End of file)</param>
            public static void Error(Exception exception, string message = null, int errCode = 0x00)
            {
                if (errCode != 0x00)
                {
                    Debug.Write("Error Infomation; ");
                    switch (errCode)
                    {
                        case 0x01:
                            Debug.Write("Program Error");
                            break;
                        case 0x02:
                            Debug.Write("C# Error");
                            break;
                        case 0x03:
                            Debug.Write("Loading Error");
                            break;
                        case 0xF1:
                            Debug.Write("File Error");
                            break;
                        case -0x01:
                            Debug.Write("Unknown Error");
                            break;
                        case 0x10:
                            Debug.Write("Error Occured in main program file");
                            break;
                        default:
                            Debug.Write("Unknown Error Code");
                            break;
                    }
                }
                Debug.WriteLine($"\nError: \n{exception.Message}");
                if (message != null)
                {
                    Debug.WriteLine(message);
                }
            }

            public static void Warning(Exception ex, string message = null)
            {
                Debug.WriteLine($"\nWarning: \n{ex.Message}");
                Debug.WriteLine(message);
            }

            public static void Message(Exception ex, string message = null)
            {
                Debug.WriteLine("\nMessage: \n" + ex.ToString());
                Debug.WriteLine(message);
            }
        }

        public static class ConsoleLineReporter
        {
            public static void changeColor(string consoleColor)
            {
                ConsoleColor ConsoleColor_ = ConsoleColor.White;
                try
                {
                    if (consoleColor == "")
                    {
                        Console.ForegroundColor = ConsoleColor_;
                        return;
                    }
                    ConsoleColor_ = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), consoleColor, true);
                }
                catch (Exception ex)
                {
                    ConsoleColor_ = ConsoleColor.Gray;
                    Debug.WriteLine("Color Value invalid, Value;\"" + Convert.ToString(consoleColor) + "\", Error Occured in ErrorLogger");
                    ErrorReportor.Logger.Error(ex);
                }
                Console.ForegroundColor = ConsoleColor_;
            }
            public static void CSharpError(string Title, string Message)
            {
                changeColor("Red");
                Console.WriteLine("C# Exception Error Occured! Please report this bug to the Github repo so that this can be fixed!!\n\n".ToUpper() + Message, Title);
                changeColor("");
            }
            public static void CSharpError(string Message)
            {
                changeColor("Red");
                Console.WriteLine("C# Exception Error Occured! Please report this bug to the Github repo so that this can be fixed!!\n\n".ToUpper() + Message);
                changeColor("");
            }

            public static void Error(string Title, string Message)
            {
                changeColor("Red");
                Console.WriteLine("Easy14 Error; \n\n".ToUpper() + Message.ToString(), Title);
                changeColor("");
            }
            public static void Error(string Message)
            {
                changeColor("Red");
                Console.WriteLine("Easy14 Error; \n\n".ToUpper() + Message.ToString());
                changeColor("");
            }

            public static void Warning(string Title, string Message)
            {
                changeColor("Yellow");
                Console.WriteLine("Easy14 Warning; \n\n" + Message.ToString(), Title);
                changeColor("");
            }
            public static void Warning(string Message)
            {
                changeColor("Yellow");
                Console.WriteLine("Easy14 Warning; \n\n" + Message.ToString());
                changeColor("");
            }

            public static void Message(string Title, string Message)
            {
                changeColor("Grey");
                Console.WriteLine("Easy14 Message; \n\n" + Message.ToString(), Title);
                changeColor("");
            }
            public static void Message(string Message)
            {
                changeColor("Grey");
                Console.WriteLine("Easy14 Message; \n\n" + Message.ToString());
                changeColor("");
            }
        }

        public static class MessageBoxReporter
        {
            public static void CSharpError(string Title, string Message)
            {
                MessageBox.Show("C# Exception Error Occured! Please report this bug to the Github repo so that this can be fixed!!\n\n".ToUpper() + Message, Title);
            }
            public static void CSharpError(string Message)
            {
                MessageBox.Show("C# Exception Error Occured! Please report this bug to the Github repo so that this can be fixed!!\n\n".ToUpper() + Message);
            }
            public static void Error(string Title, string Message)
            {
                MessageBox.Show("An Error Occured! \n\n".ToUpper() + Message, Title);
            }
            public static void Error(string Message)
            {
                MessageBox.Show("An Error Occured! \n\n".ToUpper() + Message);
            }
            public static void Warning(string Message)
            {
                MessageBox.Show("C# Warning!!\n\n" + Message);
            }
            public static void Warning(string Title, string Message)
            {
                MessageBox.Show("C# Warning!!\n\n" + Message, Title);
            }
        }
    }
}