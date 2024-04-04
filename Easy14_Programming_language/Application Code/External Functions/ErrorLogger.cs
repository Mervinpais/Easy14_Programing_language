using System;

namespace Easy14_Programming_Language
{
    public class Easy14Exception : Exception
    {
        public Easy14Exception(string message) : base(message) { }
    }

    public static class Debugger
    {
        public enum ErrorLevel
        {
            CSharpError,
            Error,
            Warning,
            Message
        }

        public static void CS_Error(string title = "Error", string message = "Unknown Error")
        {
            ReportError(ErrorLevel.CSharpError, title, message);
        }

        public static void Error(string title = "Error", string message = "Unknown Error")
        {
            ReportError(ErrorLevel.Error, title, message);
        }

        public static void Warning(string title = "Warning", string message = "Unknown Warning")
        {
            ReportError(ErrorLevel.Warning, title, message);
        }

        public static void Message(string title = "Information", string message = "Some Information")
        {
            ReportError(ErrorLevel.Message, title, message);
        }

        private static void ReportError(ErrorLevel errorLevel, string title, string message)
        {
            ConsoleColor textColor = ConsoleColor.White;
            ConsoleColor bgColor = ConsoleColor.Black;

            switch (errorLevel)
            {
                case ErrorLevel.Error:
                    textColor = ConsoleColor.Red;
                    bgColor = ConsoleColor.Black;
                    break;
                case ErrorLevel.CSharpError:
                    textColor = ConsoleColor.Red;
                    bgColor = ConsoleColor.Black;
                    break;
                case ErrorLevel.Warning:
                    textColor = ConsoleColor.Yellow;
                    bgColor = ConsoleColor.Black;
                    break;
                case ErrorLevel.Message:
                    textColor = ConsoleColor.Gray;
                    bgColor = ConsoleColor.Black;
                    break;
            }

            Change.ForegroundColor(textColor);
            Change.BackgroundColor(bgColor);
            Console.WriteLine($"{errorLevel.ToString().ToUpper()}:> {message} \n {title}");

            Console.ResetColor();

            // Throw a custom exception
            if (errorLevel == ErrorLevel.Error)
            {
                //throw new Easy14Exception($"Error: {message}");
                Program.ProgramStatus = Program.Status.CODE_ERROR;
            }
            else if (errorLevel == ErrorLevel.CSharpError)
            {
                //throw new Easy14Exception($"Error: {message}");
                Program.ProgramStatus = Program.Status.CSHARP_ERROR;
            }
        }
    }
}
