namespace PayDay2SaveView
{
    public class StringValue : System.Attribute
    {
        public string Value { get; }

        public StringValue(string value)
        {
            Value = value;
        }
    }
}