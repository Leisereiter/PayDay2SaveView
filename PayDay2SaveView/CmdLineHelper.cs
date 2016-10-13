using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace PayDay2SaveView
{
    public class CmdLineHelper
    {
        [Argument("help", "Ähm, ja.")]
        public bool IsHelp { get; private set; }

        [Argument("list-sessions", "Listet alle Name-keys die nicht dem JobNameResolver bekannt sind")]
        public bool IsListSessions { get; private set; }

        [Argument("hide-dlc", "Verstecke Heists die nur durch DLCs zugänglich sind")]
        public bool IsHideDlc { get; private set; }

        [Argument("xls", "Excel-freundliche Ausgabe, falls verfügbar.")]
        public bool IsXls { get; private set; }

        [Argument("tree-dump", "Gibt das kompletet Savefile als Baum aus")]
        public bool IsTreeDump { get; private set; }

        public IList<string> Positional { get; }

        private CmdLineHelper()
        {
            Positional = new List<string>();
        }

        public CmdLineHelper(IEnumerable<string> args) : this()
        {
            Parse(args);
        }

        private void Parse(IEnumerable<string> args)
        {
            var flagTable = CreateFlagTable();

            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    var name = arg.Substring(2);
                    if (!flagTable.ContainsKey(name)) throw new Exception($"Unbekannter Parameter {name}");
                    flagTable[name].SetValue(this, true, null);
                }
                else
                {
                    Positional.Add(arg);
                }
            }
        }

        public static void PrintHelp(TextWriter writer)
        {
            writer.WriteLine("Aufruf: ");
            writer.WriteLine("  PayDay2SaveView.exe [FLAGS]");
            writer.WriteLine("  PayDay2SaveView.exe [FLAGS] [SAVPATH]");
            writer.WriteLine();

            writer.WriteLine("Beispiele:");
            writer.WriteLine(@"  PayDay2SaveView.exe");
            writer.WriteLine(@"  PayDay2SaveView.exe ""%LOCALAPPDATA%\PAYDAY 2\saves\<STEAMUSER>\save098.sav""");
            writer.WriteLine();

            writer.WriteLine("Flags:");
            foreach (var member in typeof(CmdLineHelper).GetMembers())
            {
                var desc = GetAttibutesOfType<ArgumentAttribute>(member).FirstOrDefault();
                if (desc != null)
                    writer.WriteLine($"  --{desc.Name.PadRight(20)} {desc.Description}");
            }
        }

        private static IEnumerable<T> GetAttibutesOfType<T>(MemberInfo member)
        {
            return member.GetCustomAttributes(typeof(T), false).Cast<T>();
        }

        private static IDictionary<string, PropertyInfo> CreateFlagTable()
        {
            IDictionary<string, PropertyInfo> flagTable = new Dictionary<string, PropertyInfo>();
            foreach (var propertyInfo in typeof(CmdLineHelper).GetProperties())
            {
                var desc = GetAttibutesOfType<ArgumentAttribute>(propertyInfo).FirstOrDefault();
                if (desc == null) continue;
                flagTable.Add(desc.Name, propertyInfo);
            }
            return flagTable;
        }
    }
}