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
            var cmdLineHelper = new CmdLineHelper(args);

            var context = new Context
            {
                HeistDb = new HeistDb(),
                Args = cmdLineHelper,
                Formatter = ChooseFormatter(cmdLineHelper)
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
                ListallSessions(saveFile, context);
                return;
            }

            var sessions = GetPlayedSessions(saveFile)
                .Select(x => SessionCount.FromDictKvp(x, context.HeistDb))
                .GroupBy(x => x.Heist.Key, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.ToList()));

            context.Formatter.Begin();

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

            context.Formatter.End();
        }

        private static IFormatter ChooseFormatter(CmdLineHelper cmdLineHelper)
        {
            if (cmdLineHelper.IsXls)
            {
                throw new NotImplementedException("Noch nicht, sorry.");
            }

            return new ConsoleFormatter();
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

            context.Formatter.WriteVillainBegin(villain);

            foreach (var pair in heistsToList.OrderBy(x => x.Value.Name))
            {
                var jobs = sessions.ContainsKey(pair.Key) ? sessions[pair.Key] : null;
                var heist = pair.Value;

                PrintCountForDifficulty(Difficulty.Normal, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.Hard, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.Overkill, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.Overkill145, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.EasyWish, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.Overkill290, jobs, heist, context);
                PrintCountForDifficulty(Difficulty.SmWish, jobs, heist, context);
                context.Formatter.WriteHeistName(heist);
            }
        }

        private static IEnumerable<KeyValuePair<string, Heist>> GetAllJobsFromHeistDbAndSession(Context context, Dictionary<string, Dictionary<Difficulty, List<SessionCount>>> sessions)
        {
            var allHeistsInSessions = sessions.Keys.ToDictionary(x => x, x => context.HeistDb.GetHeistFromNameKey(x));
            return HeistDb.JobNames.Union(allHeistsInSessions);
        }

        private static void ListallSessions(SaveFile saveFile, Context context)
        {
            var sessions = GetPlayedSessions(saveFile);
            foreach (var session in sessions)
                context.Formatter.WriteRawSession(session);
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
                context.Formatter.UnknownKeyRaw(unknownKey);
            context.Formatter.UnknownKeysEnd();
        }

        private static void PrintCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty, Heist heist, Context context)
        {
            var count = GetCountForDifficulty(difficulty, sessionsByDifficulty);
            context.Formatter.WriteCounter(count, difficulty, heist);
        }

        private static int GetCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty)
        {
            if (!sessionsByDifficulty.ContainsKey(difficulty)) return 0;
            var completedSessions = sessionsByDifficulty[difficulty].FirstOrDefault(x => x.SessionState == SessionState.Completed);
            return completedSessions?.Count ?? 0;
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
        public IFormatter Formatter { get; set; }
    }
}
