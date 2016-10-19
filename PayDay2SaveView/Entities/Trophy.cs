using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper.Internal;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace PayDay2SaveView.Entities
{
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
}