using System.Configuration;
using SteamWebAPI2.Interfaces;

namespace PayDay2SaveView.Actions
{
    public class AchievementsAction : ICallable
    {
        private const string Language = "english";

        public void Run(Context context)
        {
            var mySteamId = GetMySteamId();

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

        private static ISet<string> GetAchievedApiNames(ISteamUserStats userStats, long playerSteamId)
        {
            var playerStats = userStats.GetPlayerAchievementsAsync(Program.Pd2SteamId, playerSteamId).Result;
            var achieved = playerStats.Achievements.Where(x => x.Achieved == 1);
            return new HashSet<string>(achieved.Select(x => x.APIName));
        }

        private static long GetMySteamId()
        {
            return long.Parse(ConfigurationManager.AppSettings["MySteamId"]);
        }

        private static string GetKey()
        {
            var encryptedKey = ConfigurationManager.AppSettings["SteamApiKey"];
            return encryptedKey;
        }
    }
}
