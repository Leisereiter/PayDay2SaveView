namespace PayDay2SaveView
{
    public enum Villain
    {
        Bain,
        Classics,
        Events,
        Hector,
        Jimmy,
        Locke,
        TheButcher,
        TheDentist,
        TheElephant,
        Vlad,
    }

    public class Heist
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public bool IsAvailable { get; private set; }

        public Heist(string key, string name, bool IsAvailable = true)
        {
            Key = key;
            Name = name;
            this.IsAvailable = IsAvailable;
        }
    }
}