using System;

namespace PayDay2SaveView.Formatter
{
    public class XlsFriendlyAchievementFormatter : AchievementFormatterBase
    {
        public override void Write()
        {
            string[] values = { IsAchieved.ToString(), DisplayName, Description };
            Console.WriteLine(string.Join(",", values));
        }
    }
}