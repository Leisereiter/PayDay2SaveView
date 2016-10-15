using System;

namespace PayDay2SaveView.Formatter
{
    public abstract class AchievementFormatterBase : IAchievementFormatter
    {
        protected string DisplayName = string.Empty;
        protected string Description = string.Empty;
        protected bool IsAchieved = false;
        protected string HeistName = string.Empty;
        protected Villain Villain = Villain.None;
        protected Difficulty? Difficulty;

        public IAchievementFormatter WithDisplayName(string displayName)
        {
            DisplayName = displayName;
            return this;
        }

        public IAchievementFormatter WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public IAchievementFormatter WithIsAchieved(bool achieved)
        {
            IsAchieved = achieved;
            return this;
        }

        public IAchievementFormatter WithHeist(string heist)
        {
            HeistName = heist;
            return this;
        }

        public IAchievementFormatter WithVillain(Villain villain)
        {
            Villain = villain;
            return this;
        }

        public IAchievementFormatter WithDifficulty(Difficulty? difficulty)
        {
            Difficulty = difficulty;
            return this;
        }

        public abstract void WriteHeader();

        public abstract void Write();
    }
}