namespace PayDay2SaveView
{
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