﻿namespace PayDay2SaveView.Actions
{
    public class ListSessionsAction : ICallable
    {
        public void Run(Context context)
        {
            var sessions = context.SaveFile.GetPlayedSessions();
            foreach (var session in sessions)
                context.Formatter.WriteRawSession(session);
        }
    }
}
