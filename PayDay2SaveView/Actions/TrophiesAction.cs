using System;
using System.Collections.Generic;
using System.Linq;
using PayDay2SaveView.Entities;

namespace PayDay2SaveView.Actions
{
    internal class TrophiesAction : ICallable
    {
        public void Run(Context context)
        {
            var trophies = GetTrophies(context);
            if (context.Args.IsTodo) trophies = trophies.Where(x => !x.IsCompleted);
            trophies = trophies.OrderBy(x => x.Nr);

            foreach (var trophy in trophies)
            {
                ConsoleUtils.WriteInColor(() => Console.Write(trophy.IsCompleted ? "TODO " : "DONE "), GetTodoColor(trophy));
                ConsoleUtils.WriteInColor(() => Console.WriteLine(trophy.Id), ConsoleColor.White);

                foreach (var objective in trophy.Objectives)
                {
                    Console.Write("     ");
                    if (objective.AchievementId != null)
                    {
                        ConsoleUtils.WriteInColor(() => Console.Write("Achievement "), ConsoleColor.DarkGray);
                        ConsoleUtils.WriteInColor(() => Console.Write(objective.AchievementId),
                            GetObjectiveColor(objective));
                    }
                    else
                    {
                        ConsoleUtils.WriteInColor(() => Console.Write($"{objective.ProgressId}"), GetObjectiveColor(objective));
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

        private static Dictionary<object, object> GetRawTrophies(IReadOnlyDictionary<object, object> gameData)
        {
            if (!gameData.ContainsKey("CustomSafehouseManager"))
                throw new Exception("CustomSafehouseManager non existent");

            var csm = (Dictionary<object, object>)gameData["CustomSafehouseManager"];
            if (!csm.ContainsKey("trophies"))
                throw new Exception("Key trophies non existent");

            return (Dictionary<object, object>)csm["trophies"];
        }
    }
}