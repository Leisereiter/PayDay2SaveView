using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PaydaySaveEditor.PD2;

namespace PayDay2SaveView
{
    public static class Program
    {
        private const int Pd2SteamId = 218620;

        public static void Main(string[] args)
        {
            var context = new Context
            {
                HeistDb = new HeistDb(),
                Args = new CmdLineHelper(args)
            };

            if (context.Args.IsHelp)
            {
                CmdLineHelper.PrintHelp(Console.Out);
                return;
            }

            var steamUtils = new SteamUtils();

            var saveFilePath = context.Args.Positional.Any() ? context.Args.Positional.First() : GetSaveFilePath(steamUtils);
            var saveFile = new SaveFile(saveFilePath);

            if (context.Args.IsListUnknownMaps)
            {
                ListUnknownMaps(context, saveFile);
                return;
            }

            if (context.Args.IsListSessions)
            {
                ListallSessions(saveFile);
                return;
            }

            var sessions = GetPlayedSessions(saveFile)
                .Select(x => SessionCount.FromDictKvp(x, context.HeistDb))
                .GroupBy(x => x.Heist.Key, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.ToList()));

            Console.Write("NO".PadLeft(4));
            Console.Write("HD".PadLeft(4));
            Console.Write("VH".PadLeft(4));
            Console.Write("OK".PadLeft(4));
            Console.Write("EW".PadLeft(4));
            Console.Write("DW".PadLeft(4));
            Console.Write("SM".PadLeft(4));
            Console.WriteLine("  Heist");

            ShowSessionsPerVillain(context, sessions, Villain.Unknown);
            ShowSessionsPerVillain(context, sessions, Villain.Bain);
            ShowSessionsPerVillain(context, sessions, Villain.Classics);
            ShowSessionsPerVillain(context, sessions, Villain.Events);
            ShowSessionsPerVillain(context, sessions, Villain.Hector);
            ShowSessionsPerVillain(context, sessions, Villain.Jimmy);
            ShowSessionsPerVillain(context, sessions, Villain.Locke);
            ShowSessionsPerVillain(context, sessions, Villain.TheButcher);
            ShowSessionsPerVillain(context, sessions, Villain.TheDentist);
            ShowSessionsPerVillain(context, sessions, Villain.TheElephant);
            ShowSessionsPerVillain(context, sessions, Villain.Vlad);
        }

        private static void ShowSessionsPerVillain(Context context, Dictionary<string, Dictionary<Difficulty, List<SessionCount>>> sessions, Villain villain)
        {
            var heistsToList = GetAllJobsFromHeistDbAndSession(context, sessions)
                .Where(x => x.Value.IsAvailable)
                .Where(x => !(context.Args.IsHideDlc && x.Value.IsDlc))
                .Where(x => x.Value.Villain == villain)
                .ToList();

            if (!heistsToList.Any())
                return;

            Console.Write("----------------------------- ");
            WriteInColor(() => Console.WriteLine(EnumUtils.GetString(villain)), GetVillainNameColor(villain));

            foreach (var pair in heistsToList.OrderBy(x => x.Value.Name))
            {
                var jobs = sessions.ContainsKey(pair.Key) ? sessions[pair.Key] : null;
                var heist = pair.Value;

                // Console.WriteLine(FormatCountForDifficulty(Difficulty.Easy, jobs));
                PrintCountForDifficulty(Difficulty.Normal, jobs, heist);
                PrintCountForDifficulty(Difficulty.Hard, jobs, heist);
                PrintCountForDifficulty(Difficulty.Overkill, jobs, heist);
                PrintCountForDifficulty(Difficulty.Overkill145, jobs, heist);
                PrintCountForDifficulty(Difficulty.EasyWish, jobs, heist);
                PrintCountForDifficulty(Difficulty.Overkill290, jobs, heist);
                PrintCountForDifficulty(Difficulty.SmWish, jobs, heist);
                Console.Write("  ");
                FormatHeistName(heist);
                Console.WriteLine();
            }
        }

        private static ConsoleColor GetVillainNameColor(Villain villain)
        {
            return villain == Villain.Unknown ? ConsoleColor.Red : ConsoleColor.White;
        }

        private static IEnumerable<KeyValuePair<string, Heist>> GetAllJobsFromHeistDbAndSession(Context context, Dictionary<string, Dictionary<Difficulty, List<SessionCount>>> sessions)
        {
            var allHeistsInSessions = sessions.Keys.ToDictionary(x => x, x => context.HeistDb.GetHeistFromNameKey(x));
            return HeistDb.JobNames.Union(allHeistsInSessions);
        }

        private static void FormatHeistName(Heist heist)
        {
            Console.Write(heist.Name);
            if (heist.IsStealthable)
                WriteInColor(() => Console.Write("*"), ConsoleColor.DarkCyan);
            if (heist.IsDlc)
                WriteInColor(() => Console.Write(" (DLC)"), ConsoleColor.DarkYellow);
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
                Console.WriteLine($"{session.Key} => {session.Value}");
        }

        private static void ListUnknownMaps(Context context, SaveFile saveFile)
        {
            var sessions = GetPlayedSessions(saveFile);
            var counters = sessions.Select(kvp => SessionCount.FromDictKvp(kvp, context.HeistDb));
            ISet<string> allKeys = new SortedSet<string>(counters.Select(x => x.Heist.Key));

            ISet<string> allKnwonKeys = new HashSet<string>();
            foreach (var nameKey in HeistDb.DayNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in HeistDb.EscapeNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in HeistDb.JobNames.Keys) allKnwonKeys.Add(nameKey);

            foreach (var unknownKey in allKeys.Except(allKnwonKeys))
                Console.WriteLine(unknownKey);

            Console.WriteLine("done.");
        }

        private static void PrintCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty, Heist heist)
        {
            var count = GetCountForDifficulty(difficulty, sessionsByDifficulty);
            var color = count > 0 ? ConsoleColor.Gray : ColorFromDifficulty(difficulty, heist, ConsoleColor.Gray);
            WriteInColor(() => Console.Write(count.ToString().PadLeft(4)), color);
        }

        private static int GetCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty)
        {
            if (!sessionsByDifficulty.ContainsKey(difficulty)) return 0;
            var completedSessions = sessionsByDifficulty[difficulty].FirstOrDefault(x => x.SessionState == SessionState.Completed);
            return completedSessions?.Count ?? 0;
        }

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

        private static Dictionary<object, object> GetPlayedSessions(SaveFile saveFile)
        {
            var gameData = saveFile.GameData;
            var statisticsManager = (Dictionary<object, object>)gameData["StatisticsManager"];
            var sessions = (Dictionary<object, object>)statisticsManager["sessions"];
            var jobs = (Dictionary<object, object>)sessions["jobs"];
#if DEBUG
            jobs["foo_normal_completed"] = 1;
            jobs["foo_hard_completed"] = 23;
            jobs["bar_sm_wish_started"] = 3;
#endif
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

    public class Context
    {
        public HeistDb HeistDb { set; get; }
        public CmdLineHelper Args { get; set; }
    }
}
