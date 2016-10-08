using System.Collections.Generic;
using System.Linq;

namespace PayDay2SaveView
{
    public class JobNameResolver
    {
        private static IDictionary<string, Heist> _jobNames;
        public static IDictionary<string, Heist> JobNames => _jobNames ?? (_jobNames = Heists.ToDictionary(x => x.Key, x => x));

        public string GetJobNameFromKey(string key)
        {
            if (JobNames.ContainsKey(key)) return JobNames[key].Name;
            if (DayNames.ContainsKey(key)) return DayNames[key];
            if (EscapeNames.ContainsKey(key)) return EscapeNames[key];
            return $"?{key}";
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
            new Heist("safehouse", "Safehouse"),
            new Heist("haunted", "?haunted"),

            // Tutorial
            new Heist("short1", "Tutorial I"),
            new Heist("short2", "Tutorial II"),

            // Miscellaneous
            new Heist("gallery", "Art Gallery"),
            new Heist("branchbank", "Bank Heist"),
            new Heist("branchbank_prof", "Bank Heist (Pro Job)"),
            new Heist("branchbank_cash", "Bank Heist: Cash"),
            new Heist("branchbank_deposit", "Bank Heist Deposit"),
            new Heist("branchbank_gold_prof", "Bank Heist Gold (Pro Job)"),

            new Heist("cage", "Car Shop"),
            new Heist("rat", "Cook Off"),
            new Heist("family", "Diamond Store"),
            new Heist("roberts", "GO Bank"),
            new Heist("jewelry_store", "Jewelry Store"),
            new Heist("kosugi", "Shadow Raid"),
            new Heist("arena", "The Alesso Heist"),

            // Armored Transport
            new Heist("arm_cro", "Armored Transport: Crossroads"),
            new Heist("arm_hcm", "Armored Transport: Downtown"),
            new Heist("arm_fac", "Armored Transport: Harbor"),
            new Heist("arm_par", "Armored Transport: Park"),
            new Heist("arm_for", "Armored Transport: Train Heist"),
            new Heist("arm_und", "Armored Transport: Underpass"),

            // Classics
            new Heist("pal", "Counterfeit"),
            new Heist("red2", "First World Bank"),
            new Heist("dinner", "Slaughterhouse"),
            new Heist("man", "Undercover"),

            // Events
            new Heist("nail", "Lab Rats"),

            // Hector
            new Heist("firestarter", "Firestarter"),
            new Heist("firestarter_prof", "Firestarter (Pro Job)"),
            new Heist("alex", "Rats"),
            new Heist("alex_prof", "Rats Professional"),
            new Heist("watchdogs", "Watchdogs"),
            new Heist("watchdogs_night", "Watchdogs (Night)"),
            new Heist("watchdogs_prof", "Watchdogs (Pro Job)"),
            new Heist("watchdogs_night_prof", "Watchdogs (Night) (Pro Job)"),

            // Jimmy
            new Heist("dark", "Murky Station"),
            new Heist("mad", "Boiling Point"),

            // Locke
            new Heist("pbr", "Beneath The Mountain"),
            new Heist("pbr2", "Birth of Sky"),

            // The Butcher
            new Heist("crojob1", "The Bomb: Dockyard"),
            new Heist("crojob2", "The Bomb: Forest"),

            // The Dentist
            new Heist("kenaz", "Golden Grin Casino"),
            new Heist("mia", "Hotline Miami"),
            new Heist("mia_prof", "Hotline Miami (Pro Job)"),
            new Heist("hox", "Hoxton Breakout"),
            new Heist("hox_prof", "Hoxton Breakout (Pro Job)"),
            new Heist("hox_3", "Hoxton Revenge"),
            new Heist("big", "Big Bank"),
            new Heist("mus", "The Diamond"),

            // Vlad
            new Heist("jolly", "Aftershock"),
            new Heist("four_stores", "Four Stores"),
            new Heist("peta", "Goat Simulator"),
            new Heist("peta_prof", "Goat Simulator (Pro Job)"),
            new Heist("mallcrasher", "Mallcrasher"),
            new Heist("shoutout_raid", "Meltdown"),
            new Heist("nightclub", "Nightclub"),
            new Heist("cane", "Santa's Workshop"),
            new Heist("ukrainian_job", "Ukranian Job (Escape forces)"),
            new Heist("ukrainian_job_prof", "Ukranian Job (Pro Job)"),
            new Heist("pines", "White Xmas"),

            // The Elephant
            new Heist("welcome_to_the_jungle", "Big Oil"),
            new Heist("welcome_to_the_jungle_prof", "Big Oil (Pro Job)"),
            new Heist("welcome_to_the_jungle_night_prof", "Big Oil (Night) (Pro Job)"),
            new Heist("election_day", "Election Day"),
            new Heist("election_day_prof", "Election Day (Pro Job)"),
            new Heist("framing_frame", "Framing Frame"),
            new Heist("framing_frame_prof", "Framing Frame (Pro Job)"),
            new Heist("born", "The Biker Heist"),
            new Heist("born_pro", "The Biker Heist (Pro Job)"),
        };
    };
}