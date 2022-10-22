using System;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public static class CSharpErrorReporter
    {
        public static class ConsoleLineReporter
        {
            public static void changeColor(string consoleColor)
            {
                ConsoleColor ConsoleColor_ = ConsoleColor.White;
                try { ConsoleColor_ = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), consoleColor, true); } catch { ConsoleColor_ = ConsoleColor.Gray; }
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
                Console.WriteLine("An Error Occured! \n\n".ToUpper() + Message, Title);
                changeColor("");
            }
            public static void Error(string Message)
            {
                changeColor("Red");
                Console.WriteLine("An Error Occured! \n\n".ToUpper() + Message);
                changeColor("");
            }
            public static void Warning(string Title, string Message)
            {
                changeColor("Yellow");
                Console.WriteLine("C# Warning!\n\n" + Message, Title);
                changeColor("");
            }
            public static void Warning(string Message)
            {
                changeColor("Yellow");
                Console.WriteLine("C# Warning!\n\n" + Message);
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