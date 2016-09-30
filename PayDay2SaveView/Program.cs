using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PaydaySaveEditor.PD2;

namespace PayDay2SaveView
{
    public static class Program
    {
        private const string Pd2SteamId = "218620";

        public static void Main(string[] args)
        {
            var jobNameResolver = new JobNameResolver();

            var saveFile = new SaveFile(GetSaveFilePath());
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
                    PrintTree(subTree, depth + 1);
                }
            }
        }

        private static string GetSaveFilePath()
        {
            //return @"D:\temp\out\save098.sav";
            var steamFolder = GetSteamFolder();
            if (string.IsNullOrEmpty(steamFolder))
                throw new Exception("Steam nicht gefunden :(");

            var userDataPath = Path.Combine(steamFolder, "userdata", GetSteamUser(steamFolder), Pd2SteamId, "remote");
            var currentSaveFile = GetCurrectSaveFile(Directory.GetFiles(userDataPath, "save*.sav"));
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

        private static string GetSteamUser(string steamFolder)
        {
            // HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\Users
            var hcu = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Users");
            var uids = hcu.GetSubKeyNames();
            if (uids.Length > 1)
                throw new NotImplementedException();
            return uids.First();
        }

        private static string GetSteamFolder()
        {
            // HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\SteamPath
            return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null);
        }
    }
}
