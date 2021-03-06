using PayDay2SaveView.Entities;

namespace PayDay2SaveView.Utils
{
    public static class XlsFriendlyUtils
    {
        public static string Stringify(object obj)
        {
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (obj is string) return StringifyString((string)obj);
            if (obj is bool) return StringifyBool((bool)obj);
            if (obj is Villain) return StringifyString(EnumUtils.GetString((Villain)obj));
            if (obj is Difficulty) return StringifyString(EnumUtils.GetString((Difficulty)obj));
            if (obj == null) return StringifyString(string.Empty);
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