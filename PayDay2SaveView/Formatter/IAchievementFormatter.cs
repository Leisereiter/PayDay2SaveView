using PayDay2SaveView.Entities;

namespace PayDay2SaveView.Formatter
{
    public interface IAchievementFormatter
    {
        IAchievementFormatter WithDisplayName(string displayName);
        IAchievementFormatter WithDescription(string description);
        IAchievementFormatter WithIsAchieved(bool achieved);
        IAchievementFormatter WithHeist(string heist);
        IAchievementFormatter WithVillain(Villain villain);
        IAchievementFormatter WithDifficulty(Difficulty? difficulty);

        void WriteHeader();
        void Write();
    }
}