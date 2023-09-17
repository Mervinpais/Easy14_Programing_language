using Newtonsoft.Json.Linq;

namespace LIM_package_manager.AppFunctions
{
    public static class ProgressBar
    {
        static int Min = 0;
        static int Max = 100;
        public static int Current = 0;

        static int CursorYPos = 0;

        public static void Show(int value)
        {
            Current = value;
            CursorYPos = Console.GetCursorPosition().Top + 1;
            Console.SetCursorPosition(0, CursorYPos);
            string progrssBarIncrements = "";
            for (int i = 0; i < Math.Round((decimal)Current / 10); i++)
            {
                progrssBarIncrements += "#";
            }
            for (int i = 0; i < ((Max / 10) - Math.Round((decimal)Current / 10)); i++)
            {
                progrssBarIncrements += " ";
            }
            Console.WriteLine($"[{progrssBarIncrements}]");
        }

        public static void Update(int value)
        {
            Current = value;
            CursorYPos = Console.GetCursorPosition().Top - 1;
            Console.SetCursorPosition(0, CursorYPos);
            string progrssBarIncrements = "";
            for (int i = 0; i < Math.Round((decimal)Current / 10); i++)
            {
                progrssBarIncrements += "#";
            }
            for (int i = 0; i < ((Max / 10) - Math.Round((decimal)Current / 10)); i++)
            {
                progrssBarIncrements += " ";
            }
            Console.WriteLine($"[{progrssBarIncrements}]");
        }
        public static void Clear()
        {
            CursorYPos = Console.GetCursorPosition().Top - 1;
            Console.SetCursorPosition(0, CursorYPos);
            Console.Write("                                ");
            Console.SetCursorPosition(0, CursorYPos);

            Current = 0;
        }
    }
}
