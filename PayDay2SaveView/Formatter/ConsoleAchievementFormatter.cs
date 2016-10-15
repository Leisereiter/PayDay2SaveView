using System;

namespace PayDay2SaveView.Formatter
{
    public class ConsoleAchievementFormatter : AchievementFormatterBase
    {
        public override void Write()
        {
            Console.Write($"# {DisplayName}: ");
            Console.WriteLine(IsAchieved ? "DONE" : "TBD");
            Console.WriteLine(Description);
            Console.WriteLine();
        }
    }
}