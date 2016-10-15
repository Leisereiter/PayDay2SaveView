using System;

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
            return string.Concat('"', s, '"');
        }

        public static string StringifyBool(bool x)
        {
            return x ? "TRUE" : "FALSE";
        }
    }
}