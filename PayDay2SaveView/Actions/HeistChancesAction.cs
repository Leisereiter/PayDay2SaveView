using System;
using System.Collections.Generic;
using System.Linq;
using PayDay2SaveView.Entities;
using PayDay2SaveView.Utils;

namespace PayDay2SaveView.Actions
{
    public class HeistChancesAction : ICallable
    {
        public void Run(Context context)
        {
            var sessions = context.SaveFile.GetAllSessionCounts(context.HeistDb);
            var heists = sessions.GroupBy(x => x.Heist.Name, x => x)
                               .ToDictionary(x => x.Key, CreateHeistStatistic);

            foreach (var heist in heists.OrderByDescending(x => x.Value.SuccessRateAvg).ThenBy(x => x.Value.Heist.Name))
            {
                Console.WriteLine($"  {heist.Key}: c.a. {heist.Value.SuccessRateAvg:P1}");
                Console.WriteLine("════════════════════════════════════════════════════════");
                ConsoleUtils.WriteInColor(() => Console.WriteLine("        DIFF: STA END AVG   =>        $$$         XP"), ConsoleColor.DarkGray);
                WriteStatisticsPerDifficulty(Difficulty.Normal, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.Hard, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.Overkill, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.Overkill145, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.EasyWish, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.Overkill290, context, heist.Value);
                WriteStatisticsPerDifficulty(Difficulty.SmWish, context, heist.Value);
                Console.WriteLine();
            }
        }

        private static void WriteStatisticsPerDifficulty(Difficulty difficulty, Context context, HeistStatistic statistic)
        {
            var started = statistic.GetHeistsStarted(difficulty);
            var ended = statistic.GetHeistsCompleted(difficulty);

            Console.WriteLine("  {0}: {1,3:D} {2,3:D} {3,5:P0} => {4,10:N0}   {5,6:N0}XP",
                EnumUtils.GetString(difficulty).PadLeft(10),
                started, ended, statistic.GetSuccessRate(difficulty),
                statistic.GetExpectedMoney(difficulty),
                statistic.GetExpectedXp(difficulty));
        }

        private HeistStatistic CreateHeistStatistic(IGrouping<string, SessionCount> sessions)
        {
            var counters = sessions.ToList();
            var res = new HeistStatistic(counters.First().Heist, counters);
            return res;
        }
    }

    public class HeistStatistic
    {
        public Heist Heist { get; }
        private List<SessionCount> Counters { get; }

        public HeistStatistic(Heist heist, List<SessionCount> counters)
        {
            Heist = heist;
            Counters = counters;
        }

        public decimal GetExpectedMoney(Difficulty difficulty)
        {
            return 100000 * SuccessRateAvg;
        }

        public decimal GetExpectedXp(Difficulty difficulty)
        {
            return 200 * SuccessRateAvg;
        }

        public int GetHeistsStarted(Difficulty difficulty)
        {
            return SeasonsWithDifficulty(difficulty).Where(SeasonWasStarted).Sum(x => x.Count);
        }

        public int GetHeistsCompleted(Difficulty difficulty)
        {
            return SeasonsWithDifficulty(difficulty).Where(SeasonWasCompleted).Sum(x => x.Count);
        }

        public decimal GetSuccessRate(Difficulty difficulty)
        {
            var heistsStarted = GetHeistsStarted(difficulty);
            if (heistsStarted == 0) return 0;
            return (decimal)GetHeistsCompleted(difficulty) / heistsStarted;
        }

        public decimal SuccessRateAvg
        {
            get
            {
                return (GetSuccessRate(Difficulty.SmWish)
                    + GetSuccessRate(Difficulty.Overkill290)
                    + GetSuccessRate(Difficulty.EasyWish)
                    + GetSuccessRate(Difficulty.Overkill145)
                    + GetSuccessRate(Difficulty.Overkill)
                    + GetSuccessRate(Difficulty.Hard)
                    + GetSuccessRate(Difficulty.Normal)) / 7.0m;
            }
        }

        #region Helpers

        private static bool SeasonWasCompleted(SessionCount x)
        {
            return x.SessionState == SessionState.Completed;
        }

        private static bool SeasonWasStarted(SessionCount x)
        {
            return x.SessionState == SessionState.Started;
        }

        private IEnumerable<SessionCount> SeasonsWithDifficulty(Difficulty difficulty)
        {
            return Counters.Where(x => x.Difficulty == difficulty);
        }

        #endregion
    }
}
