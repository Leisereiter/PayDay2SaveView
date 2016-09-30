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
            var jobNameResolver = new JobNameResolver();
            var steamUtils = new SteamUtils();

            var saveFile = new SaveFile(GetSaveFilePath(steamUtils));
            var sessions = GetPlayedSessions(saveFile)
                .Select(x => SessionCount.FromDictKvp(x, jobNameResolver))
                .Where(x=>x.SessionState == SessionState.Completed)
                .GroupBy(x => x.NameKey, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

            Console.Write("Heist".PadLeft(30));
            Console.Write("NO".PadLeft(4));
            Console.Write("HD".PadLeft(4));
            Console.Write("VH".PadLeft(4));
            Console.Write("OK".PadLeft(4));
            Console.Write("DW".PadLeft(4));
            Console.WriteLine();

            foreach (var name in JobNameResolver.JobNames.OrderBy(x => x.Value))
            {
                Console.Write(name.Value.PadLeft(30));
                var jobs = sessions.ContainsKey(name.Key) ? sessions[name.Key] : null;

                // Console.WriteLine(FormatCountForDifficulty(Difficulty.Easy, jobs));
                Console.Write(FormatCountForDifficulty(Difficulty.Normal, jobs));
                Console.Write(FormatCountForDifficulty(Difficulty.Hard, jobs));
                Console.Write(FormatCountForDifficulty(Difficulty.Overkill, jobs));
                Console.Write(FormatCountForDifficulty(Difficulty.Overkill145, jobs));
                Console.Write(FormatCountForDifficulty(Difficulty.Overkill290, jobs));
                Console.WriteLine();
            }
        }

        private static string FormatCountForDifficulty(Difficulty difficulty, IReadOnlyDictionary<Difficulty, SessionCount> jobs)
        {
            var count = (jobs != null && jobs.ContainsKey(difficulty)) ? jobs[difficulty].Count : 0;
            return $"{count.ToString().PadLeft(4)}";
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
