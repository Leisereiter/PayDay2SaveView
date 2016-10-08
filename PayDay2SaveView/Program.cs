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
            var arg_help = false;
            var arg_list_sessions = false;
            var arg_list_unknown_maps = false;

            IList<string> args_positional = new List<string>();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "--help":
                        arg_help = true;
                        break;

                    case "--list-unknown-maps":
                        arg_list_unknown_maps = true;
                        break;

                    case "--list-sessions":
                        arg_list_sessions = true;
                        break;
                    default:
                        args_positional.Add(arg);
                        break;
                }
            }

            if (arg_help)
            {
                Console.WriteLine("Beispiele:");
                Console.WriteLine(@"PayDay2SaveView.exe");
                Console.WriteLine(@"PayDay2SaveView.exe ""%LOCALAPPDATA%\PAYDAY 2\saves\<STEAMUSER>\save098.sav""");
                return;
            }

            var jobNameResolver = new JobNameResolver();
            var steamUtils = new SteamUtils();

            var saveFilePath = args_positional.Any() ? args_positional.First() : GetSaveFilePath(steamUtils);
            var saveFile = new SaveFile(saveFilePath);

            if (arg_list_unknown_maps)
            {
                ListUnknownMaps(saveFile, jobNameResolver);
                return;
            }

            if (arg_list_sessions)
            {
                ListallSessions(saveFile);
                return;
            }

            var sessions = GetPlayedSessions(saveFile)
                .Select(x => SessionCount.FromDictKvp(x, jobNameResolver))
                .Where(x => x.SessionState == SessionState.Completed)
                .GroupBy(x => x.NameKey, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

            Console.Write("NO".PadLeft(4));
            Console.Write("HD".PadLeft(4));
            Console.Write("VH".PadLeft(4));
            Console.Write("OK".PadLeft(4));
            Console.Write("DW".PadLeft(4));
            Console.WriteLine("  Heist");

            foreach (var name in JobNameResolver.JobNames.OrderBy(x => x.Value))
            {
                var jobs = sessions.ContainsKey(name.Key) ? sessions[name.Key] : null;

                // Console.WriteLine(FormatCountForDifficulty(Difficulty.Easy, jobs));
                PrintCountForDifficulty(Difficulty.Normal, jobs);
                PrintCountForDifficulty(Difficulty.Hard, jobs);
                PrintCountForDifficulty(Difficulty.Overkill, jobs);
                PrintCountForDifficulty(Difficulty.Overkill145, jobs);
                PrintCountForDifficulty(Difficulty.Overkill290, jobs);
                Console.WriteLine("  " + name.Value);
            }
        }

        private static void ListallSessions(SaveFile saveFile)
        {
            var sessions = GetPlayedSessions(saveFile);
            foreach (var session in sessions)
            {
                Console.WriteLine($"{session.Key} => {session.Value}");
            }
        }

        private static void ListUnknownMaps(SaveFile saveFile, JobNameResolver jobNameResolver)
        {
            var sessions = GetPlayedSessions(saveFile);
            var counters = sessions.Select(kvp => SessionCount.FromDictKvp(kvp, jobNameResolver));
            ISet<string> allKeys = new SortedSet<string>(counters.Select(x => x.NameKey));

            ISet<string> allKnwonKeys = new HashSet<string>();
            foreach (var nameKey in JobNameResolver.DayNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in JobNameResolver.EscapeNames.Keys) allKnwonKeys.Add(nameKey);
            foreach (var nameKey in JobNameResolver.JobNames.Keys) allKnwonKeys.Add(nameKey);

            foreach (var unknownKey in allKeys.Except(allKnwonKeys))
                Console.WriteLine(unknownKey);

            Console.WriteLine("done.");
        }

        private static void PrintCountForDifficulty(Difficulty difficulty, IReadOnlyDictionary<Difficulty, SessionCount> jobs)
        {
            var count = (jobs != null && jobs.ContainsKey(difficulty)) ? jobs[difficulty].Count : 0;
            var defaultFgColor = Console.ForegroundColor;
            if (count == 0) Console.ForegroundColor = ColorFromDifficulty(difficulty, defaultFgColor);
            Console.Write(count.ToString().PadLeft(4));
            Console.ForegroundColor = defaultFgColor;
        }

        private static ConsoleColor ColorFromDifficulty(Difficulty difficulty, ConsoleColor defaultColor)
        {
            switch (difficulty)
            {
                case Difficulty.Hard:
                case Difficulty.Overkill:
                case Difficulty.Overkill145:
                    return ConsoleColor.Red;

                case Difficulty.Overkill290:
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
