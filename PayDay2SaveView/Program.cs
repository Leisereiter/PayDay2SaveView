using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PaydaySaveEditor.PD2;

namespace PayDay2SaveView
{
    public static class Program
    {
        private static CmdLineHelper CmdArgs { get; set; }

        private const int Pd2SteamId = 218620;

        public static void Main(string[] args)
        {
            CmdArgs = new CmdLineHelper();
            CmdArgs.Parse(args);

            if (CmdArgs.IsHelp)
            {
                CmdArgs.PrintHelp(Console.Out);
                return;
            }

            var heistDb = new HeistDb();
            var steamUtils = new SteamUtils();

            var saveFilePath = CmdArgs.Positional.Any() ? CmdArgs.Positional.First() : GetSaveFilePath(steamUtils);
            var saveFile = new SaveFile(saveFilePath);

            if (CmdArgs.IsListUnknownMaps)
            {
                ListUnknownMaps(saveFile, heistDb);
                return;
            }

            if (CmdArgs.IsListSessions)
            {
                ListallSessions(saveFile);
                return;
            }

            var sessions = GetPlayedSessions(saveFile)
                .Select(x => SessionCount.FromDictKvp(x, heistDb))
                .Where(x => x.SessionState == SessionState.Completed)
                .GroupBy(x => x.Heist.Key, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

            Console.Write("NO".PadLeft(4));
            Console.Write("HD".PadLeft(4));
            Console.Write("VH".PadLeft(4));
            Console.Write("OK".PadLeft(4));
            Console.Write("EW".PadLeft(4));
            Console.Write("DW".PadLeft(4));
            Console.Write("SM".PadLeft(4));
            Console.WriteLine("  Heist");

            ShowSessionsPerVillain(sessions, Villain.Bain);
            ShowSessionsPerVillain(sessions, Villain.Classics);
            ShowSessionsPerVillain(sessions, Villain.Events);
            ShowSessionsPerVillain(sessions, Villain.Jimmy);
            ShowSessionsPerVillain(sessions, Villain.Locke);
            ShowSessionsPerVillain(sessions, Villain.TheButcher);
            ShowSessionsPerVillain(sessions, Villain.TheDentist);
            ShowSessionsPerVillain(sessions, Villain.TheElephant);
            ShowSessionsPerVillain(sessions, Villain.Vlad);
            ShowSessionsPerVillain(sessions, Villain.Hector);
        }

        private static void ShowSessionsPerVillain(IDictionary<string, Dictionary<Difficulty, SessionCount>> sessions, Villain villain)
        {
            var heistsToList = HeistDb.JobNames
                .Where(x => x.Value.IsAvailable)
                .Where(x => x.Value.Villain == villain);

            Console.Write("----------------------------- ");
            WriteInColor(() => Console.WriteLine(villain), ConsoleColor.White);

            foreach (var pair in heistsToList.OrderBy(x => x.Value.Name))
            {
                var jobs = sessions.ContainsKey(pair.Key) ? sessions[pair.Key] : null;

                // Console.WriteLine(FormatCountForDifficulty(Difficulty.Easy, jobs));
                PrintCountForDifficulty(Difficulty.Normal, jobs);
                PrintCountForDifficulty(Difficulty.Hard, jobs);
                PrintCountForDifficulty(Difficulty.Overkill, jobs);
                PrintCountForDifficulty(Difficulty.Overkill145, jobs);
                PrintCountForDifficulty(Difficulty.EasyWish, jobs);
                PrintCountForDifficulty(Difficulty.Overkill290, jobs);
                PrintCountForDifficulty(Difficulty.SmWish, jobs);
                WriteInColor(() => Console.WriteLine("  " + pair.Value.Name), GetColorForHeistName(pair.Value));
            }
        }

        private static ConsoleColor GetColorForHeistName(Heist heist)
        {
            //if (heist.IsStealthable) return ConsoleColor.DarkYellow;
            return ConsoleColor.Gray;
        }

        private static void WriteInColor(Action action, ConsoleColor color)
        {
            var backup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = backup;
        }

        private static void ListallSessions(SaveFile saveFile)
        {
            var sessions = GetPlayedSessions(saveFile);
            foreach (var session in sessions)
            {
                Console.WriteLine($"{session.Key} => {session.Value}");
            }
        }

        private static void ListUnknownMaps(SaveFile saveFile, HeistDb heistDb)
        {
            var sessions = GetPlayedSessions(saveFile);
            var counters = sessions.Select(kvp => SessionCount.FromDictKvp(kvp, heistDb));
            ISet<string> allKeys = new SortedSet<string>(counters.Select(x => x.Heist.Key));

            ISet<string> allKnwonKeys = new HashSet<string>();
            foreach (var nameKey in HeistDb.DayNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in HeistDb.EscapeNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in HeistDb.JobNames.Keys) allKnwonKeys.Add(nameKey);

            foreach (var unknownKey in allKeys.Except(allKnwonKeys))
                Console.WriteLine(unknownKey);

            Console.WriteLine("done.");
        }

        private static void PrintCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, SessionCount> jobs)
        {
            var count = jobs != null && jobs.ContainsKey(difficulty) ? jobs[difficulty].Count : 0;
            WriteInColor(() => Console.Write(count.ToString().PadLeft(4)), ColorFromDifficulty(difficulty, ConsoleColor.Gray));
        }

