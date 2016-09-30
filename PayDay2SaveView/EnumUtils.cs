using System;

namespace PayDay2SaveView
{
    public static class EnumUtils
    {
        public static string GetStringValue(Difficulty x)
        {
            switch (x)
            {
                case Difficulty.Easy:
                    return "Easy";

                case Difficulty.Normal:
                    return "Normal";

                case Difficulty.Hard:
                    return "Hard";

                case Difficulty.Overkill:
                    return "Very Hard";

                case Difficulty.Overkill145:
                    return "Overkill";

                case Difficulty.Overkill290:
                    return "Death Wish";

                default:
                    throw new ArgumentOutOfRangeException(nameof(x), x, null);
            }
        }
    }
}