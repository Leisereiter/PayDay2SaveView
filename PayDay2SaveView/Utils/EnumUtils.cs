using System;
using System.Collections.Generic;
using System.Linq;
using PayDay2SaveView.Entities;

namespace PayDay2SaveView.Utils
{
    public class EnumUtils
    {
        public static string GetString(Villain villain)
        {
            switch (villain)
            {
                case Villain.None: return "None";
                case Villain.Unknown: return "UNKNOWN HEISTS";
                case Villain.Bain: return "Bain";
                case Villain.Classics: return "Classics";
                case Villain.Events: return "Events";
                case Villain.Hector: return "Hector";
                case Villain.Jimmy: return "Jimmy";
                case Villain.Locke: return "Locke";
                case Villain.TheButcher: return "The Butcher";
                case Villain.TheDentist: return "The Dentist";
                case Villain.TheElephant: return "The Elephant";
                case Villain.Vlad: return "Vlad";

                default:
                    throw new ArgumentOutOfRangeException(nameof(villain), villain, null);
            }
        }

        public static string GetString(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy: return "Easy";
                case Difficulty.Normal: return "Normal";
                case Difficulty.Hard: return "Hard";
                case Difficulty.Overkill: return "Very Hard";
                case Difficulty.Overkill145: return "Overkill";
                case Difficulty.EasyWish: return "Mayhem";
                case Difficulty.Overkill290: return "Death Wish";
                case Difficulty.SmWish: return "One Down";

                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        public static IEnumerable<KeyValuePair<string, Villain>> GetAllVillainsByName()
        {
            var villains = Enum.GetValues(typeof(Villain)).Cast<Villain>();
            return villains.Select(x => new KeyValuePair<string, Villain>(GetString(x), x));
        }

        public static IEnumerable<KeyValuePair<string, Difficulty>> GetAllDifficultiesByName()
        {
            var difficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
            return difficulties.Select(x => new KeyValuePair<string, Difficulty>(GetString(x), x));
        }
    }
}
