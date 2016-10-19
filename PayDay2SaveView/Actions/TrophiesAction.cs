using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper.Internal;

namespace PayDay2SaveView.Actions
{
    internal class TrophiesAction : ICallable
    {
        public void Run(Context context)
        {
            var trophies = GetTrophies(context).OrderBy(x => x.Nr);
            foreach (var trophy in trophies)
            {
                ConsoleUtils.WriteInColor(() => Console.Write(trophy.IsCompleted ? "TODO " : "DONE "), GetTodoColor(trophy));
                ConsoleUtils.WriteInColor(() => Console.WriteLine(trophy.Id), ConsoleColor.White);

                foreach (var objective in trophy.Objectives)
                {
                    Console.Write("     ");
                    if (objective.AchievementId != null)
                    {
                        Console.Write("Achievement ");
                        ConsoleUtils.WriteInColor(() => Console.Write(objective.AchievementId),
                            GetObjectiveColor(objective));
                    }
                    else
                    {
                        ConsoleUtils.WriteInColor(() => Console.Write($"{objective.ProgressId}"),
                            GetObjectiveColor(objective));
                        Console.Write($" progress={objective.Progress}/?");
                    }
                    Console.WriteLine();

                    if (!objective.CompletedHeists.Any()) continue;
                    
                    // heists_completed
                    ConsoleUtils.WriteInColor(() => Console.WriteLine("     Completed heists:"), ConsoleColor.DarkGray);
                    foreach (var heistNameKey in objective.CompletedHeists)
                    {
                        var heistName = context.HeistDb.GetHeistFromNameKey(heistNameKey).Name;
                        ConsoleUtils.WriteInColor(() => Console.WriteLine("     - " + heistName),
                            ConsoleColor.DarkGray);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private static ConsoleColor GetObjectiveColor(TrophyObjective objective)
        {
            return objective.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
        }

        private static ConsoleColor GetTodoColor(Trophy trophy)
        {
            return trophy.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
        }

        private static IEnumerable<Trophy> GetTrophies(Context context)
        {
            var gameData = context.SaveFile.GameData;
            return GetRawTrophies(gameData).Select(Trophy.FromDict);
        }

        private static Dictionary<object, object> GetRawTrophies(Dictionary<object, object> gameData)
        {
            if (!gameData.ContainsKey("CustomSafehouseManager"))
                throw new Exception("CustomSafehouseManager non existent");

            var csm = (Dictionary<object, object>)gameData["CustomSafehouseManager"];
            if (!csm.ContainsKey("trophies"))
                throw new Exception("Key trophies non existent");

            return (Dictionary<object, object>)csm["trophies"];
        }
    }

    public class Trophy
    {
        public int Nr { get; private set; }
        public string Id { get; private set; }
        public List<TrophyObjective> Objectives { get; private set; }

        private Trophy() { }

        public static Trophy FromDict(KeyValuePair<object, object> dict)
        {
            var value = (Dictionary<object, object>)dict.Value;

            var objectives = ((Dictionary<object, object>)value["objectives"])
                .Select(TrophyObjective.FromDict)
                .ToList();

            var trophy = new Trophy
            {
                Nr = (byte)dict.Key,
                Id = (string)value["id"],
                Objectives = objectives
            };

            return trophy;
        }

        public bool IsCompleted { get { return Objectives.All(x => x.IsCompleted); } }
    }

    public class TrophyObjective
    {
        public int Id { get; private set; }
        public bool IsCompleted { get; private set; }
        public int Progress { get; private set; }
        public string ProgressId { get; private set; }
        public string AchievementId { get; private set; }
        public IList<string> CompletedHeists { get; private set; }

        public static TrophyObjective FromDict(KeyValuePair<object, object> dict)
        {
            var value = (Dictionary<object, object>)dict.Value;

            return new TrophyObjective
            {
                Id = (byte)dict.Key,
                IsCompleted = (bool)value["completed"],
                Progress = GetProgress(value),
                ProgressId = (string)value.GetOrDefault("progress_id"),
                AchievementId = (string)value.GetOrDefault("achievement_id"),
                CompletedHeists = GetCompletedHeists(value)
            };
        }

        private static IList<string> GetCompletedHeists(Dictionary<object, object> value)
        {
            if (!value.ContainsKey("completed_heists")) return new List<string>();
            var dict = value["completed_heists"] as Dictionary<object, object>;

            Debug.Assert(dict != null, "dict != null");
            return dict.Select(x => (string)x.Value).ToList();
        }

        private static int GetProgress(IReadOnlyDictionary<object, object> value)
        {
            var progress = value["progress"];
            if (progress == null) return 0;
            if (progress is byte) return (byte)progress;
            return (short)progress;
        }
    }
}