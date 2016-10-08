using System.Collections.Generic;

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
    }
}