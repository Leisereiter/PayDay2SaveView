namespace PayDay2SaveView
{
    public interface IAchievementFormatter
    {
        IAchievementFormatter WithDisplayName(string displayName);
        IAchievementFormatter WithDescription(string description);
        IAchievementFormatter WithIsAchieved(bool achieved);

        void WriteHeader();
        void Write();
    }
}