        private static ConsoleColor ColorFromDifficulty(Difficulty difficulty, ConsoleColor defaultColor)
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
                    return ConsoleColor.DarkRed;

                case Difficulty.Easy:
                case Difficulty.Normal:
                    return defaultColor;

                default:
                    return defaultColor;
            }
        }

        private static Dictionary<object, object> GetPlayedSessions(SaveFile saveFile)
        {
            var gameData = saveFile.GameData;
            var statisticsManager = (Dictionary<object, object>)gameData["StatisticsManager"];
            var sessions = (Dictionary<object, object>)statisticsManager["sessions"];
            var jobs = (Dictionary<object, object>)sessions["jobs"];
            return jobs;
        }

        private static string GetSaveFilePath(SteamUtils steamUtils)
        {
            var steamUsers = steamUtils.GetSteamUsersWithGame(Pd2SteamId).ToList();
            if (!steamUsers.Any())
                throw new Exception("Ich finde bei Steam keinen Benutzer der PD2 installiert hat.");

            var steamUser = steamUsers.Count() == 1 ? steamUsers.First() : PromptForSteamUser(steamUsers);

            var payday2SavePath = Path.Combine(steamUtils.GetGameDirectory(steamUser, Pd2SteamId), "remote");
            var currentSaveFile = GetLatestSaveFile(Directory.GetFiles(payday2SavePath, "save*.sav"));
            return currentSaveFile;
        }

        private static long PromptForSteamUser(IList<long> steamUsers)
        {
            Console.WriteLine("Es wurden mehrer Nutzer mit PD2 gefunden:");
            for (var i = 0; i < steamUsers.Count; ++i)
                Console.WriteLine($" [{i + 1}] {steamUsers[i]}");

            var line = Prompt();

            int choice;
            while (true)
            {
                if (int.TryParse(line, out choice) && choice >= 1 && choice <= steamUsers.Count) break;
                Console.WriteLine($"Ne, das muss schon eine Zahl zwischen 1 und {steamUsers.Count} sein.");

                line = Prompt();
            }

            return steamUsers[choice - 1];
        }

        private static string Prompt()
        {
            Console.Write(" > ");
            return Console.ReadLine();
        }

        private static string GetLatestSaveFile(IEnumerable<string> files)
        {
            var pathLastAccessTimes = files.Select(path => new { path, date = File.GetLastWriteTime(path) });
            return pathLastAccessTimes.OrderByDescending(x => x.date).First().path;
        }
    }
}
