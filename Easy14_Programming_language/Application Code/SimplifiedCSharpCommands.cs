using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Easy14_Programming_Language
{
    internal class SimplifiedCSharpCommands
    {
        /*
        Documentation will prob be made after i finish this;

        But yeah :)

        Hope this will speed up making this language
        */
    }

    public static class Change
    {
        public static void BackgroundColor(ConsoleColor newBackGroundColor)
        {
            Console.BackgroundColor = newBackGroundColor;
        }

        public static void ForegroundColor(ConsoleColor newForeGroundColor)
        {
            Console.ForegroundColor = newForeGroundColor;
        }

        public static void CursorPos(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }
    }
}
