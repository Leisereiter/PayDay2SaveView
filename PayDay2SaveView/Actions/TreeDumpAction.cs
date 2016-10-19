using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PayDay2SaveView.Actions
{
    public class TreeDumpAction : ICallable
    {
        public void Run(Context context)
        {
            PrintTreeDump(context.SaveFile.GameData, depth: 0);
        }

        private static void PrintTreeDump(Dictionary<object, object> saveFile, int depth)
        {
            var entries = saveFile
                .OrderBy(x => IsGameDataDict(x.Value))
                .ThenBy(x => x.Key.ToString());

            Console.WriteLine("{");

            int idx = 0, total = entries.Count();

            foreach (var kv in entries)
            {
                WriteWithDepth(depth + 1, $"\"{kv.Key}\": ");

                if (IsGameDataDict(kv.Value))
                    PrintTreeDump((Dictionary<object, object>)kv.Value, depth + 1);
                else
                    Console.Write(ToJsonString(kv.Value));

                if (++idx < total)
                    Console.WriteLine(",");
            }

            Console.WriteLine();
            WriteWithDepth(depth, "}");
        }

        private static object ToJsonString(object obj)
        {
            if (obj == null) return "null";
            if (obj is string) return string.Concat('"', obj, '"');
            if (obj is bool) return (bool)obj ? "true" : "false";
            if (obj is decimal) return ((decimal)obj).ToString(new CultureInfo("en-US"));
            if (obj is double) return ((double)obj).ToString(new CultureInfo("en-US"));
            if (obj is float) return ((float)obj).ToString(new CultureInfo("en-US"));
            return obj.ToString();
        }

        private static void WriteWithDepth(int depth, string format)
        {
            Console.Write(string.Concat(new string(' ', depth * 2), format));
        }

        private static bool IsGameDataDict(object x)
        {
            return x is Dictionary<object, object>;
        }
    }
}
