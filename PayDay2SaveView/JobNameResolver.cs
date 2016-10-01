﻿using System.Collections.Generic;

namespace PayDay2SaveView
{
    public class JobNameResolver
    {
        public string GetJobNameFromKey(string id)
        {
            if (JobNames.ContainsKey(id))
                return JobNames[id];
            return DayNames[id];
        }

        public static readonly IReadOnlyDictionary<string, string> DayNames = new Dictionary<string, string>()
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

        public static readonly IReadOnlyDictionary<string, string> EscapeNames = new Dictionary<string, string>
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

        // siehe levelstweakdata.lua
        public static readonly IReadOnlyDictionary<string, string> JobNames = new Dictionary<string, string>()
        {
            { "safehouse", "Safehouse" },
            { "haunted", "?haunted" }, // Haloween special?

            // Miscellaneous
            { "gallery", "Art Gallery" },
            { "branchbank", "Bank Heist" },
            { "branchbank_prof", "Bank Heist (Pro Job)" },
            { "branchbank_cash", "Bank Heist: Cash" },
            { "branchbank_deposit", "Bank Heist Deposit" },
            { "branchbank_gold_prof", "Bank Heist Gold (Pro Job)" },

            { "cage", "Car Shop" },
            { "rat", "Cook Off" },
            { "family", "Diamond Store" },
            { "roberts", "GO Bank" },
            { "jewelry_store", "Jewelry Store" },
            { "kosugi", "Shadow Raid" },
            { "arena", "The Alesso Heist" },
            
            // Armored Transport
            { "arm_cro", "Armored Transport: Crossroads" },
            { "arm_hcm", "Armored Transport: Downtown" },
            { "arm_fac", "Armored Transport: Harbor" },
            { "arm_par", "Armored Transport: Park" },
            { "arm_for", "Armored Transport: Train Heist" },
            { "arm_und", "Armored Transport: Underpass" },

            // Classics
            { "pal", "Counterfeit" },
            { "red2", "First World Bank" },
            { "dinner", "Slaughterhouse" },
            { "man", "Undercover" },

            // Events
            { "nail", "Lab Rats" },

            // Hector
            { "firestarter", "Firestarter" },
            { "firestarter_prof", "Firestarter (Pro Job)" },
            { "alex", "Rats" },
            { "alex_prof", "Rats Professional" },
            { "watchdogs", "Watchdogs" },
            { "watchdogs_night", "Watchdogs (Night)" },
            { "watchdogs_prof", "Watchdogs (Pro Job)" },
            { "watchdogs_night_prof", "Watchdogs (Night) (Pro Job)" },

            // Jimmy
            { "dark", "Murky Station" },
            { "mad", "Boiling Point" },

            // Locke
            { "pbr", "Beneath The Mountain" },
            { "pbr2", "Birth of Sky" },

            // The Butcher
            { "crojob1", "The Bomb: Dockyard" },
            { "crojob2", "The Bomb: Forest" },

            // The Dentist
            { "kenaz", "Golden Grin Casino" },
            { "mia", "Hotline Miami" },
            { "mia_prof", "Hotline Miami (Pro Job)" },
            { "hox", "Hoxton Breakout"},
            { "hox_prof", "Hoxton Breakout (Pro Job)"},
            { "hox_3", "Hoxton Revenge" },
            { "big", "Big Bank" },
            { "mus", "The Diamond" },

            // Vlad
            { "jolly", "Aftershock" },
            { "four_stores", "Four Stores"},
            { "peta", "Goat Simulator" },
            { "peta_prof", "Goat Simulator (Pro Job)" },
            { "mallcrasher", "Mallcrasher"},
            { "shoutout_raid", "Meltdown" },
            { "nightclub", "Nightclub"},
            { "cane", "Santa's Workshop"},
            { "ukrainian_job", "Ukranian Job (Escape forces)"},
            { "ukrainian_job_prof", "Ukranian Job (Pro Job)"},
            { "pines", "White Xmas" },

            // The Elephant
            { "welcome_to_the_jungle", "Big Oil"},
            { "welcome_to_the_jungle_prof", "Big Oil (Pro Job)"},
            { "welcome_to_the_jungle_night_prof", "Big Oil (Night) (Pro Job)" },
            { "election_day", "Election Day" },
            { "election_day_prof", "Election Day (Pro Job)" },
            { "framing_frame", "Framing Frame" },
            { "framing_frame_prof", "Framing Frame (Pro Job)" },
            { "born", "The Biker Heist" },
            { "born_pro", "The Biker Heist (Pro Job)" },
        };
    }
}