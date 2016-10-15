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
            Console.Write(IsAchieved ? "DONE" : "TODO");
            Console.Write($" {DisplayName}");

            if (!string.IsNullOrWhiteSpace(HeistName))
                Console.Write($" | {HeistName}");

            if (Villain != Villain.None)
                Console.Write($" | {Villain}");

            if (Difficulty.HasValue)
                Console.Write($" | {Difficulty.Value}");

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine();
                Console.WriteLine($"     {Description}");
            }

            Console.WriteLine();
        }
    }
}