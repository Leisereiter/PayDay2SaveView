using System;
using System.Collections.Generic;
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

            foreach (var kv in entries)
            {
                if (depth > 0)
                {
                    var padding = String.Concat(new string(' ', depth * 2), "- ");
                    Console.Write(padding);
                }

                if (IsGameDataDict(kv.Value))
                {
                    Console.WriteLine($"{kv.Key}:");
                    PrintTreeDump((Dictionary<object, object>)kv.Value, depth + 1);
                }
                else
                {
                    Console.WriteLine($"{kv.Key}: {kv.Value}");
                }
            }
        }

        private static bool IsGameDataDict(object x)
        {
            return x is Dictionary<object, object>;
        }
    }
}
