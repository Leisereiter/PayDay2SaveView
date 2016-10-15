using System;
using System.Linq;
using PayDay2SaveView.Utils;

namespace PayDay2SaveView.Formatter
{
    public class XlsFriendlyAchievementFormatter : AchievementFormatterBase
    {
        public override void Write()
        {
            object[] values = { IsAchieved, DisplayName, Description };
            Console.WriteLine(string.Join(",", values.Select(XlsFriendlyUtils.Stringify)));
        }
    }
}