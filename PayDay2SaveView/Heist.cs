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

        Unknown = 999999
    }

    public class Heist
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Villain Villain { get; private set; }
        public bool IsStealthable { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsDlc { get; private set; }

        public Heist(string key, string name, Villain villain, bool isStealthable=false, bool isAvailable = true, bool isDlc=false)
        {
            Key = key;
            Name = name;
            Villain = villain;
            IsStealthable = isStealthable;
            IsAvailable = isAvailable;
            IsDlc = isDlc;
        }
    }

    public class UnknownHeist : Heist
    {
        public UnknownHeist(string key) : base(key, key, Villain.Unknown)
        {

        }
    }
}