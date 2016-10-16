using System;
using System.Collections;
using System.Collections.Generic;

namespace PayDay2SaveView.Formatter
{
    public class ConsoleAchievementFormatter : AchievementFormatterBase
    {
        public override void WriteHeader()
        {
        }

        public override void Write()
        {
            ConsoleUtils.WriteInColor(() => Console.Write(IsAchieved ? "DONE" : "TODO"), GetIsAchievedColor(IsAchieved));
            ConsoleUtils.WriteInColor(() => Console.Write($" {DisplayName}"), ConsoleColor.White);

            if (!string.IsNullOrWhiteSpace(HeistName))
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(HeistName), ConsoleColor.Red);
            }

            if (Villain != Villain.None)
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(EnumUtils.GetString(Villain)), ConsoleColor.Yellow);
            }

            if (Difficulty.HasValue)
            {
                Console.Write(" | ");
                ConsoleUtils.WriteInColor(() => Console.Write(EnumUtils.GetString(Difficulty.Value)), ConsoleColor.Magenta);
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine();

                var words = new Queue<string>(Description.Split(' ', '\n'));
                while (words.Count > 0)
                {
                    var columnsLeft = 75;

                    var row = new List<string>();
                    while (words.Count > 0 && columnsLeft >= words.Peek().Length)
                    {
                        var word = words.Dequeue();
                        row.Add(word);
                        columnsLeft -= word.Length;
                    }

                    ConsoleUtils.WriteInColor(() => Console.Write("   | "), GetIsAchievedColor(IsAchieved));
                    Console.WriteLine(string.Join(" ", row));
                }
            }

            Console.WriteLine();
        }

        private ConsoleColor GetIsAchievedColor(bool isAchieved)
        {
            return isAchieved ? ConsoleColor.DarkGreen : ConsoleColor.Cyan;
        }
    }
}