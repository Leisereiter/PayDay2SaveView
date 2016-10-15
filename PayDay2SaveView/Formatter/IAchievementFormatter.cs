namespace PayDay2SaveView
{
    public interface IAchievementFormatter
    {
        IAchievementFormatter WithDisplayName(string displayName);
        IAchievementFormatter WithDescription(string description);
        IAchievementFormatter WithIsAchieved(bool achieved);
        IAchievementFormatter WithHeist(string heist);
        IAchievementFormatter WithVillain(Villain villain);

        void WriteHeader();
        void Write();
    }
}