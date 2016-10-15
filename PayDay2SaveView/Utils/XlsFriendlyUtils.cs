namespace PayDay2SaveView.Utils
{
    public static class XlsFriendlyUtils
    {
        public static string Stringify(object obj)
        {
            if (obj is string) return StringifyString((string)obj);
            if (obj is bool) return StringifyBool((bool)obj);
            return StringifyString(obj.ToString());
        }

        public static string StringifyString(string s)
        {
            var sanitized = s.Replace("\"", "\"\"");
            return string.Concat('"', sanitized, '"');
        }

        public static string StringifyBool(bool x)
        {
            return x ? "TRUE" : "FALSE";
        }
    }
}