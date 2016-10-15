using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

            var achieved = GetAchievedApiNames(userStats, mySteamId);

            var gameStats = userStats.GetSchemaForGameAsync(Program.Pd2SteamId, Language).Result;

            context.Formatter.AchievementsBefore();

            foreach (var result in gameStats.AvailableGameStats.Achievements)
            {
                var description = result.Description;

                context.Formatter.Achievement()
                    .WithDisplayName(result.DisplayName)
                    .WithDescription(description)
                    .WithIsAchieved(achieved.Contains(result.Name))
                    .WithHeist(GuessHeistFromDescription(description).Name)
                    .WithVillain(GuessVillainFromDescription(description))
                    .Write();
            }
        }

        private static Villain GuessVillainFromDescription(string description)
        {
            return Villain.None;
        }

        private static Heist GuessHeistFromDescription(string description)
        {
            return new UnknownHeist("None");
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
