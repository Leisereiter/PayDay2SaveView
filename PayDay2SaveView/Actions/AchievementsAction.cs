using System.Configuration;
using SteamWebAPI2.Interfaces;

namespace PayDay2SaveView.Actions
{
    public class AchievementsAction : ICallable
    {
        private const string Language = "english";

        public void Run(Context context)
        {
            ISteamUserStats userStats = new SteamUserStats(GetKey());

            var task = userStats.GetSchemaForGameAsync(Program.Pd2SteamId, Language);
            var results = task.Result;
            foreach (var result in results.AvailableGameStats.Achievements)
            {
                var fmt = context.Formatter;

                fmt.WriteAchievementName(result.DisplayName);
                fmt.WriteAchievementDescription(result.Description);
                fmt.WriteAchievementEnd();
            }
        }

        private static string GetKey()
        {
            var encryptedKey = ConfigurationManager.AppSettings["SteamApiKey"];
            return encryptedKey;
        }
    }
}
