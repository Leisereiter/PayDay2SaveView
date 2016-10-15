using System;

namespace PayDay2SaveView
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
    }
}
