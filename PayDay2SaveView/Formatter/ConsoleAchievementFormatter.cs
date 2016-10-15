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
            Console.Write($"# {DisplayName}: ");
            Console.WriteLine(IsAchieved ? "DONE" : "TBD");

            if (!string.IsNullOrWhiteSpace(HeistName))
                Console.WriteLine($"Heist: {HeistName}");

            if (Villain != Villain.None)
                Console.WriteLine($"Villain: {Villain}");

            if (Difficulty.HasValue)
                Console.WriteLine($"Difficulty: {Difficulty.Value}");

            if (!string.IsNullOrWhiteSpace(Description))
                Console.WriteLine($"Description: {Description}");

            Console.WriteLine();
        }
    }
}