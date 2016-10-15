namespace PayDay2SaveView
{
    public class StringValue : System.Attribute
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public string Value { get; }

        public StringValue(string value)
        {
            Value = value;
        }
    }
}