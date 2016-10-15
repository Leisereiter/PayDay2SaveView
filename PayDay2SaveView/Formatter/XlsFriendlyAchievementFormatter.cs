using System;
using System.Linq;
using PayDay2SaveView.Utils;

namespace PayDay2SaveView.Formatter
{
    public class XlsFriendlyAchievementFormatter : AchievementFormatterBase
    {
        public override void WriteHeader()
        {
            WriteLine("Achieved?", "Name", "Description");
        }

        public override void Write()
        {
            WriteLine(IsAchieved, DisplayName, Description);
        }

        private static void WriteLine(params object[] values)
        {
            Console.WriteLine(string.Join(",", values.Select(XlsFriendlyUtils.Stringify)));
        }
    }
}