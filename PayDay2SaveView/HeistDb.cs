using System.Collections.Generic;
using System.Linq;

namespace PayDay2SaveView
{
    public class HeistDb
    {
        private static IDictionary<string, Heist> _jobNames;
        public static IDictionary<string, Heist> JobNames => _jobNames ?? (_jobNames = Heists.ToDictionary(x => x.Key, x => x));

        public Heist GetHeistFromNameKey(string key)
        {
            return JobNames.ContainsKey(key) ? JobNames[key] : new UnknownHeist(key);
        }

        public static readonly IDictionary<string, string> DayNames = new Dictionary<string, string>()
        {
            // Armored Transport
            {"arm_for", "Armored Transport: Train Heist"},
            {"arm_hcm", "Armored Transport: Downtown"},
            {"arm_cro", "Armored Transport: Crossroads"},
            {"arm_fac", "Armored Transport: Harbor"},
            {"arm_par", "Armored Transport: Park"},
            {"arm_und", "Armored Transport: Underpass"},

            // Miscellaneous
            {"roberts", "GO Bank"},
            {"kosugi", "Shadow Raid"},
            {"branchbank", "Bank Heist"},
            {"safehouse", "Safehouse"},
            {"jewelry_store", "Jewelry Store"},
            {"family", "Diamond Heist"},

            // Firestarter
            {"firestarter_1", "Firestarter Day 1"},
            {"firestarter_2", "Firestarter Day 2"},
            {"firestarter_3", "Firestarter Day 3"},

            // Rats
            {"alex_1", "Rats Day 1"},
            {"alex_2", "Rats Day 2"},
            {"alex_3", "Rats Day 3"},

            // Watchdogs
            {"watchdogs_1", "Watchdogs Day 1"},
            {"watchdogs_2", "Watchdogs Day 2"},

            // Framing Frame
            {"framing_frame_1", "Framing Frame Day 1"},
            {"framing_frame_2", "Framing Frame Day 2"},
            {"framing_frame_3", "Framing Frame Day 3"},

            // Big Oil
            {"welcome_to_the_jungle_1", "Big Oil Day 1"},
            {"welcome_to_the_jungle_2", "Big Oil Day 2"},

            // Election Day
            {"election_day_1", "Election Day Day 1"},
            {"election_day_2", "Election Day Day 2"},
            {"election_day_3", "Election Day Day 3"},
            {"election_day_3_skip1", "Election Day Day 3 (Not sure the difference)"},
            {"election_day_3_skip2", "Election Day Day 3 (Not sure the difference)"},

            // The Dentist
            {"big", "Big Bank"},

            // Vlad
            {"mallcrasher", "Mallcrasher"},
            {"nightclub", "Nightclub"},
            {"ukrainian_job", "Ukranian Job"},
            {"four_stores", "Four Stores"},

            // Hoxton Breakout
            {"hox_1", "Hoxton Breakout Day 1"},
            {"hox_2", "Hoxton Breakout Day 2"},
        };

        public static readonly IDictionary<string, string> EscapeNames = new Dictionary<string, string>
        {
            {"escape_cafe", "Escape: Cafe"},
            {"escape_cafe_day", "Escape: Cafe (Day)"},
            {"escape_park", "Escape: Park"},
            {"escape_park_day", "Escape: Park (Day)"},
            {"escape_overpass", "Escape: Overpass"},
            {"escape_overpass_night", "Escape: Overpass (Night)"},
            {"escape_street", "Escape: Street"},
            {"escape_garage", "Escape: Garage"},

            {"election_day_3", "?election_day_3"},
            // {"arm_for", "Armored Transport: Train Heist"},
            {"escape_hell", "?escape_hell"},
            {"blueharvest_3", "?blueharvest_3"},
            {"driving_escapes_industry_day", "?driving_escapes_industry_day"},
            {"driving_escapes_city_day", "?driving_escapes_city_day"}
        };

