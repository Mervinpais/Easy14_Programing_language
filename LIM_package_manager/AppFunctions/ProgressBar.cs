namespace LIM_package_manager.AppFunctions
{
    public static class ProgressBar
    {
        public static int Current = 0;

        static int CursorYPos = 0;

        static int keyFrame = 0;
        static public List<string> spinningSymbols = new()
        { 
            "#"
        };

        public static void Show(string text = "", ConsoleColor color = ConsoleColor.Green)
        {
            CursorYPos = Console.CursorTop;
            Console.SetCursorPosition(0, CursorYPos);

            // Display the text before the progress bar
            Console.Write($"{text}: ");

            int numHashes = (int)Math.Round((decimal)Current / 10);
            int numSpaces = (100 / 10) - numHashes;

            Console.Write("[");
            for (int i = 0; i < numHashes; i++)
            {
                Console.Write("#");
            }

            if (numHashes > 0)
            {
                Console.ForegroundColor = color;
                Console.Write(spinningSymbols[keyFrame]);
                Console.ResetColor();
            }

            keyFrame++;
            if (keyFrame > spinningSymbols.Count - 1)
            {
                keyFrame = 0;
            }
            for (int i = 0; i < numSpaces; i++)
            {
                Console.Write(" ");
            }

            Console.Write("]");
            Console.Write(new string(' ', Console.WindowWidth - 12)); // Adjusted for the length of the progress bar
            Console.SetCursorPosition(0, CursorYPos);
        }

        public static void Update(int value)
        {
            Current = value;
            Show(); // Call the Show method without text to update the progress bar
        }

        public static void Update(int value, string text)
        {
            Current = value;
            Show(text);
        }

        public static void Update(int value, string text, ConsoleColor color)
        {
            Current = value;
            Show(text, color);
        }

        public static void Clear()
        {
            CursorYPos = Console.CursorTop;
            Console.SetCursorPosition(0, CursorYPos);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, CursorYPos);
            Current = 0;
        }
    }
}
