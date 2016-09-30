using System.Collections.Generic;

namespace PayDay2SaveView
{
    public enum SessionType
    {
        Day,
        Level
    }

    public enum SessionState
    {
        Started,
        Completed,
        Dropin,
        Failed,
        StartedDropin,
        FailedDropin,
        CompletedDropin
    }

    public enum Difficulty
    {
        [StringValue("Easy")]
        Easy,

        [StringValue("Normal")]
        Normal,

        [StringValue("Hard")]
        Hard,

        [StringValue("Very Hard")]
        Overkill,

        [StringValue("Overkill")]
        Overkill145,

        [StringValue("Death Wish")]
        Overkill290
    }

    public class SessionCount
    {
        public string NameKey { get; }
        public string Name { get; }
        public SessionType SessionType { get; }
        public Difficulty Difficulty { get; }
        public SessionState SessionState { get; }
        public int Count { get; }

        private SessionCount(string nameKey, string name, SessionType sessionType, Difficulty difficulty, SessionState sessionState, int count)
        {
            NameKey = nameKey;
            Name = name;
            SessionType = sessionType;
            SessionState = sessionState;
            Difficulty = difficulty;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Name.PadLeft(30)} {SessionType.ToString().PadRight(7)} {Difficulty.ToString().PadRight(12)} {SessionState} : {Count}";
        }

        public static SessionCount FromDictKvp(KeyValuePair<object, object> kvp, JobNameResolver resolver)
        {
            var key = (string)kvp.Key;

            var keyParser = new SessionKeyParser(key);

            var state = keyParser.ReadSessionState();
            var difficulty = keyParser.ReadDifficulty();
            var nameKey = keyParser.ReadJobId();
            var name = resolver.GetJobNameFromKey(nameKey);

            var sessionType = PayDay2SaveView.SessionType.Level;

            var count = ValueHelper.ConvertToInt(kvp.Value);

            return new SessionCount(nameKey, name, sessionType, difficulty, state, count);
        }
    }
}