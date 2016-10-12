using System;
using System.Collections.Generic;

namespace PayDay2SaveView
{
    public class XlsFriendlyFormatter : IFormatter
    {
        private int _counterColumnWidth = 11;

        public void Begin()
        {
            Console.Write("\"Normal\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"Hard\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"Very Hard\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"OK\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"EW\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"DW\"".PadLeft(_counterColumnWidth) + " ");
            Console.Write("\"SM\"".PadLeft(_counterColumnWidth) + " ");
            Console.WriteLine("\"Heist\"");
        }

        public void WriteVillainBegin(Villain villain)
        {

        }

        public void End()
        {

        }

        public void WriteHeistName(Heist heist)
        {
            Console.WriteLine('"' + heist.Name + '"');
        }

        public void WriteCounter(int count, Difficulty difficulty, Heist heist)
        {
            Console.Write($"{count.ToString().PadLeft(_counterColumnWidth)} ");
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