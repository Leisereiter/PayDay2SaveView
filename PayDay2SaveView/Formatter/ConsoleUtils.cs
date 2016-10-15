using System;

namespace PayDay2SaveView
{
    public class ConsoleUtils
    {
        public static void WriteInColor(Action action, ConsoleColor color)
        {
            var backup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = backup;
        }
    }
}