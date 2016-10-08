using System.Collections.Generic;
using System.IO;

namespace PayDay2SaveView
{
    public class CmdLineHelper
    {
        public bool IsHelp { get; private set; }
        public bool IsListSessions { get; private set; }
        public bool IsListUnknownMaps { get; private set; }
        public IList<string> Positional { get; }

        public CmdLineHelper()
        {
            Positional = new List<string>();
        }

        public void Parse(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "--help":
                        IsHelp = true;
                        break;

                    case "--list-unknown-maps":
                        IsListUnknownMaps = true;
                        break;

                    case "--list-sessions":
                        IsListSessions = true;
                        break;
                    default:
                        Positional.Add(arg);
                        break;
                }
            }
        }

        public void PrintHelp(TextWriter writer)
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
            writer.WriteLine(@"  --help                Ähm, ja.");
            writer.WriteLine(@"  --list-unknown-maps   Listet alle Name-keys die nicht dem JobNameResolver bekannt sind.");
            writer.WriteLine(@"  --list-sessions       Listet die gespielten Sessions und deren Anzhl als rohdaten.");
        }
    }
}