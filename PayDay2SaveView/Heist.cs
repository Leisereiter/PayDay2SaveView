namespace PayDay2SaveView
{
    public enum Villain
    {
        None,
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
        public Villain Villain { get; private set; }
        public bool IsAvailable { get; private set; }

        public Heist(string key, string name, Villain villain, bool isAvailable = true)
        {
            Key = key;
            Name = name;
            Villain = villain;
            IsAvailable = isAvailable;
        }
    }
}