using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                var isAchieved = achieved.Contains(result.Name);
                if (context.Args.IsTodo && isAchieved) continue;

                var description = result.Description;
                var heist = GuessHeistFromDescription(description);
                var villain = heist is UnknownHeist ? GuessVillainFromDescription(description) : heist.Villain;

                context.Formatter.Achievement()
                    .WithDisplayName(result.DisplayName.Trim())
                    .WithDescription(description.Trim())
                    .WithIsAchieved(isAchieved)
                    .WithHeist(heist.Name.Trim())
                    .WithVillain(villain)
                    .WithDifficulty(GuessDifficultyFromDescription(description))
                    .Write();
            }
        }

        private static Difficulty? GuessDifficultyFromDescription(string description)
        {
            var difficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().OrderByDescending(x => x);
            var keywords = difficulties.Select(difficulty => new KeyValuePair<Difficulty, string>(
                difficulty, EnumUtils.GetString(difficulty) + " difficulty"))
                .ToList();
            keywords.Add(new KeyValuePair<Difficulty, string>(Difficulty.Normal, "any difficulty"));

            foreach (var pair in keywords)
                if (description.IndexOf(pair.Value, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return pair.Key;

            return null;
        }

        private static Villain GuessVillainFromDescription(string description)
        {
            return EnumUtils.GetAllVillainsByName()
                .Where(villain => description.Contains(villain.Key))
                .Select(villain => villain.Value)
                .FirstOrDefault();
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
