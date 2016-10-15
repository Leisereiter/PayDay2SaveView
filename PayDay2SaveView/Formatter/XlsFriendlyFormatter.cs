using System;
using System.Collections.Generic;
using PayDay2SaveView.Formatter;
using PayDay2SaveView.Utils;

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
            Console.Write(XlsFriendlyUtils.Stringify(heist.Name) + ',');
        }

        public void WriteHeistIsInDlc(bool inDlc)
        {
            var res = XlsFriendlyUtils.Stringify(inDlc);
            Console.Write(res);
        }

        public IAchievementFormatter Achievement()
        {
            return new XlsFriendlyAchievementFormatter();
        }

        public void WriteHeistVillain(Villain villain)
        {
            Console.Write(XlsFriendlyUtils.Stringify(EnumUtils.GetString(villain)) + ',');
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
    }
}