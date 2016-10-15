namespace PayDay2SaveView.Utils
{
    public static class XlsFriendlyUtils
    {
        public static string Stringify(string s)
        {
            return string.Concat('"', s, '"');
        }

        public static string Stringify(bool x)
        {
            return x ? "TRUE" : "FALSE";
        }
    }
}