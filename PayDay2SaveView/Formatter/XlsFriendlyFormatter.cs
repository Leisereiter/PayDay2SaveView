using System.Collections.Generic;
using System.IO;
using PayDay2SaveView.Entities;
using PayDay2SaveView.Utils;

namespace PayDay2SaveView.Formatter
{
    public class XlsFriendlyFormatter : IFormatter
    {
        private TextWriter Writer { get; }

        public XlsFriendlyFormatter(TextWriter writer)
        {
            Writer = writer;
        }

        public void Begin()
        {
            Writer.Write("\"Normal\",");
            Writer.Write("\"Hard\",");
            Writer.Write("\"Very Hard\",");
            Writer.Write("\"Overkill\",");
            Writer.Write("\"Mayhem\",");
            Writer.Write("\"Death Wish\",");
            Writer.Write("\"One Down\",");
            Writer.Write("\"Heist\",");
            Writer.Write("\"Villain\",");
            Writer.Write("\"DLC\"");
            Writer.WriteLine();
        }

        public void WriteVillainBegin(Villain villain)
        {

        }

        public void End()
        {

        }

        public void WriteHeistName(Heist heist)
        {
            Writer.Write(XlsFriendlyUtils.StringifyString(heist.Name) + ',');
        }

        public void WriteHeistIsInDlc(bool inDlc)
        {
            var res = XlsFriendlyUtils.StringifyBool(inDlc);
            Writer.Write(res);
        }

        public IAchievementFormatter Achievement()
        {
            return new XlsFriendlyAchievementFormatter(Writer);
        }

        public void AchievementsBefore()
        {
            new XlsFriendlyAchievementFormatter(Writer).WriteHeader();
        }

        public void WriteHeistVillain(Villain villain)
        {
            Writer.Write(XlsFriendlyUtils.StringifyString(EnumUtils.GetString(villain)) + ',');
        }

        public void WriteHeistEnd()
        {
            Writer.WriteLine();
        }

        public void WriteCounter(int count, Difficulty difficulty, Heist heist)
        {
            Writer.Write($"{count},");
        }

        public void WriteRawSession(KeyValuePair<object, object> session)
        {

        }

        public void UnknownKeyRaw(string unknownKey)
        {

        }

        public void UnknownKeysEnd()
        {

        }
    }
}