        public static readonly IList<Heist> Heists = new List<Heist>
        {
            new Heist("safehouse", "Safehouse", Villain.None),
            new Heist("chill", "Safehouse II", Villain.None),
            new Heist("chill_combat", "Safehouse Defense", Villain.None),

            // Tutorial
            new Heist("short1", "Tutorial I", Villain.None, isAvailable:false),
            new Heist("short2", "Tutorial II", Villain.None, isAvailable:false),

            // Miscellaneous
            new Heist("gallery", "Art Gallery", Villain.Bain, isStealthable:true),
            new Heist("branchbank", "Bank Heist", Villain.Bain, isStealthable:true, isAvailable:false),
            new Heist("branchbank_prof", "Bank Heist: Random", Villain.Bain, isStealthable:true),
            new Heist("branchbank_cash", "Bank Heist: Cash", Villain.Bain, isStealthable:true),
            new Heist("branchbank_deposit", "Bank Heist: Deposit", Villain.Bain, isStealthable:true),
            new Heist("branchbank_gold_prof", "Bank Heist: Gold", Villain.Bain, isStealthable:true),

            new Heist("cage", "Car Shop", Villain.Bain, isStealthable:true),
            new Heist("rat", "Cook Off", Villain.Bain),
            new Heist("family", "Diamond Store", Villain.Bain, isStealthable:true),
            new Heist("roberts", "GO Bank", Villain.Bain, isStealthable:true),
            new Heist("jewelry_store", "Jewelry Store", Villain.Bain, isStealthable:true),
            new Heist("kosugi", "Shadow Raid", Villain.Bain, isStealthable:true),
            new Heist("arena", "The Alesso Heist", Villain.Bain, isStealthable:true, isDlc:true),

            // Armored Transport
            new Heist("arm_cro", "Transport: Crossroads", Villain.Bain, isDlc:true),
            new Heist("arm_hcm", "Transport: Downtown", Villain.Bain, isDlc:true),
            new Heist("arm_fac", "Transport: Harbor", Villain.Bain, isDlc:true),
            new Heist("arm_par", "Transport: Park", Villain.Bain, isDlc:true),
            new Heist("arm_for", "Transport: Train Heist", Villain.Bain, isDlc:true),
            new Heist("arm_und", "Transport: Underpass", Villain.Bain, isDlc:true),

            // Classics
            new Heist("pal", "Counterfeit", Villain.Classics, isDlc:true),
            new Heist("red2", "First World Bank", Villain.Classics, isStealthable:true),
            new Heist("dinner", "Slaughterhouse", Villain.Classics),
            new Heist("man", "Undercover", Villain.Classics, isDlc:true),

            // Events
            new Heist("nail", "Lab Rats", Villain.Events),
            new Heist("haunted", "Safe House Nightmare", Villain.Events),

            // Hector
            new Heist("firestarter", "Firestarter", Villain.Hector, isStealthable:true),
            new Heist("firestarter_prof", "Firestarter (Pro Job)", Villain.Hector, isStealthable:true, isAvailable:false),
            new Heist("alex", "Rats", Villain.Hector),
            new Heist("alex_prof", "Rats Professional", Villain.Hector, isAvailable:false),
            new Heist("watchdogs", "Watchdogs", Villain.Hector),
            new Heist("watchdogs_night", "Watchdogs (Night)", Villain.Hector),
            new Heist("watchdogs_prof", "Watchdogs (Pro Job)", Villain.Hector, isAvailable:false),
            new Heist("watchdogs_night_prof", "Watchdogs (Night) (Pro Job)", Villain.Hector, isAvailable:false),

            // Jimmy
            new Heist("dark", "Murky Station", Villain.Jimmy, isStealthable:true),
            new Heist("mad", "Boiling Point", Villain.Jimmy),

            // Locke
            new Heist("pbr", "Beneath The Mountain", Villain.Locke, isDlc:true),
            new Heist("pbr2", "Birth of Sky", Villain.Locke, isDlc:true),

            // The Butcher
            new Heist("crojob1", "The Bomb: Dockyard", Villain.TheButcher, isStealthable:true, isDlc:true),
            new Heist("crojob2", "The Bomb: Forest", Villain.TheButcher, isDlc:true),

            // The Dentist
            new Heist("kenaz", "Golden Grin Casino", Villain.TheDentist, isStealthable:true, isDlc:true),
            new Heist("mia", "Hotline Miami", Villain.TheDentist, isDlc:true),
            new Heist("mia_prof", "Hotline Miami (Pro Job)", Villain.TheDentist, isAvailable:false),
            new Heist("hox", "Hoxton Breakout", Villain.TheDentist),
            new Heist("hox_prof", "Hoxton Breakout (Pro Job)", Villain.TheDentist, isAvailable:false),
            new Heist("hox_3", "Hoxton Revenge", Villain.TheDentist, isStealthable:true),
            new Heist("big", "The Big Bank", Villain.TheDentist, isStealthable:true, isDlc:true),
            new Heist("mus", "The Diamond", Villain.TheDentist, isStealthable:true, isDlc:true),

            // Vlad
            new Heist("jolly", "Aftershock", Villain.Vlad),
            new Heist("four_stores", "Four Stores", Villain.Vlad, isStealthable:true),
            new Heist("peta", "Goat Simulator", Villain.Vlad, isDlc:true),
            new Heist("peta_prof", "Goat Simulator (Pro Job)", Villain.Vlad, isAvailable:false, isDlc:true),
            new Heist("mallcrasher", "Mallcrasher", Villain.Vlad),
            new Heist("shoutout_raid", "Meltdown", Villain.Vlad),
            new Heist("nightclub", "Nightclub", Villain.Vlad, isStealthable:true),
            new Heist("cane", "Santa's Workshop", Villain.Vlad),
            new Heist("ukrainian_job", "Ukranian Job (Escape forces)", Villain.Vlad, isAvailable:false, isStealthable:true),
            new Heist("ukrainian_job_prof", "Ukranian Job", Villain.Vlad, isStealthable:true),
            new Heist("pines", "White Xmas", Villain.Vlad),

            // The Elephant
            new Heist("welcome_to_the_jungle", "Big Oil (Prae 109.1)", Villain.TheElephant, isAvailable:false),
            new Heist("welcome_to_the_jungle_prof", "Big Oil", Villain.TheElephant),
            new Heist("welcome_to_the_jungle_night_prof", "Big Oil (Night)", Villain.TheElephant),
            new Heist("election_day", "Election Day", Villain.TheElephant, isStealthable:true),
            new Heist("election_day_prof", "Election Day (Pro Job)", Villain.TheElephant, isStealthable:true, isAvailable:false),
            new Heist("framing_frame", "Framing Frame", Villain.TheElephant, isStealthable:true),
            new Heist("framing_frame_prof", "Framing Frame (Pro Job)", Villain.TheElephant, isStealthable:true, isAvailable:false),
            new Heist("born", "The Biker Heist", Villain.TheElephant, isDlc:true),
            new Heist("born_pro", "The Biker Heist (Pro Job)", Villain.TheElephant, isAvailable:false, isDlc:true),
        };
    };
}