using System;
using System.Collections.Generic;
using System.Linq;
using PayDay2SaveView.Entities;

namespace PayDay2SaveView.Actions
{
    public class ListHeistsAction : ICallable
    {
        public void Run(Context context)
        {
            var sessions = context.SaveFile.GetAllSessionCounts(context.HeistDb)
                .GroupBy(x => x.Heist.Key, x => x)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Difficulty, y => y)
                                                .ToDictionary(y => y.Key, y => y.ToList()));

            context.Formatter.Begin();

            ShowSessionsPerVillain(context, sessions, Villain.Unknown);
            ShowSessionsPerVillain(context, sessions, Villain.None);
            ShowSessionsPerVillain(context, sessions, Villain.Bain);
            ShowSessionsPerVillain(context, sessions, Villain.Classics);
            ShowSessionsPerVillain(context, sessions, Villain.Events);
            ShowSessionsPerVillain(context, sessions, Villain.Hector);
            ShowSessionsPerVillain(context, sessions, Villain.Jimmy);
            ShowSessionsPerVillain(context, sessions, Villain.Locke);
            ShowSessionsPerVillain(context, sessions, Villain.TheButcher);
            ShowSessionsPerVillain(context, sessions, Villain.Continental);
            ShowSessionsPerVillain(context, sessions, Villain.TheDentist);
            ShowSessionsPerVillain(context, sessions, Villain.TheElephant);
            ShowSessionsPerVillain(context, sessions, Villain.Vlad);

            context.Formatter.End();
        }

        private static IEnumerable<KeyValuePair<string, Heist>> GetAllJobsFromHeistDbAndSession(Context context, Dictionary<string, Dictionary<Difficulty, List<SessionCount>>> sessions)
        {
            var allHeistsInSessions = sessions.Keys.ToDictionary(x => x, x => context.HeistDb.GetHeistFromNameKey(x));
            return HeistDb.JobNames.Union(allHeistsInSessions);
        }

        private static void PrintCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty, Heist heist, Context context)
        {
            var count = GetCountForDifficulty(difficulty, sessionsByDifficulty);
            context.Formatter.WriteCounter(count, difficulty, heist);
        }

        private static int GetCountForDifficulty(Difficulty difficulty, IDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty)
        {
            if (sessionsByDifficulty == null || !sessionsByDifficulty.ContainsKey(difficulty)) return 0;
            var completedSessions = sessionsByDifficulty[difficulty].FirstOrDefault(x => x.SessionState == SessionState.Completed);
            return completedSessions?.Count ?? 0;
        }

        private static void ShowSessionsPerVillain(Context context, Dictionary<string, Dictionary<Difficulty, List<SessionCount>>> sessions, Villain villain)
        {
            var heistsQuery = GetAllJobsFromHeistDbAndSession(context, sessions)
                .Where(x => x.Value.IsAvailable)
                .Where(x => !(context.Args.IsHideDlc && x.Value.IsDlc))
                .Where(x => x.Value.Villain == villain);

            if (context.Args.IsTodo)
            {
                var heistDoneInAllDifficulties = new HashSet<string>(sessions.Where(x => HeistIsDone(x.Value)).Select(x => x.Key));
                heistsQuery = heistsQuery.Where(x => !heistDoneInAllDifficulties.Contains(x.Key));
            }

            var heistsToList = heistsQuery.ToList();

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
                context.Formatter.WriteHeistVillain(heist.Villain);
                context.Formatter.WriteHeistIsInDlc(heist.IsDlc);
                context.Formatter.WriteHeistEnd();
            }
        }

        private static bool HeistIsDone(Dictionary<Difficulty, List<SessionCount>> sessionsByDifficulty)
        {
            var activeDifficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().Where(x => x >= Difficulty.Normal);
            return activeDifficulties.All(difficulty => SessionCompletedOnDifficulty(sessionsByDifficulty, difficulty));
        }

        private static bool SessionCompletedOnDifficulty(IReadOnlyDictionary<Difficulty, List<SessionCount>> sessionsByDifficulty, Difficulty difficulty)
        {
            if (!sessionsByDifficulty.ContainsKey(difficulty)) return false;
            var session = sessionsByDifficulty[difficulty].FirstOrDefault(s => s.SessionState == SessionState.Completed);
            return session?.Count > 0;
        }
    }
}
