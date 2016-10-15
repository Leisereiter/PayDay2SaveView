namespace PayDay2SaveView.Formatter
{
    public abstract class AchievementFormatterBase : IAchievementFormatter
    {
        protected string DisplayName = string.Empty;
        protected string Description = string.Empty;
        protected bool IsAchieved = false;

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

        public abstract void Write();
    }
}