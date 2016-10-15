using System;

namespace PayDay2SaveView
{
    public static class ValueHelper
    {
        public static int ConvertToInt(object x)
        {
            if (x is byte) return (byte)x;
            if (x is short) return (short)x;
            if (x is int) return (int)x;

            throw new NotImplementedException();
        }
    }
}