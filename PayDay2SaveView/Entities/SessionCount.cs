using System.Collections.Generic;

namespace PayDay2SaveView.Entities
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
        Easy,
        Normal,
        Hard,
        Overkill,
        Overkill145,
        EasyWish,
        Overkill290,
        SmWish
    }

    public class SessionCount
    {
        public Heist Heist { get; }
        public SessionType SessionType { get; }
        public Difficulty Difficulty { get; }
        public SessionState SessionState { get; }
        public int Count { get; }

        private SessionCount(Heist heist, SessionType sessionType, Difficulty difficulty, SessionState sessionState, int count)
        {
            Heist = heist;
            SessionType = sessionType;
            SessionState = sessionState;
            Difficulty = difficulty;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Heist.Name.PadLeft(30)} {SessionType.ToString().PadRight(7)} {Difficulty.ToString().PadRight(12)} {SessionState} : {Count}";
        }

        public static SessionCount FromDictKvp(KeyValuePair<object, object> kvp, HeistDb resolver)
        {
            var key = (string)kvp.Key;

            var keyParser = new SessionKeyParser(key);

            var state = keyParser.ReadSessionState();
            var difficulty = keyParser.ReadDifficulty();
            var nameKey = keyParser.ReadJobId();
            var heist = resolver.GetHeistFromNameKey(nameKey);

            var count = ValueHelper.ConvertToInt(kvp.Value);

            return new SessionCount(heist, SessionType.Level, difficulty, state, count);
        }
    }
}