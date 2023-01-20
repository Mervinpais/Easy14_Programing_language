using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public static class CSharpErrorReporter
    {

        public static class Logger
        {
            public static void Error(Exception ex, string message = null)
            {
                Debug.WriteLine($"\nError: \n{ex.Message}");
                Debug.WriteLine(message);
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
                        //return;
                    }
                    ConsoleColor_ = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), consoleColor, true);
                }
                catch (Exception ex)
                {
                    ConsoleColor_ = ConsoleColor.Gray;
                    Debug.WriteLine("Color Value invalid, Value;\"" + Convert.ToString(consoleColor) + "\", Error Occured in CSharpErrorReporter");
                    CSharpErrorReporter.Logger.Error(ex);
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