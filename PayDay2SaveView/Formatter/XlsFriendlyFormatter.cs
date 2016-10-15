using System;
using System.Collections.Generic;

namespace PayDay2SaveView
{
    public class XlsFriendlyFormatter : IFormatter
    {
        public void Begin()
        {
            Console.Write("\"Normal\",");
            Console.Write("\"Hard\",");
            Console.Write("\"Very Hard\",");
            Console.Write("\"Overkill\",");
            Console.Write("\"Mayhem\",");
            Console.Write("\"Death Wish\",");
            Console.Write("\"One Down\",");
            Console.Write("\"Heist\",");
            Console.Write("\"Villain\",");
            Console.Write("\"DLC\"");
            Console.WriteLine();
        }

        public void WriteVillainBegin(Villain villain)
        {

        }

        public void End()
        {

        }

        public void WriteHeistName(Heist heist)
        {
            Console.Write(XlsFriendly(heist.Name) + ',');
        }

        public void WriteHeistIsInDlc(bool inDlc)
        {
            var res = XlsFriendly(inDlc);
            Console.Write(res);
        }

        public void WriteAchievementName(string displayName)
        {
            Console.Write($"{displayName},");
        }

        public void WriteAchievementDescription(string description)
        {
            Console.Write($"{description}");

        }

        public void WriteAchievementEnd()
        {
            Console.WriteLine();
        }

        public void WriteAchievementAchieved(bool achieved)
        {
            Console.Write(XlsFriendly(achieved));
        }

        public void WriteHeistVillain(Villain villain)
        {
            Console.Write(XlsFriendly(EnumUtils.GetString(villain)) + ',');
        }

        public void WriteHeistEnd()
        {
            Console.WriteLine();
        }

        public void WriteCounter(int count, Difficulty difficulty, Heist heist)
        {
            Console.Write($"{count},");
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

        private static string XlsFriendly(string s)
        {
            return string.Concat('"', s, '"');
        }

        private static string XlsFriendly(bool x)
        {
            return x ? "TRUE" : "FALSE";
        }
    }
}