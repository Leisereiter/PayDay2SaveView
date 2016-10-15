using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PaydaySaveEditor.PD2;
using PayDay2SaveView.Actions;

namespace PayDay2SaveView
{
    public static class Program
    {
        private const int Pd2SteamId = 218620;

        public static void Main(string[] args)
        {
            var cmdLineHelper = new CmdLineHelper(args);
            var steamUtils = new SteamUtils();
            var saveFilePath = cmdLineHelper.Positional.Any() ? cmdLineHelper.Positional.First() : GetSaveFilePath(steamUtils);

            var context = new Context
            {
                HeistDb = new HeistDb(),
                Args = cmdLineHelper,
                Formatter = ChooseFormatter(cmdLineHelper),
                SaveFile = new SaveFile(saveFilePath)
            };

            if (context.Args.IsHelp)
            {
                CmdLineHelper.PrintHelp(Console.Out);
                return;
            }

            GetCallable(context).Run(context);
        }

        private static ICallable GetCallable(Context context)
        {
            if (context.Args.IsListSessions)
                return new ListSessionsAction();
            if (context.Args.IsTreeDump)
                return new TreeDumpAction();
            if (context.Args.IsAchievements)
                return new AchievementsAction();

            return new ListHeistsAction();
        }

        private static IFormatter ChooseFormatter(CmdLineHelper cmdLineHelper)
        {
            if (cmdLineHelper.IsXls)
                return new XlsFriendlyFormatter();
            return new ConsoleFormatter();
        }

        private static string GetSaveFilePath(SteamUtils steamUtils)
        {
            var steamUsers = steamUtils.GetSteamUsersWithGame(Pd2SteamId).ToList();
            if (!steamUsers.Any())
                throw new Exception("Ich finde bei Steam keinen Benutzer der PD2 installiert hat.");

            var steamUser = steamUsers.Count() == 1 ? steamUsers.First() : PromptForSteamUser(steamUsers);

            var payday2SavePath = Path.Combine(steamUtils.GetGameDirectory(steamUser, Pd2SteamId), "remote");
            var currentSaveFile = GetLatestSaveFile(Directory.GetFiles(payday2SavePath, "save*.sav"));
            return currentSaveFile;
        }

        private static long PromptForSteamUser(IList<long> steamUsers)
        {
            Console.WriteLine("Es wurden mehrer Nutzer mit PD2 gefunden:");
            for (var i = 0; i < steamUsers.Count; ++i)
                Console.WriteLine($" [{i + 1}] {steamUsers[i]}");

            var line = Prompt();

            int choice;
            while (true)
            {
                if (int.TryParse(line, out choice) && choice >= 1 && choice <= steamUsers.Count) break;
                Console.WriteLine($"Ne, das muss schon eine Zahl zwischen 1 und {steamUsers.Count} sein.");

                line = Prompt();
            }

            return steamUsers[choice - 1];
        }

        private static string Prompt()
        {
            Console.Write(" > ");
            return Console.ReadLine();
        }

        private static string GetLatestSaveFile(IEnumerable<string> files)
        {
            var pathLastAccessTimes = files.Select(path => new { path, date = File.GetLastWriteTime(path) });
            return pathLastAccessTimes.OrderByDescending(x => x.date).First().path;
        }
    }

    public class Context
    {
        public HeistDb HeistDb { set; get; }
        public CmdLineHelper Args { get; set; }
        public IFormatter Formatter { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}
