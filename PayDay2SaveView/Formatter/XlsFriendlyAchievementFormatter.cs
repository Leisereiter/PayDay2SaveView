using System.IO;
using System.Linq;
using PayDay2SaveView.Utils;

namespace PayDay2SaveView.Formatter
{
    public class XlsFriendlyAchievementFormatter : AchievementFormatterBase
    {
        private TextWriter Writer { get; }

        public XlsFriendlyAchievementFormatter(TextWriter writer)
        {
            Writer = writer;
        }

        public override void WriteHeader()
        {
            WriteLine("Achieved?", "Name", "Heist", "Villain", "Description");
        }

        public override void Write()
        {
            WriteLine(IsAchieved, DisplayName, HeistName, Villain, Description);
        }

        private void WriteLine(params object[] values)
        {
            Writer.WriteLine(string.Join(",", values.Select(XlsFriendlyUtils.Stringify)));
        }
    }
}