using System;
using System.Collections.Generic;

namespace PayDay2SaveView
{
    public interface IFormatter
    {
        void Begin();
        void WriteVillainBegin(Villain villain);
        void End();
        void WriteHeistName(Heist heist);
        void WriteCounter(int count, Difficulty difficulty, Heist heist);
        void WriteRawSession(KeyValuePair<object, object> session);
        void UnknownKeyRaw(string unknownKey);
        void UnknownKeysEnd();
        void WriteHeistVillain(Villain villain);
        void WriteHeistEnd();
        void WriteHeistIsInDlc(bool inDlc);

        void WriteAchievementName(string displayName);
        void WriteAchievementDescription(string description);
        void WriteAchievementEnd();
    }

    public class ConsoleFormatter : IFormatter
    {
        public void Begin()
        {
            Console.Write("NO".PadLeft(4));
            Console.Write("HD".PadLeft(4));
            Console.Write("VH".PadLeft(4));
            Console.Write("OK".PadLeft(4));
            Console.Write("EW".PadLeft(4));
            Console.Write("DW".PadLeft(4));
            Console.Write("SM".PadLeft(4));
            Console.WriteLine("  Heist");
        }

        public void End()
        {

        }

        public void WriteHeistName(Heist heist)
        {
            Console.Write("  ");
            Console.Write(heist.Name);
            if (heist.IsStealthable)
                WriteInColor(() => Console.Write("*"), ConsoleColor.DarkCyan);
        }

        public void WriteHeistVillain(Villain villain)
        {
            // Wird nicht vom ConsoleFormatter ausgegeben
        }

        public void WriteHeistIsInDlc(bool inDlc)
        {
            if (inDlc)
                WriteInColor(() => Console.Write(" (DLC)"), ConsoleColor.DarkYellow);
        }

        public void WriteAchievementName(string displayName)
        {
            WriteInColor(() => Console.WriteLine("# " + displayName), ConsoleColor.White);
        }

        public void WriteAchievementDescription(string description)
        {
            Console.WriteLine(description);
        }

        public void WriteAchievementEnd()
        {
            Console.WriteLine();
        }

        public void WriteHeistEnd()
        {
            Console.WriteLine();
        }

        public void WriteCounter(int count, Difficulty difficulty, Heist heist)
        {
            var color = count > 0 ? ConsoleColor.Gray : ColorFromDifficulty(difficulty, heist, ConsoleColor.Gray);
            WriteInColor(() => Console.Write(count.ToString().PadLeft(4)), color);
        }

        public void WriteRawSession(KeyValuePair<object, object> session)
        {
            Console.WriteLine($"{session.Key} => {session.Value}");
        }

        public void UnknownKeyRaw(string unknownKey)
        {
            Console.WriteLine(unknownKey);
        }

        public void UnknownKeysEnd()
        {
            Console.WriteLine("done.");
        }

        public void WriteVillainBegin(Villain villain)
        {
            Console.Write("----------------------------- ");
            WriteInColor(() => Console.WriteLine(EnumUtils.GetString(villain)), GetVillainNameColor(villain));
        }

        #region Helpers

        private static ConsoleColor ColorFromDifficulty(Difficulty difficulty, Heist heist, ConsoleColor defaultColor)
        {
            switch (difficulty)
            {
                case Difficulty.Hard:
                case Difficulty.Overkill:
                case Difficulty.Overkill145:
                case Difficulty.EasyWish:
                    return ConsoleColor.Red;

                case Difficulty.Overkill290:
                case Difficulty.SmWish:
                    return heist.IsStealthable ? ConsoleColor.Red : ConsoleColor.DarkRed;

                case Difficulty.Easy:
                case Difficulty.Normal:
                    return defaultColor;

                default:
                    return defaultColor;
            }
        }

        private static ConsoleColor GetVillainNameColor(Villain villain)
        {
            return villain == Villain.Unknown ? ConsoleColor.Red : ConsoleColor.White;
        }

        private static void WriteInColor(Action action, ConsoleColor color)
        {
            var backup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = backup;
        }

        #endregion
    }
}