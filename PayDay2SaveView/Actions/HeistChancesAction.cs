using AutoMapper;

namespace PayDay2SaveView.Actions
{
    public class HeistChancesAction : ICallable
    {
        public void Run(Context context)
        {
            var sessions = context.SaveFile.GetPlayedSessions();
        }
    }
}
