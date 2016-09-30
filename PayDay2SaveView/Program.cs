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
                .GroupBy(x => x.NameKey, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y
                                                .ToList()));

            foreach (var name in JobNameResolver._jobNames.OrderBy(x => x.Value))
            {
                Console.WriteLine(name.Value);
                var jobs = sessions.ContainsKey(name.Key) ? sessions[name.Key] : null;

                // Console.WriteLine(FormatCountForDifficulty(Difficulty.Easy, jobs));
                Console.WriteLine(FormatCountForDifficulty(Difficulty.Normal, jobs));
                Console.WriteLine(FormatCountForDifficulty(Difficulty.Hard, jobs));
                Console.WriteLine(FormatCountForDifficulty(Difficulty.Overkill, jobs));
                Console.WriteLine(FormatCountForDifficulty(Difficulty.Overkill145, jobs));
                Console.WriteLine(FormatCountForDifficulty(Difficulty.Overkill290, jobs));
                Console.WriteLine();
            }
        }

        private static string FormatCountForDifficulty(Difficulty difficulty, IReadOnlyDictionary<Difficulty, List<SessionCount>> jobs)
        {
            var count = (jobs != null && jobs.ContainsKey(difficulty)) ? jobs[difficulty].Count : 0;
            return $"  {count.ToString().PadLeft(3)} {EnumUtils.GetStringValue(difficulty)}";
        }

        private static Dictionary<object, object> GetPlayedSessions(SaveFile saveFile)
        {
            var gameData = saveFile.GameData;
            var statisticsManager = (Dictionary<object, object>)gameData["StatisticsManager"];
            var sessions = (Dictionary<object, object>)statisticsManager["sessions"];
            var jobs = (Dictionary<object, object>)sessions["jobs"];
            return jobs;
        }

        private static void PrintTree(Dictionary<string, object> tree, int depth = 0)
        {
            foreach (var item in tree)
            {
                Console.Write(new string(' ', depth));
                Console.Write($@"{item.Key}: ");

                var subTree = item.Value as Dictionary<string, object>;
                if (subTree == null)
                {
                    Console.WriteLine(item.Value);
                }
                else
                {
                    Console.WriteLine();
                    Program.PrintTree(subTree, depth + 1);
                }
            }
        }

        private static string GetSaveFilePath(SteamUtils steamUtils)
        {
            var steamUsers = steamUtils.GetSteamUser();
            var steamUser = steamUsers;
            var payday2SavePath = Path.Combine(steamUtils.GetGameDirectory(steamUser, Pd2SteamId), "remote");
            var currentSaveFile = GetCurrectSaveFile(Directory.GetFiles(payday2SavePath, "save*.sav"));
            return currentSaveFile;
        }

        private static string GetCurrectSaveFile(IEnumerable<string> files)
        {
            var pathLastAccessTimes = files.Select(path => new
            {
                path = path,
                date = File.GetLastWriteTime(path)
            });

            return pathLastAccessTimes.OrderByDescending(x => x.date).First().path;
        }
    }
}
