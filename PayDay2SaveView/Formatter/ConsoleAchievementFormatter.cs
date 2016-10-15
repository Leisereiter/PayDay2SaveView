using System;

namespace PayDay2SaveView.Formatter
{
    public class ConsoleAchievementFormatter : AchievementFormatterBase
    {
        public override void WriteHeader()
        {
        }

        public override void Write()
        {
            ConsoleUtils.WriteInColor(() => Console.Write(IsAchieved ? "DONE" : "TODO"), GetIsAchievedColor(IsAchieved));
            ConsoleUtils.WriteInColor(() => Console.Write($" {DisplayName}"), ConsoleColor.White);

            if (!string.IsNullOrWhiteSpace(HeistName))
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(HeistName), ConsoleColor.Red);
            }

            if (Villain != Villain.None)
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(EnumUtils.GetString(Villain)), ConsoleColor.Yellow);
            }

            if (Difficulty.HasValue)
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(EnumUtils.GetString(Difficulty.Value)), ConsoleColor.Magenta);
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine();
                Console.WriteLine($"     {Description}");
            }

            Console.WriteLine();
        }

        private ConsoleColor GetIsAchievedColor(bool isAchieved)
        {
            return isAchieved ? ConsoleColor.DarkGreen : ConsoleColor.Cyan;
        }
    }
}