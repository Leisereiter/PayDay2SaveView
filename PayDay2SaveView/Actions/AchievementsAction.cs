using System;
using System.Configuration;

namespace PayDay2SaveView.Actions
{
    public class AchievementsAction : ICallable
    {
        public void Run(Context context)
        {
            Console.WriteLine(GetKey());
        }

        private static string GetKey()
        {
            var encryptedKey = ConfigurationManager.AppSettings["SteamApiKey"];
            return encryptedKey;
        }
    }
}
