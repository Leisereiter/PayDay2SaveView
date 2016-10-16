using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using PayDay2SaveView.Entities;
using PayDay2SaveView.Utils;
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
                var heist = GuessHeistFromDescription(description);
                var villain = heist is UnknownHeist ? GuessVillainFromDescription(description) : heist.Villain;

                context.Formatter.Achievement()
                    .WithDisplayName(result.DisplayName.Trim())
                    .WithDescription(description.Trim())
                    .WithIsAchieved(achieved.Contains(result.Name))
                    .WithHeist(heist.Name.Trim())
                    .WithVillain(villain)
                    .WithDifficulty(GuessDifficultyFromDescription(description))
                    .Write();
            }
        }

        private static Difficulty? GuessDifficultyFromDescription(string description)
        {
            foreach (var diff in EnumUtils.GetAllDifficultiesByName())
            {
                var pos = description.IndexOf(diff.Key, StringComparison.CurrentCultureIgnoreCase);
                if (pos >= 0)
                    return diff.Value;
            }

            return null;
        }

        private static Villain GuessVillainFromDescription(string description)
        {
            foreach (var villain in EnumUtils.GetAllVillainsByName())
                if (description.Contains(villain.Key))
                    return villain.Value;
            return Villain.None;
        }

        private static Heist GuessHeistFromDescription(string description)
        {
            foreach (var heist in HeistDb.JobNames)
                if (description.Contains(heist.Value.Name))
                    return heist.Value;
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
